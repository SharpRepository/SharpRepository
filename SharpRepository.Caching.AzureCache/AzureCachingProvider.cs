using System;
using System.Runtime.Caching;
using Microsoft.ApplicationServer.Caching;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Caching.AzureCache
{
    public class AzureCachingProvider : ICachingProvider
    {
        protected DataCacheFactory CacheFactory { get; set; }
        protected DataCache Cache { get; set; }

        private static readonly object LockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCachingProvider"/> class.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        public AzureCachingProvider(string cacheName = null)
            : this(new DataCacheFactory(), cacheName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCachingProvider"/> class.
        /// </summary>
        /// <param name="configuration">The AbbFabric Caching configuration.</param>
        /// <param name="cacheName">Name of the cache.</param>
        public AzureCachingProvider(DataCacheFactoryConfiguration configuration, string cacheName = null)
            : this(new DataCacheFactory(configuration), cacheName)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureCachingProvider"/> class.
        /// </summary>
        /// <param name="cacheFactory">The cache factory.</param>
        /// <param name="cacheName">Name of the cache.</param>
        public AzureCachingProvider(DataCacheFactory cacheFactory, string cacheName = null)
        {
            if (cacheFactory == null) throw new ArgumentNullException("cacheFactory");

            CacheFactory = cacheFactory;

            // TODO: don't know enough about Azure caching to know if we should use the GetDefaultCache() if no cache name provided, or if we should use our own name like SharpRepository
            Cache = String.IsNullOrEmpty(cacheName) ? cacheFactory.GetDefaultCache() : cacheFactory.GetCache(cacheName);
        }

        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Default, int? timeoutInSeconds = null)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            if (timeoutInSeconds.HasValue)
            {
                Cache.Put(key, value, new TimeSpan(0, 0, 0, timeoutInSeconds.Value));
            }
            else
            {
                Cache.Put(key, value);
            }
        }

        public void Clear(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            return (Cache.Get(key) != null);
        }

        public bool Get<T>(string key, out T value)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            try
            {
                value = (T)Cache.Get(key);

                if (Equals(value, default(T)))
                {
                    value = default(T);
                    return false;
                }
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }

        public int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Default)
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
            Cache = null;
            CacheFactory.Dispose();
        }
    }
}
