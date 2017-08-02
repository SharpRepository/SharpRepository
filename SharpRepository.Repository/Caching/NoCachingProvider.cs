using Microsoft.Extensions.Caching.Memory;

namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// No caching is done using this provider.
    /// </summary>
    public class NoCachingProvider : ICachingProvider
    {
        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Normal, int? cacheTime = null)
        {
            // do nothing
        }

        public void Clear(string key)
        {
            // do nothing
        }

        public bool Exists(string key)
        {
            return false;
        }

        public bool Get<T>(string key, out T value)
        {
            value = default(T);
            return false;
        }

        public int Increment(string key, int defaultValue, int value, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            return defaultValue;
        }

        public void Dispose()
        {
            
        }
    }
}
