﻿using System;
using System.Runtime.Caching;
using Microsoft.ApplicationServer.Caching;

// Reference: http://stackoverflow.com/questions/4739548/appfabric-caching-examples-using-c-sharp

namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Uses AppFabric Caching as the caching provider.  <a href="http://msdn.microsoft.com/en-us/library/ff383731(v=azure.10).aspx">Get more info on AppFabric Caching</a>
    /// </summary>
    public class AppFabricCachingProvider : ICachingProvider
    {
        protected DataCacheFactory CacheFactory { get; set;}
        protected DataCache Cache { get; set; }

        private static object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFabricCachingProvider"/> class.
        /// </summary>
        /// <param name="cacheName">Name of the cache.</param>
        public AppFabricCachingProvider(string cacheName = null)
            : this(new DataCacheFactory(), cacheName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFabricCachingProvider"/> class.
        /// </summary>
        /// <param name="configuration">The AbbFabric Caching configuration.</param>
        /// <param name="cacheName">Name of the cache.</param>
        public AppFabricCachingProvider(DataCacheFactoryConfiguration configuration, string cacheName = null)
            : this(new DataCacheFactory(configuration), cacheName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFabricCachingProvider"/> class.
        /// </summary>
        /// <param name="cacheFactory">The cache factory.</param>
        /// <param name="cacheName">Name of the cache.</param>
        public AppFabricCachingProvider(DataCacheFactory cacheFactory, string cacheName = null)
        {
            CacheFactory = cacheFactory;

            // TODO: don't know enough about AppFabric to know if we should use the GetDefaultCache() if no cache name provided, or if we should use our own name like SharpRepository
            Cache = String.IsNullOrEmpty(cacheName) ? cacheFactory.GetDefaultCache() : cacheFactory.GetCache(cacheName);
        }

        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Default, int? cacheTime = null)
        {

            if (!cacheTime.HasValue)
            {
                Cache.Put(key, value);
            }
            else
            {
                Cache.Put(key, value, TimeSpan.FromSeconds(cacheTime.Value));
            }
        }

        public void Clear(string key)
        {
            Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return Cache.Get(key) != null;
        }

        public bool Get<T>(string key, out T value)
        {
            value = default(T);

            try
            {
                var item = Cache.Get(key);
                if (item == null)
                    return false;

                value = (T)item;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public int Increment(string key, int defaultValue, int value, CacheItemPriority priority = CacheItemPriority.Default)
        {
            lock (_lockObject)
            {
                var current = 0;
                if (!Get(key, out current))
                {
                    current = defaultValue;
                }

                var newValue = current + value;
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
