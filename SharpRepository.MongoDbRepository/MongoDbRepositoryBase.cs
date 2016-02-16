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
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private readonly string _databaseName;
        protected MongoDatabase Database;
        protected MongoServer Server;

        private readonly Dictionary<Type, BsonType> _keyTypeToBsonType = new Dictionary<Type, BsonType>
                                                                      {
                                                                          {typeof(string), BsonType.String},
                                                                          {typeof(Guid), BsonType.ObjectId},
                                                                          {typeof(ObjectId), BsonType.ObjectId},
                                                                          {typeof(byte[]), BsonType.ObjectId}
                                                                      };

        private readonly Dictionary<Type, IIdGenerator> _keyTypeToBsonGenerator = new Dictionary<Type, IIdGenerator>
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
                                                                 cm.IdMemberMap.SetRepresentation(_keyTypeToBsonType[typeof(TKey)]);
                                                                 cm.IdMemberMap.SetIdGenerator(_keyTypeToBsonGenerator[typeof(TKey)]);
                                                             }    
                                                         }

                                                         cm.Freeze();
                                                     });
            }
        }

        private MongoCollection<T> BaseCollection()
        {
            return Database.GetCollection<T>(TypeName);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return BaseCollection().AsQueryable();
        }

        protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
        {
            var keyBsonType = ((RepresentationSerializationOptions)BsonClassMap.LookupClassMap(typeof(T)).IdMemberMap.SerializationOptions).Representation;

            return IsValidKey(key)
                           ? BaseCollection().FindOneById(BsonTypeMapper.MapToBsonValue(key, keyBsonType))
                           : default(T);
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