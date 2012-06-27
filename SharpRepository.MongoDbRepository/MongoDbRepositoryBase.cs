using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.MongoDbRepository
{
    public class EndsWithIdConvention : IIdMemberConvention
    {
        public string FindIdMember(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.Name.EndsWith("Id"))
                {
                    return property.Name;
                }
            }
            return null;
        }
    }
    public class EmployeeIdGenerator : IIdGenerator
    {
        // implement GenerateId
        // implement IsEmpty
        public object GenerateId(object container, object document)
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty(object id)
        {
            throw new NotImplementedException();
        }
    }
    public class MongoDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private MongoServer _mongoServer;
        private MongoDatabase _database;
        private MongoCollection<T> _session;

        internal MongoDbRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {
            Initialize();
        }

        internal MongoDbRepositoryBase(string connectionString, ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
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
            _mongoServer = mongoServer ?? MongoServer.Create("mongodb://localhost/?safe=true");
            //BsonClassMap.RegisterClassMap<Employee>(cm =>
            //{
            //    cm.AutoMap();
                
            //});
            //if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            //{
            //    BsonClassMap.RegisterClassMap<T>(cm =>
            //        {
            //            cm.AutoMap();
            //            cm.IdMember.SetIdGenerator(new EmployeeIdGenerator());
            //            cm.SetIdMember(cm.GetMemberMap(GetPrimaryKeyPropertyInfo().Name));
            //        }
            //        //  cm.MapIdProperty(GetPrimaryKeyPropertyInfo().Name)

            //     );
            //}

            //var myConventions = new ConventionProfile();
            //myConventions.SetIdMemberConvention(new EndsWithIdConvention());
            //BsonClassMap.RegisterConventions(
            //    myConventions,
            //    t => t.FullName.StartsWith("MyNamespace.")
            //);

            _database = _mongoServer.GetDatabase(TypeName);
            _session = _database.GetCollection<T>(TypeName);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return _session.AsQueryable();
        }

        protected override T GetQuery(TKey key)
        {
            return _session.AsQueryable().FirstOrDefault(x => MatchOnPrimaryKey(x, key));
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

            _session.Insert(entity);
        }

        protected override void DeleteItem(T entity)
        {
            IMongoQuery mq = new QueryDocument(entity.ToBsonDocument());
            _session.Remove(mq);
        }

        protected override void UpdateItem(T entity)
        {
            _session.Save(entity);
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
                TKey pkValue;
                
                SortByBuilder sortBy = SortBy.Descending(GetPrimaryKeyPropertyInfo().Name);
                T last = _session.FindAllAs<T>().SetSortOrder(sortBy).FirstOrDefault();
                GetPrimaryKey(last, out pkValue);

                int nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            }

            throw new InvalidOperationException(
                "Primary key could not be generated.  This only works for GUID and Int32.");
        }
    }
}