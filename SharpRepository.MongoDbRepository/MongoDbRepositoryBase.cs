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
        
        internal MongoDbRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy) 
        {
            Initialize();
        }

        internal MongoDbRepositoryBase(string connectionString, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
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
            _database = _server.GetDatabase(TypeName);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return _database.GetCollection<T>(TypeName).AsQueryable();
        }

        protected override T GetQuery(TKey key)
        {
            return _database.GetCollection<T>(TypeName).FindOne(Query.EQ("_id", new ObjectId(key.ToString())));
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