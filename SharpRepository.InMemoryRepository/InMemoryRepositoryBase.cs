using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using System.Reflection;

namespace SharpRepository.InMemoryRepository
{
    public abstract class InMemoryRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private readonly ConcurrentDictionary<TKey, T> _items = new ConcurrentDictionary<TKey, T>();

        internal InMemoryRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy)
        {
            
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return CloneDictionary(_items).AsQueryable();
        }
        
        protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
        {
            _items.TryGetValue(key, out T result);

            return result;
        }

        private static IEnumerable<T> CloneDictionary(ConcurrentDictionary<TKey, T> list)
        {
            // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
            //  since we can't really put those constraints on T I'm going to do it via reflection

            var type = typeof (T);
            var properties = type.GetProperties();
            var clonedList = new List<T>(list.Count);

            foreach (var keyValuePair in list)
            {
                var newItem = new T();
                foreach (var propInfo in properties)
                {
                    // Don't try and set a value to a property w/o a setter
                    if(propInfo.CanWrite)
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

            _items[id] = entity;
        }

        protected override void DeleteItem(T entity)
        {
            GetPrimaryKey(entity, out TKey pkValue);

            _items.TryRemove(pkValue, out T tmp);
        }

        protected override void UpdateItem(T entity)
        {
            GetPrimaryKey(entity, out TKey pkValue);

            _items[pkValue] = entity;     
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
                var pkValue = _items.Keys.LastOrDefault();

                var nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            }

            if (typeof(TKey) == typeof(Int32))
            {
                var pkValue = _items.Keys.LastOrDefault();

                var nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            }

            if (typeof(TKey) == typeof(Int64))
            {
                var pkValue = _items.Keys.LastOrDefault();

                var nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            }

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
        }

        public override string ToString()
        {
            return "SharpRepository.InMemoryRepository";
        }
    }
}
