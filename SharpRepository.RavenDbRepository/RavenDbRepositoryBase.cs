using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.RavenDbRepository
{
    public class RavenDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        protected DocumentStore DocumentStore;
        protected IDocumentSession Session;

        internal RavenDbRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {
            Initialize();
        }

        internal RavenDbRepositoryBase(string url, ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {
            Initialize(new DocumentStore { Url = url });
        }

        internal RavenDbRepositoryBase(DocumentStore documentStore, ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {
            Initialize(documentStore);
        }  

        private void Initialize(DocumentStore documentStore = null)
        {
            DocumentStore = documentStore ?? new DocumentStore { Url = "http://localhost:8080"};
            DocumentStore.Initialize();
            
            // see if we need to change the type name that defaults to Id
            var propInfo = GetPrimaryKeyPropertyInfo();
            if (propInfo != null && propInfo.Name != "Id")
            {
                // TODO: check this out
                // this is a global convention so will be used regardless of the entity type that is accessing the document store
                //  this may or may not be a problem since the repository creates this and it's for a single entity type
                //  we will need to test this, especially when 2 different repositories are instantiated at the same time
                DocumentStore.Conventions.FindIdentityProperty = p => p.Name == propInfo.Name;
            }

            // when upgrading to the new RavenDb.Client the following error is thrown when using an int as the PK
            //  System.InvalidOperationException : Attempt to query by id only is blocked, you should use call session.Load("RavenTestIntKeys/1"); instead of session.Query().Where(x=>x.Id == "RavenTestIntKeys/1");
            //      You can turn this error off by specifying documentStore.Conventions.AllowQueriesOnId = true;, but that is not recommend and provided for backward compatibility reasons only.
            //  So for now we will follow that advice and turn on the old convention
            // TODO: look at using a new way of doing the GetQuery to not have this issue when the PK is an int
            DocumentStore.Conventions.AllowQueriesOnId = true;

            Session = DocumentStore.OpenSession();
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            // TODO: see about Raven Include syntax
            return Session.Query<T>();
        }

        protected override T GetQuery(TKey key)
        {
            if (typeof(TKey) == typeof(string))
                return Session.Load<T>(key as string);

            return base.GetQuery(key);
        }

        protected override void AddItem(T entity)
        {
            TKey id;
            
            if (GetPrimaryKey(entity, out id) && Equals(id, default(TKey)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }

            Session.Store(entity);
        }

        protected override void DeleteItem(T entity)
        {
            Session.Delete(entity);
        }

        protected override void UpdateItem(T entity)
        {
            // save changes will take care of it
        }

        protected override void SaveChanges()
        {
            Session.SaveChanges();
        }

        public override void Dispose()
        {
            if (Session != null)
                Session.Dispose();

            if (DocumentStore != null)
                DocumentStore.Dispose();
        }

        private TKey GeneratePrimaryKey()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
            }

            if (typeof(TKey) == typeof(Int32))
            {
                return (TKey)Convert.ChangeType(0, typeof(TKey));
            }

            if (typeof(TKey) == typeof(string))
            {
                // set to the plural of the typename with an ending slash
                //  that means that RavenDB will assign the next ID after the / for us
                //  http://ravendb.net/docs/client-api/basic-operations/saving-new-document
                return (TKey) Convert.ChangeType(TypeName + "s/", typeof (string));
            }

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
        }
    }
}