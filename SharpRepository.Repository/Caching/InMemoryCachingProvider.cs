using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Uses the .NET built-in MemoryCache as the caching provider.
    /// </summary>
    public class InMemoryCachingProvider : ICachingProvider
    {
        private static IMemoryCache Cache
        {
            get { return new MemoryCache(new MemoryCacheOptions()); }
        }

        private static readonly object LockObject = new object();

        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Normal, int? cacheTime = null)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            var policy = new MemoryCacheEntryOptions()
                             {
                                 Priority = priority
                             };
            if (cacheTime.HasValue)
            {
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime.Value);
            }

            Cache.Set(key, value, policy);
        }

        public void Clear(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            object value;
            return Cache.TryGetValue(key, out value);
        }

        public bool Get<T>(string key, out T value)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            value = default(T);

            try
            {
                if (!Exists(key))
                    return false;

                value = Cache.Get<T>(key);
            }
            catch (Exception)
            {
                // ignore and use default
                return false;
            }

            return true;
        }

        public int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            lock (LockObject)
            {
                int current;
                if (!Get(key, out current))
                {
                    current = defaultValue;
                }

                var newValue = current + incrementValue;
                Set(key, newValue, priority);
                return newValue;
            }
        }

        public void Dispose()
        {
            // no need to do anything
        }
    }
}
