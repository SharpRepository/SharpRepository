using System;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Db4oRepository
{
    public class Db4oRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private IObjectContainer _container;

        internal Db4oRepositoryBase(string storagePath, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize(storagePath);
        }

        private void Initialize(string storagePath)
        {
            _container = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), storagePath);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return _container.AsQueryable<T>();
        }

        protected override T GetQuery(TKey key)
        {
            return _container.AsQueryable<T>().FirstOrDefault(x => MatchOnPrimaryKey(x, key));
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

            _container.Store(entity);
        }

        protected override void DeleteItem(T entity)
        {
            _container.Delete(entity);
        }

        protected override void UpdateItem(T entity)
        {
            _container.Store(entity);
        }

        protected override void SaveChanges()
        {
            _container.Commit();
        }

        public override void Dispose()
        {
            _container.Close();
            if (_container != null)
                _container.Dispose();
        }

        private TKey GeneratePrimaryKey()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
            }

            if (typeof(TKey) == typeof(string))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid().ToString(), typeof(TKey));
            }

            if (typeof (TKey) == typeof (Int32))
            {
                TKey pkValue;

                T last = GetAll().LastOrDefault() ?? new T();
                GetPrimaryKey(last, out pkValue);

                int nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey) Convert.ChangeType(nextInt, typeof (TKey));
            }

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
        }
    }
}