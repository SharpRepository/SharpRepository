using System;
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
        private MongoServer _server;
        private MongoDatabase _database;
        private readonly string _databaseName;

        private string DatabaseName
        {
            get { return string.IsNullOrEmpty(_databaseName) ? TypeName : _databaseName; }
        }

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
            var s = key.ToString();
            if (string.IsNullOrEmpty(s)) return default(T);

            var objectId = new ObjectId(s);
            return _database.GetCollection<T>(TypeName).FindOne(Query.EQ("_id", objectId));
        }

        protected override void AddItem(T entity)
        {
            _database.GetCollection<T>(TypeName).Insert(entity);
        }
        
        protected override void DeleteItem(T entity)
        {
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);
            _database.GetCollection<T>(TypeName).Remove(Query.EQ("_id", new ObjectId(pkValue.ToString())));
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
    }
}