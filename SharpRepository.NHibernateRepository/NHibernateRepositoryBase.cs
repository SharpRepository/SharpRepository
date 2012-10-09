using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.NHibernateRepository
{
    public class NHibernateRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private ISession _session;

        //internal NHibernateRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        //{
        //    Initialize();
        //}

        internal NHibernateRepositoryBase(ISession session, ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {
            Initialize(session);
        }

        internal NHibernateRepositoryBase(ISessionFactory sessionFactory, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy) 
        {
            Initialize(sessionFactory.OpenSession());
        }

        private void Initialize(ISession session)
        {
            _session = session;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            // TODO: see about NHibernate Include syntax
            //  NHibernateUtil.Initialize() - http://nhforge.org/wikis/howtonh/lazy-loading-eager-loading.aspx
            return _session.Linq<T>();
        }

        protected override void AddItem(T entity)
        {
            TKey id;
            
            if (GetPrimaryKey(entity, out id) && Equals(id, default(TKey)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }

             _session.Save(entity);
        }

        protected override void DeleteItem(T entity)
        {
            _session.Delete(entity);
        }

        protected override void UpdateItem(T entity)
        {
             _session.Update(entity);
        }

        protected override void SaveChanges()
        {
            
        }

        public override void Dispose()
        {
            if (_session != null)
                _session.Dispose();
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