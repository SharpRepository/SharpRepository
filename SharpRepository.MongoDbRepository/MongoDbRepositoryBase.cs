using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private readonly string _databaseName;
        private MongoDatabase _database;
        private MongoServer _server;

        internal MongoDbRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize();
        }

        internal MongoDbRepositoryBase(string connectionString, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            Initialize(MongoServer.Create(connectionString));
        }

        internal MongoDbRepositoryBase(MongoServer mongoServer, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize(mongoServer);
        }

        private string DatabaseName
        {
            get { return string.IsNullOrEmpty(_databaseName) ? TypeName : _databaseName; }
        }

        private void Initialize(MongoServer mongoServer = null)
        {
            _server = mongoServer ?? MongoServer.Create("mongodb://localhost");
            _database = _server.GetDatabase(DatabaseName);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return _database.GetCollection<T>(TypeName).AsQueryable();
        }

        protected override T GetQuery(TKey key)
        {
            return IsValidKey(key)
                       ? _database.GetCollection<T>(TypeName).FindOne(Query.EQ("_id", new ObjectId(key.ToString())))
                       : default(T);
        }

        protected override void AddItem(T entity)
        {
            _database.GetCollection<T>(TypeName).Insert(entity);
        }

        protected override void DeleteItem(T entity)
        {
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);

            if (IsValidKey(pkValue))
            {
                _database.GetCollection<T>(TypeName).Remove(Query.EQ("_id", new ObjectId(pkValue.ToString())));
            }
        }

        protected override void UpdateItem(T entity)
        {
            _database.GetCollection<T>(TypeName).Save(entity);
        }

        protected override void SaveChanges()
        {
        }

        public override void Dispose()
        {
        }

        private static bool IsValidKey(TKey key)
        {
            return !string.IsNullOrEmpty(key.ToString());
        }
    }
}