using System.Linq;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SharpRepository.MongoDbRepository.Aggregates;
using SharpRepository.Repository;
using SharpRepository.Repository.Aggregates;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Helpers;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private readonly string _databaseName;
        protected MongoDatabase Database;
        protected MongoServer Server;

        internal MongoDbRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize();
        }

        internal MongoDbRepositoryBase(string connectionString, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            Initialize(new MongoClient(connectionString).GetServer());
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
            Server = mongoServer ?? new MongoClient("mongodb://localhost").GetServer();
            Database = Server.GetDatabase(DatabaseName);
        }

        protected override IAggregateQueries<T> GetAggregateQueries(IRepository<T, TKey> repository, Repository.Queries.QueryManager<T, TKey> queryManager)
        {
            return new MongoDbAggregateQueries<T, TKey>(this, QueryManager);
        }

        private MongoCollection<T> BaseCollection()
        {
            return Database.GetCollection<T>(TypeName);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return BaseCollection().AsQueryable();
        }

        protected override T GetQuery(TKey key)
        {
            var keyBsonType = ((RepresentationSerializationOptions)BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.SerializationOptions).Representation;

            return IsValidKey(key)
                           ? BaseCollection().FindOneById(BsonTypeMapper.MapToBsonValue(key, keyBsonType))
                           : default(T);
        }

        protected override void AddItem(T entity)
        {
            BaseCollection().Insert(entity);
        }

        protected override void DeleteItem(T entity)
        {
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);

            if (IsValidKey(pkValue))
            {
                var keyMemberMap = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap;
                var keyBsonType = ((RepresentationSerializationOptions)keyMemberMap.SerializationOptions).Representation;
                BaseCollection().Remove(Query.EQ(keyMemberMap.ElementName, BsonTypeMapper.MapToBsonValue(pkValue, keyBsonType)));
            }
        }

        protected override void UpdateItem(T entity)
        {
            BaseCollection().Save(entity);
        }

        protected override void SaveChanges()
        {
        }

        protected override PropertyInfo GetPrimaryKeyPropertyInfo()
        {
            // checks for the MongoDb BsonIdAttribute and if not there no the normal checks
            var type = typeof(T);
            var keyType = typeof(TKey);

            return type.GetProperties().FirstOrDefault(x => x.HasAttribute<BsonIdAttribute>() && x.PropertyType == keyType)
                ?? base.GetPrimaryKeyPropertyInfo();
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