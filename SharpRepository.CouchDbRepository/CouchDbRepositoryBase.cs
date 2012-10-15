using System;
using System.Linq;
using RedBranch.Hammock; // reference: http://code.google.com/p/relax-net/
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.CouchDbRepository
{
    public class CouchDbRepositoryBase<T> : LinqRepositoryBase<T, string> where T : class, new()
    {
        protected Connection Connection;
        protected Session Session;

        internal CouchDbRepositoryBase()
            : this("http://127.0.0.1:5984/")
         {
         }

        internal CouchDbRepositoryBase(string url)
        {
            Initialize(url, typeof(T).Name);
        }

        internal CouchDbRepositoryBase(string url, string database)
        {
            Initialize(url, database);
        }

        private void Initialize(string url, string database)
        {
            database = database.ToLower(); // CouchDb requires lowercase  database names

            Connection = new Connection(new Uri(url));
            Session = Connection.CreateSession(database);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            // TODO: this is terrible and ridiculously non-performant, change to be able to convert and use the Hammock fluent syntax or convert to JS map/reduce that CouchDb uses

            var hammockRepository = new Repository<T>(Session);
            var all = hammockRepository.All();

            //var query = new Query<T>(all.Query.Session, all.Query.Design, all.Query.View)

           
             return all .ToList().AsQueryable();

            //var all = Session.ListDocuments();
           // return all.Select(x => Session.Load<T>(x.Id)).AsQueryable();
        }

        // we override the implementation fro LinqBaseRepository becausee this is built in 
        protected override T GetQuery(string key)
        {
            var item =  Session.Load<T>(key);

            // this always returns an object, so check to see if the PK is null, if so then return null
            string id;
            GetPrimaryKey(item, out id);

            return id == null ? null : item;
        }

        protected override void AddItem(T entity)
        {
            string id;
            if (GetPrimaryKey(entity, out id) && Equals(id, default(string)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }

            Session.Save(entity, id); // save the generated PK as the internal id (_id) and as the entity PK
        }

        protected override void DeleteItem(T entity)
        {
            Session.Delete(entity);
        }

        protected override void UpdateItem(T entity)
        {
            Session.Save(entity);
        }

        protected override void SaveChanges()
        {

        }

        public override void Dispose()
        {
            
        }

        private static string GeneratePrimaryKey()
        {
            return (string) Convert.ChangeType(Guid.NewGuid().ToString("N"), typeof (string));
        }
    }
}
