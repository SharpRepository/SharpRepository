using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Specifications;
using MongoDB.Bson.Serialization.Serializers;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private readonly string _databaseName;
        protected IMongoDatabase Database;

        private readonly Dictionary<Type, BsonType> _keyTypeToBsonType = 
            new Dictionary<Type, BsonType>
            {
                {typeof(string), BsonType.String},
                {typeof(Guid), BsonType.ObjectId},
                {typeof(ObjectId), BsonType.ObjectId},
                {typeof(byte[]), BsonType.ObjectId}
            };

        private readonly Dictionary<Type, IIdGenerator> _keyTypeToBsonGenerator = 
            new Dictionary<Type, IIdGenerator>
            {
                {typeof (string), new StringObjectIdGenerator() },
                {typeof (Guid), new GuidGenerator()},
                {typeof (ObjectId), new ObjectIdGenerator()},
                {typeof(byte[]), new BsonBinaryDataGuidGenerator(GuidRepresentation.Standard)}
            };

        internal MongoDbRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize();
        }

        internal MongoDbRepositoryBase(string connectionString, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var cli = new MongoClient(connectionString);
            Initialize(cli.GetDatabase(_databaseName));
        }

        internal MongoDbRepositoryBase(IMongoDatabase mongoDatabase, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize(mongoDatabase);
        }

        private string DatabaseName
        {
            get { return string.IsNullOrEmpty(_databaseName) ? TypeName : _databaseName; }
        }

        private void Initialize(IMongoDatabase mongoDatabase = null)
        {
            Database = mongoDatabase ?? new MongoClient("mongodb://localhost").GetDatabase(MongoUrl.Create("mongodb://localhost").DatabaseName);

            if (!BsonClassMap.IsClassMapRegistered(typeof (T)))
            {
                var primaryKeyPropInfo = GetPrimaryKeyPropertyInfo();
                var primaryKeyName = primaryKeyPropInfo.Name;

                BsonClassMap.RegisterClassMap<T>(cm =>
                    {
                        cm.AutoMap();
                        if (cm.IdMemberMap == null)
                        {
                            cm.SetIdMember(cm.GetMemberMap(primaryKeyName));

                            if (_keyTypeToBsonType.ContainsKey(typeof(TKey)) && (_keyTypeToBsonGenerator.ContainsKey(typeof(TKey))))
                            {
                                cm.IdMemberMap.SetSerializer(new StringSerializer(_keyTypeToBsonType[typeof(TKey)]));
                                cm.IdMemberMap.SetIdGenerator(_keyTypeToBsonGenerator[typeof(TKey)]);
                            }    
                        }

                        cm.Freeze();
                    }
                );
            }
        }

        private IMongoCollection<T> BaseCollection()
        {
            return Database.GetCollection<T>(TypeName);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return BaseCollection().AsQueryable();
        }

        protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
        {
            var keyBsonType = ((StringSerializer)BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.GetSerializer()).Representation;
            var keyMemberName = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.MemberName;
            if (IsValidKey(key)) {
                var keyBsonValue = BsonTypeMapper.MapToBsonValue(key, keyBsonType);
                var filter = Builders<T>.Filter.Eq(keyMemberName, keyBsonValue);
                return BaseCollection().Find(filter).FirstOrDefault();
            } else return default(T);
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
            BaseCollection().InsertOne(entity);
        }

        protected override void DeleteItem(T entity)
        {
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);

            if (IsValidKey(pkValue))
            {
                var keyPropertyName = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.ElementName;
                var keyPair = GetMongoProperty(entity, keyPropertyName);
                var filter = Builders<T>.Filter.Eq(keyPair.Key, keyPair.Value);

                BaseCollection().DeleteOne(filter);
            }
        }

        protected override void UpdateItem(T entity)
        {
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);
            if (IsValidKey(pkValue))
            {
                var keyPropertyName = BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.ElementName;
                var keyPair = GetMongoProperty(entity, keyPropertyName);
                var filter = Builders<T>.Filter.Eq(keyPair.Key, keyPair.Value);


                var bsonMembers = BsonClassMap.LookupClassMap(typeof(T)).AllMemberMaps.Where(m => m.MemberName != keyPropertyName);
                var updates = new List<UpdateDefinition<T>>();
                foreach (var members in bsonMembers)
                {
                    var propPair = GetMongoProperty(entity, members.MemberName);
                    updates.Add(Builders<T>.Update.Set(propPair.Key, propPair.Value));
                }

                BaseCollection().UpdateOne(filter, Builders<T>.Update.Combine(updates));
            }

        }

        public static KeyValuePair<string, BsonValue> GetMongoProperty(T entity, string propertyName)
        {
            var value = typeof(T).GetProperty(propertyName).GetValue(entity);
            var memberMap = BsonClassMap.LookupClassMap(typeof(TKey)).GetMemberMap(propertyName);
            var keyBsonType = ((StringSerializer)memberMap.GetSerializer()).Representation;
            var bsonPropertyValue = BsonTypeMapper.MapToBsonValue(value, keyBsonType);
            
            return new KeyValuePair<string, BsonValue>(propertyName, bsonPropertyValue);
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