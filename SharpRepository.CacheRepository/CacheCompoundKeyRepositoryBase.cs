using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.CacheRepository
{
    public abstract class CacheCompoundKeyRepositoryBase<T> : LinqCompoundKeyRepositoryBase<T> where T : class, new()
    {
        private readonly string _prefix;
        private readonly ICachingProvider _cachingProvider;

        internal CacheCompoundKeyRepositoryBase(string prefix, ICachingProvider cachingProvider, ICompoundKeyCachingStrategy<T> cachingStrategy = null)
            : base(cachingStrategy)
        {
            _prefix = prefix;
            _cachingProvider = cachingProvider;
        }

        private ConcurrentDictionary<string, T>  Items
        {
            get
            {
                ConcurrentDictionary<string, T> items = null;

                if (!_cachingProvider.Exists(_prefix + ".CacheRepository.Items"))
                {
                    items = new ConcurrentDictionary<string, T>();
                    _cachingProvider.Set(_prefix + ".CacheRepository.Items", items);
                }
                else
                {
                    if (!_cachingProvider.Get(_prefix + ".CacheRepository.Items", out items))
                    {
                        items = new ConcurrentDictionary<string, T>();
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

        protected override T GetQuery(params object[] keys)
        {
            Items.TryGetValue(String.Join("/", keys), out T result);

            return result;
        }

        private static IEnumerable<T> CloneDictionary(ConcurrentDictionary<string, T> list)
        {
            // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
            //  since we can't really put those constraints on T I'm going to do it via reflection

            Type type = typeof(T);
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
            if (!GetPrimaryKeys(entity, out object[] keys))
            {
                throw new ArgumentException("Primary keys not set");
            }

            Items[String.Join("/", keys)] = entity;
        }

        protected override void DeleteItem(T entity)
        {
            if (!GetPrimaryKeys(entity, out object[] keys))
            {
                throw new ArgumentException("Primary keys not set");
            }

            Items.TryRemove(String.Join("/", keys), out T tmp);
        }

        protected override void UpdateItem(T entity)
        {
            if (!GetPrimaryKeys(entity, out object[] keys))
            {
                throw new ArgumentException("Primary keys not set");
            }

            Items[String.Join("/", keys)] = entity;
        }

        protected override void SaveChanges()
        {
        }

        public override void Dispose()
        {
        }

        public override string ToString()
        {
            return "SharpRepository.InMemoryRepository";
        }
    }

    public abstract class CacheCompoundKeyRepositoryBase<T, TKey, TKey2> : LinqCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        private struct CompoundKey
        {
            public TKey Key1 { private get; set; }
            public TKey2 Key2 { private get; set; }

            public override int GetHashCode()
            {
                return Key1.GetHashCode() ^ Key2.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is CompoundKey compositeKey)
                {
                    return Key1.Equals(compositeKey.Key1) && Key2.Equals(compositeKey.Key2);
                }

                return false;
            }
        }

        private ConcurrentDictionary<CompoundKey, T> Items
        {
            get
            {
                ConcurrentDictionary<CompoundKey, T> items = null;

                if (!_cachingProvider.Exists(_prefix + ".CacheRepository.Items"))
                {
                    items = new ConcurrentDictionary<CompoundKey, T>();
                    _cachingProvider.Set(_prefix + ".CacheRepository.Items", items);
                }
                else
                {
                    if (!_cachingProvider.Get(_prefix + ".CacheRepository.Items", out items))
                    {
                        items = new ConcurrentDictionary<CompoundKey, T>();
                    }
                }

                return items;
            }
            set
            {
                _cachingProvider.Set(_prefix + ".CacheRepository.Items", value);
            }
        }

        private readonly string _prefix;
        private readonly ICachingProvider _cachingProvider;
        
        internal CacheCompoundKeyRepositoryBase(string prefix, ICachingProvider cachingProvider, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(cachingStrategy)
        {
            _prefix = prefix;
            _cachingProvider = cachingProvider;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return CloneDictionary(Items).AsQueryable();
        }

        protected override T GetQuery(TKey key, TKey2 key2)
        {
            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2 };
            Items.TryGetValue(compoundKey, out T result);

            return result;
        }

        private static IEnumerable<T> CloneDictionary(ConcurrentDictionary<CompoundKey, T> list)
        {
            // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
            //  since we can't really put those constraints on T I'm going to do it via reflection

            var type = typeof(T);
            var properties = type.GetProperties();

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
            GetPrimaryKey(entity, out TKey key, out TKey2 key2);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2 };
            Items[compoundKey] = entity;
        }

        protected override void DeleteItem(T entity)
        {
            GetPrimaryKey(entity, out TKey key, out TKey2 key2);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2 };
            Items.TryRemove(compoundKey, out T tmp);
        }

        protected override void UpdateItem(T entity)
        {
            GetPrimaryKey(entity, out TKey key, out TKey2 key2);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2 };
            Items[compoundKey] = entity;
        }

        protected override void SaveChanges()
        {
        }

        public override void Dispose()
        {
        }

        public override string ToString()
        {
            return "SharpRepository.InMemoryRepository";
        }
    }


    public abstract class CacheCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> : LinqCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> where T : class, new()
    {
        private struct CompoundKey
        {
            public TKey Key1 { private get; set; }
            public TKey2 Key2 { private get; set; }
            public TKey3 Key3 { private get; set; }

            public override int GetHashCode()
            {
                return Key1.GetHashCode() ^ Key2.GetHashCode() ^ Key3.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is CompoundKey compositeKey)
                {
                    return Key1.Equals(compositeKey.Key1) && Key2.Equals(compositeKey.Key2) && Key3.Equals(compositeKey.Key2);
                }

                return false;
            }
        }

        private ConcurrentDictionary<CompoundKey, T> Items
        {
            get
            {
                ConcurrentDictionary<CompoundKey, T> items = null;

                if (!_cachingProvider.Exists(_prefix + ".CacheRepository.Items"))
                {
                    items = new ConcurrentDictionary<CompoundKey, T>();
                    _cachingProvider.Set(_prefix + ".CacheRepository.Items", items);
                }
                else
                {
                    if (!_cachingProvider.Get(_prefix + ".CacheRepository.Items", out items))
                    {
                        items = new ConcurrentDictionary<CompoundKey, T>();
                    }
                }

                return items;
            }
            set
            {
                _cachingProvider.Set(_prefix + ".CacheRepository.Items", value);
            }
        }

        private readonly string _prefix;
        private readonly ICachingProvider _cachingProvider;
        
        internal CacheCompoundKeyRepositoryBase(string prefix, ICachingProvider cachingProvider, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
            : base(cachingStrategy)
        {
            _prefix = prefix;
            _cachingProvider = cachingProvider;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return CloneDictionary(Items).AsQueryable();
        }

        protected override T GetQuery(TKey key, TKey2 key2, TKey3 key3)
        {
            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            Items.TryGetValue(compoundKey, out T result);

            return result;
        }

        private static IEnumerable<T> CloneDictionary(ConcurrentDictionary<CompoundKey, T> list)
        {
            // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
            //  since we can't really put those constraints on T I'm going to do it via reflection

            var type = typeof(T);
            var properties = type.GetProperties();

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
            GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            Items[compoundKey] = entity;
        }

        protected override void DeleteItem(T entity)
        {
            GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            Items.TryRemove(compoundKey, out T tmp);
        }

        protected override void UpdateItem(T entity)
        {
            GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            Items[compoundKey] = entity;
        }

        protected override void SaveChanges()
        {
        }

        public override void Dispose()
        {
        }

        public override string ToString()
        {
            return "SharpRepository.InMemoryRepository";
        }
    }


    public abstract class InMemoryCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> : LinqCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> where T : class, new()
    {
        private struct CompoundKey
        {
            public TKey Key1 { get; set; }
            public TKey2 Key2 { get; set; }
            public TKey3 Key3 { get; set; }

            public override int GetHashCode()
            {
                return Key1.GetHashCode() ^ Key2.GetHashCode() ^ Key3.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is CompoundKey compositeKey)
                {
                    return Key1.Equals(compositeKey.Key1) && Key2.Equals(compositeKey.Key2) && Key3.Equals(compositeKey.Key3);
                }

                return false;
            }
        }

        private readonly ConcurrentDictionary<CompoundKey, T> _items = new ConcurrentDictionary<CompoundKey, T>();

        internal InMemoryCompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return CloneDictionary(_items).AsQueryable();
        }

        protected override T GetQuery(TKey key, TKey2 key2, TKey3 key3)
        {
            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            _items.TryGetValue(compoundKey, out T result);

            return result;
        }

        private static IEnumerable<T> CloneDictionary(ConcurrentDictionary<CompoundKey, T> list)
        {
            // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
            //  since we can't really put those constraints on T I'm going to do it via reflection

            var type = typeof(T);
            var properties = type.GetProperties();
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
            GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            _items[compoundKey] = entity;
        }

        protected override void DeleteItem(T entity)
        {
            GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            _items.TryRemove(compoundKey, out T tmp);
        }

        protected override void UpdateItem(T entity)
        {
            GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3);

            var compoundKey = new CompoundKey { Key1 = key, Key2 = key2, Key3 = key3 };
            _items[compoundKey] = entity;
        }

        protected override void SaveChanges()
        {
        }

        public override void Dispose()
        {
        }

        public override string ToString()
        {
            return "SharpRepository.InMemoryRepository";
        }
    }
}
