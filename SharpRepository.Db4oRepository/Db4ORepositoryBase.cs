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
        protected IObjectContainer Container;

        internal Db4oRepositoryBase(string storagePath, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Initialize(storagePath);
        }

        private void Initialize(string storagePath)
        {
            Container = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), storagePath);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return Container.AsQueryable<T>();
        }

        protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
        {
            return BaseQuery(fetchStrategy).FirstOrDefault(x => MatchOnPrimaryKey(x, key));
        }

        private bool MatchOnPrimaryKey(T item, TKey keyValue)
        {
            return GetPrimaryKey(item, out TKey value) && keyValue.Equals(value);
        }

        protected override void AddItem(T entity)
        {
            if (GenerateKeyOnAdd && GetPrimaryKey(entity, out TKey id) && Equals(id, default(TKey)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }

            Container.Store(entity);
        }

        protected override void DeleteItem(T entity)
        {
            Container.Delete(entity);
        }

        protected override void UpdateItem(T entity)
        {
            Container.Store(entity);
        }

        protected override void SaveChanges()
        {
            Container.Commit();
        }

        public override void Dispose()
        {
            Container.Close();
            if (Container != null)
                Container.Dispose();
        }

        protected virtual TKey GeneratePrimaryKey()
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
                T last = GetAll().LastOrDefault() ?? new T();
                GetPrimaryKey(last, out TKey pkValue);

                int nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey) Convert.ChangeType(nextInt, typeof (TKey));
            }

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
        }
    }
}