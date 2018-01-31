using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using System.Reflection;

namespace SharpRepository.CacheRepository
{
    public abstract class CacheRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private readonly string _prefix;
        private readonly ICachingProvider _cachingProvider;

        internal CacheRepositoryBase(string prefix, ICachingProvider cachingProvider, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            _prefix = prefix;
            _cachingProvider = cachingProvider;
        }

        private ConcurrentDictionary<TKey, T>  Items
        {
            get
            {
                ConcurrentDictionary<TKey, T> items = null;

                if (!_cachingProvider.Exists(_prefix + ".CacheRepository.Items"))
                {
                    items = new ConcurrentDictionary<TKey, T>();
                    _cachingProvider.Set(_prefix + ".CacheRepository.Items", items);
                }
                else
                {
                    if (!_cachingProvider.Get(_prefix + ".CacheRepository.Items", out items))
                    {
                        items = new ConcurrentDictionary<TKey, T>();
                    }
                }

                return items;
            }
            set
            {
                _cachingProvider.Set(_prefix + ".CacheRepository.Items", value);
            }
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return CloneDictionary(Items).AsQueryable();
        }

        protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
        {
            Items.TryGetValue(key, out T result);

            return result;
        }

        private static IEnumerable<T> CloneDictionary(ConcurrentDictionary<TKey, T> list)
        {
            // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
            //  since we can't really put those constraints on T I'm going to do it via reflection

            var type = typeof(T);
            var properties = type.GetTypeInfo().DeclaredProperties;

            var clonedList = new List<T>(list.Count);

            foreach (var keyValuePair in list)
            {
                var newItem = new T();
                foreach (var propInfo in properties)
                {
                    propInfo.SetValue(newItem, propInfo.GetValue(keyValuePair.Value, null), null);
                }

                clonedList.Add(newItem);
            }

            return clonedList;
        }

        protected override void AddItem(T entity)
        {
            if (GetPrimaryKey(entity, out TKey id) && GenerateKeyOnAdd && Equals(id, default(TKey)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }

            Items[id] = entity;
        }

        protected override void DeleteItem(T entity)
        {
            GetPrimaryKey(entity, out TKey pkValue);

            Items.TryRemove(pkValue, out T tmp);
        }

        protected override void UpdateItem(T entity)
        {
            GetPrimaryKey(entity, out TKey pkValue);

            Items[pkValue] = entity;     
        }

        protected override void SaveChanges()
        {
        }

        public override void Dispose()
        {
        }

        protected virtual TKey GeneratePrimaryKey()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
            }

            if (typeof(TKey) == typeof(string))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid().ToString("N"), typeof(TKey));
            }

            if (typeof(TKey) == typeof(Int32))
            {
                var pkValue = Items.Keys.LastOrDefault();

                var nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            }

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
        }

        public override string ToString()
        {
            return "SharpRepository.CacheRepository";
        }
    }
}