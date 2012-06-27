using System;
using System.Linq;
using Norm;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private IMongo _provider;
        private IMongoDatabase _database;
        
        internal MongoDbRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy) 
        {
            Initialize();
        }

        internal MongoDbRepositoryBase(string connectionString, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize(Mongo.Create(connectionString));
        }

        internal MongoDbRepositoryBase(IMongo mongoProvider, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy) 
        {
            Initialize(mongoProvider);
        }

        private void Initialize(IMongo mongoProvider = null)
        {
            _provider = mongoProvider ?? Mongo.Create("mongodb://127.0.0.1/Test?strict=false");
            _database = _provider.Database;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return _database.GetCollection<T>().AsQueryable();
        }

        protected override T GetQuery(TKey key)
        {
            return _database.GetCollection<T>().AsQueryable().ToList()
                .FirstOrDefault(x => MatchOnPrimaryKey(x, key));
        }

        private bool MatchOnPrimaryKey(T item, TKey keyValue)
        {
            TKey value;
            return GetPrimaryKey(item, out value) && keyValue.Equals(value);
        }
        
        protected override void AddItem(T entity)
        {
            TKey id;

            if (GetPrimaryKey(entity, out id) && Equals(id, default(TKey)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }
            
            _database.GetCollection<T>().Save(entity);
        }

        protected override void DeleteItem(T entity)
        {
            _database.GetCollection<T>().Delete(entity);
        }

        protected override void UpdateItem(T entity)
        {
            _database.GetCollection<T>().Save(entity);
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

            if (typeof(TKey) == typeof(Int32))
            {
                var nextInt = Convert.ToInt32(_database.GetCollection<T>().GenerateId());
                return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            }

            if (typeof(TKey) == typeof(Int64))
            {
                var nextLong = _database.GetCollection<T>().GenerateId();
                return (TKey)Convert.ChangeType(nextLong, typeof(TKey));
            }
            
            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and Int64.");
        }
    }
}