using System;
using System.Runtime.Caching;
using ServiceStack.Redis;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Caching.Redis
{
    public class RedisCachingProvider : ICachingProvider
    {
        protected RedisClient Client { get; set; }

        public RedisCachingProvider() : this(new RedisClient())
        {
        }

        public RedisCachingProvider(string host)
        {
            if (String.IsNullOrEmpty(host)) throw new ArgumentNullException("host");

            Client = new RedisClient(host);
        }

        public RedisCachingProvider(string host, int port)
        {
            if (String.IsNullOrEmpty(host)) throw new ArgumentNullException("host");

            Client = new RedisClient(host, port);
        }

        public RedisCachingProvider(string host, int port, string password)
        {
            if (String.IsNullOrEmpty(host)) throw new ArgumentNullException("host");

            Client = new RedisClient(host, port, password);
        }

        public RedisCachingProvider(RedisClient client)
        {
            if (client == null) throw new ArgumentNullException("client");

            Client = client;
        }

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="value">Item to be cached</param>
        /// <param name="key">Name of item</param>
        /// <param name="priority"></param>
        /// <param name="timeoutInSeconds">Seconds to cache</param>
        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Default, int? timeoutInSeconds = null)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            if (timeoutInSeconds.HasValue)
            {
                Client.Set(key, value, new TimeSpan(0, 0, 0, timeoutInSeconds.Value));
            }
            else
            {
                Client.Set(key, value);    
            }
            
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public void Clear(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            Client.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

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
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

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
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            // no need to use a lock since the redis increment method is atomic already
            return Convert.ToInt32(Client.Increment(key, Convert.ToUInt32(value)));
        }

        public void Dispose()
        {
            Client.Dispose();
            Client = null;
        }
    }
}
