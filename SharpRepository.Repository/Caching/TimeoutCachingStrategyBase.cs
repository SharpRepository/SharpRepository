using System;
using System.Linq.Expressions;
using System.Runtime.Caching;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class TimeoutCachingStrategyBase<T, TKey> : CachingStrategyBase<T, TKey> where T : class
    {
        public int TimeoutInSeconds { get; set;  }

        internal TimeoutCachingStrategyBase(int timeoutInSeconds, int? maxResults, ICachingProvider cachingProvider = null)
            : base(maxResults, cachingProvider)
        {
            TimeoutInSeconds = timeoutInSeconds;
        }

        public override void Add(TKey key, T result)
        {
            // update the cached item
            SetCache(GetWriteThroughCacheKey(key), result);
        }

        public override void Update(TKey key, T result)
        {
            // update the cached item
            SetCache(GetWriteThroughCacheKey(key), result);
        }

        public override void Delete(TKey key, T result)
        {
            ClearCache(GetWriteThroughCacheKey(key));
        }

        public override void Save()
        {
            // nothing to do
        }

        // helpers
        protected override  void SetCache<TCacheItem>(string cacheKey, TCacheItem result, IQueryOptions<T> queryOptions = null)
        {
            try
            {
                CachingProvider.Set(cacheKey, result, CacheItemPriority.Default, TimeoutInSeconds);

                if (queryOptions is IPagingOptions)
                {
                    CachingProvider.Set(cacheKey + "=>pagingTotal", ((IPagingOptions)queryOptions).TotalItems, CacheItemPriority.Default, TimeoutInSeconds);
                }
            }
            catch (Exception)
            {
                // don't let caching errors mess with the repository
            }
        }
    }
}
