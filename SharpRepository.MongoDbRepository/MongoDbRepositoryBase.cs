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
            return BaseQuery().FirstOrDefault();
        }
        
        protected override void AddItem(T entity)
        {
            _database.GetCollection<T>(TypeName).Insert(entity);
        }
       
        protected override void DeleteItem(T entity)
        {
            // Yikes. 
            //IMongoQuery mq = new QueryDocument(entity.ToBsonDocument());  
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);
            _database.GetCollection<T>(TypeName).Remove(Query.EQ("_id", new ObjectId(pkValue.ToString())));
            //_database.GetCollection<T>(TypeName).Remove(mq);
        }

        protected override void UpdateItem(T entity)
        {
            _database.GetCollection<T>(TypeName).Save(entity);
        }

        protected override void SaveChanges()
        {
            //_session.Save();
        }

        public override void Dispose()
        {
            
        }

        private TKey GeneratePrimaryKey()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
            }

            //if (typeof(TKey) == typeof(Int32))
            //{
            //    var nextInt = Convert.ToInt32(_database.GetCollection<T>(TypeName).GenerateId());
            //    return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            //}

            //if (typeof(TKey) == typeof(Int64))
            //{
            //    var nextLong = _database.GetCollection<T>().GenerateId();
            //    return (TKey)Convert.ChangeType(nextLong, typeof(TKey));
            //}
            
            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and Int64.");
        }


        private bool MatchOnPrimaryKey(T item, TKey keyValue)
        {
            TKey value;
            return GetPrimaryKey(item, out value) && keyValue.Equals(value);
        }
    }
}