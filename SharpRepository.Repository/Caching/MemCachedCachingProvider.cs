using System;
using System.Runtime.Caching;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Uses Memcached for as the caching provider.  See <a href="http://memcached.org/">memcached.org</a> for more information on memcached.
    /// </summary>
    public class MemcachedCachingProvider : ICachingProvider
    {
        protected MemcachedClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedCachingProvider"/> class with the default settings based on the configuration file default section.
        /// </summary>
        public MemcachedCachingProvider()
        {
            Client = new MemcachedClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedCachingProvider"/> class from a specific configuration section settings
        /// </summary>
        /// <param name="configSectionName">Name of the configuration file section.</param>
        public MemcachedCachingProvider(string configSectionName)
        {
            Client = new MemcachedClient(configSectionName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedCachingProvider"/> class with an already instantiated <see cref="MemcachedClient"/>.
        /// </summary>
        /// <param name="memcachedClient">Instantiated client already configured</param>
        public MemcachedCachingProvider(MemcachedClient memcachedClient)
        {
            Client = memcachedClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedCachingProvider"/> class with a custom configuration provided.
        /// </summary>
        /// <param name="configuration">Memcached configuration object</param>
        public MemcachedCachingProvider(IMemcachedClientConfiguration configuration)
        {
            Client = new MemcachedClient(configuration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemcachedCachingProvider"/> class with specific settings provided as parameters.
        /// </summary>
        /// <param name="host">IP Address of the host, use 127.0.0.1 for local</param>
        /// <param name="port">Port number, default is 11211</param>
        /// <param name="username">Provide username if authentication is needed</param>
        /// <param name="password">Provide password if authentication is needed</param>
        /// <param name="authenticationType">Defaults to typeof (PlainTextAuthenticator) if username or password provided</param>
        public MemcachedCachingProvider(string host, int port, string username = null, string password = null, Type authenticationType = null)
        {
            var config = new MemcachedClientConfiguration
                             {
                                 Protocol = MemcachedProtocol.Binary
                             };
            config.AddServer(host, port);

            if (!String.IsNullOrEmpty(username) || !String.IsNullOrEmpty(password))
            {
                config.Authentication.Type = authenticationType ?? typeof (PlainTextAuthenticator);
                config.Authentication.Parameters["userName"] = username;
                config.Authentication.Parameters["password"] = password;
            }
         
            Client = new MemcachedClient(config);
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="value">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="priority"></param>
        /// <param name="cacheTime">Seconds to cache</param>
        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Default, int? cacheTime = null)
        {
            if (cacheTime.HasValue)
            {
                Client.Store(StoreMode.Set, key, value, new TimeSpan(0, 0, 0, cacheTime.Value)); // time is in milliseconds in memcache, but we pass in seconds
            }
            else
            {
                Client.Store(StoreMode.Set, key, value);
            }
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Clear(string key)
        {
            Client.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return (Client.Get(key) != null);
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if
        /// item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        public bool Get<T>(string key, out T value)
        {
            try
            {
                value = Client.Get<T>(key);

                if (Object.Equals(value, default(T)))
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

        public int Increment(string key, int defaultValue, int value, CacheItemPriority priority = CacheItemPriority.Default)
        {
            // no need to use a lock since the memcached increment method is atomic already
            return Convert.ToInt32(Client.Increment(key, Convert.ToUInt64(defaultValue), Convert.ToUInt64(value)));
        }

        public void Dispose()
        {
            Client.Dispose();
            Client = null;
        }
    }
}
