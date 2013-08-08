using System;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.CouchDbRepository.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.CouchDbRepository
{
    public class CouchDbRepositoryBase<T> : LinqRepositoryBase<T, string> where T : class, new()
    {
        protected CouchDbClient<T> Client;
        private readonly string _serverUrl;
        private readonly string _database;

        //private readonly CouchDbQueryProvider _provider;
        private readonly IQueryable<T> _baseQuery;

        internal CouchDbRepositoryBase()
            : this("127.0.0.1", 5984)
         {
         }

        internal CouchDbRepositoryBase(string host)
            : this(host, 5984)
        {
        }

        internal CouchDbRepositoryBase(string host, int port, string database = null, string username = null, string password = null)
        {
            if (String.IsNullOrEmpty(database))
            {
                database = typeof (T).Name;
            }
            _database = database.ToLower(); // CouchDb requires lowercase  database names

            _serverUrl = String.Format("http://{0}:{1}", host, port);

            Client = new CouchDbClient<T>(_serverUrl, _database);

            if (!CouchDbManager.HasDatabase(_serverUrl, _database))
            {
                CouchDbManager.CreateDatabase(_serverUrl, _database);
            }
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return CouchDbQueryFactory.Queryable<T>(_serverUrl, _database);
        }

        // we override the implementation of LinqBaseRepository because this is built in 
        protected override T GetQuery(string key)
        {
            var item = Client.GetDocument(key);

            if (item == null)
                return null;

            // this always returns an object, so check to see if the PK is null, if so then return null
            string id;
            GetPrimaryKey(item, out id);

            return id == null ? null : item;
        }

        public override TResult Min<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            var pagingOptions = new PagingOptions<T, TResult>(1, 1, selector);

            return QueryManager.ExecuteMin(
                () => FindAll(criteria, selector, pagingOptions).ToList().First(),
                selector,
                criteria
                );
        }

        public override TResult Max<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector)
        {
            var pagingOptions = new PagingOptions<T, TResult>(1, 1, selector, isDescending: true);

            return QueryManager.ExecuteMin(
                () => FindAll(criteria, selector, pagingOptions).ToList().First(),
                selector,
                criteria
                );
        }

        public override int Sum(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override decimal? Sum(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override decimal Sum(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override double? Sum(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override double Sum(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override float? Sum(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override float Sum(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override int? Sum(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override long? Sum(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override long Sum(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return QueryManager.ExecuteSum(
                 () => FindAll(criteria, selector).ToList().Sum(),
                 selector,
                 criteria
                 );
        }

        public override double Average(ISpecification<T> criteria, Expression<Func<T, int>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override decimal? Average(ISpecification<T> criteria, Expression<Func<T, decimal?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override decimal Average(ISpecification<T> criteria, Expression<Func<T, decimal>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double? Average(ISpecification<T> criteria, Expression<Func<T, double?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double Average(ISpecification<T> criteria, Expression<Func<T, double>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override float? Average(ISpecification<T> criteria, Expression<Func<T, float?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override float Average(ISpecification<T> criteria, Expression<Func<T, float>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double? Average(ISpecification<T> criteria, Expression<Func<T, int?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double? Average(ISpecification<T> criteria, Expression<Func<T, long?>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        public override double Average(ISpecification<T> criteria, Expression<Func<T, long>> selector)
        {
            return QueryManager.ExecuteAverage(
                 () => FindAll(criteria, selector).ToList().Average(),
                 selector,
                 criteria
                 );
        }

        protected override void AddItem(T entity)
        {
            string id;
            if (GetPrimaryKey(entity, out id) && Equals(id, default(string)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }

            Client.CreateDocument(entity, id);
        }

        protected override void DeleteItem(T entity)
        {
            string id;
            if (!GetPrimaryKey(entity, out id))
                return;

            Client.DeleteDocument(id);
        }

        protected override void UpdateItem(T entity)
        {
            string id;
            GetPrimaryKey(entity, out id);

            Client.UpdateDocument(entity, id);
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
