using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class TimeoutCachingStrategyBase<T, TKey> : ICachingStrategy<T, TKey> where T : class
    {
        public ICachingProvider CachingProvider { get; set; }
        public string CachePrefix { get; set; }
        public int TimeoutInSeconds { get; set;  }

        private readonly string _typeFullName;

        internal TimeoutCachingStrategyBase(int timeoutInSeconds, ICachingProvider cachingProvider = null)
        {
            CachingProvider = cachingProvider ?? new InMemoryCachingProvider();
            CachePrefix = "#Repo";
            TimeoutInSeconds = timeoutInSeconds;

            _typeFullName = typeof(T).FullName ?? typeof(T).Name; // sometimes FullName returns null in certain derived type situations, so I added the check to use the Name property if FullName is null
        }

        public bool TryGetResult(TKey key, out T result)
        {
            return IsInCache(GetWriteThroughCacheKey(key), out result);
        }

        public void SaveGetResult(TKey key, T result)
        {
            SetCache(GetWriteThroughCacheKey(key), result);
        }

        public bool TryGetAllResult(IQueryOptions<T> queryOptions, out IEnumerable<T> result)
        {
            return IsInCache(GetAllCacheKey(queryOptions), out result);
        }

        public void SaveGetAllResult(IQueryOptions<T> queryOptions, IEnumerable<T> result)
        {
            SetCache(GetAllCacheKey(queryOptions), result);
        }

        public bool TryFindAllResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out IEnumerable<T> result)
        {
            return IsInCache(FindAllCacheKey(criteria, queryOptions), out result);
        }

        public void SaveFindAllResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, IEnumerable<T> result)
        {
            SetCache(FindAllCacheKey(criteria, queryOptions), result);
        }

        public bool TryFindResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T result)
        {
            return IsInCache(FindCacheKey(criteria, queryOptions), out result);
        }

        public void SaveFindResult(ISpecification<T> criteria, IQueryOptions<T> queryOptions, T result)
        {
            SetCache(FindCacheKey(criteria, queryOptions), result);
        }

        public void Add(TKey key, T result)
        {
            // nothing to do
        }

        public void Update(TKey key, T result)
        {
            // nothing to do
        }

        public void Delete(TKey key, T result)
        {
            // nothing to do
        }

        public void Save()
        {
            // nothing to do
        }

        // helpers

        private bool IsInCache<TCacheItem>(string cacheKey, out TCacheItem result)
        {
            result = default(TCacheItem);

            try
            {
                if (CachingProvider.Get(cacheKey, out result))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // don't let caching errors cause problems for the Repository
            }

            return false;
        }

        private void SetCache<TCacheItem>(string cacheKey, TCacheItem result)
        {
            try
            {
                CachingProvider.Set(cacheKey, result, CacheItemPriority.Default, TimeoutInSeconds);
            }
            catch (Exception)
            {
                // don't let caching errors mess with the repository
            }
        }

        private string GetWriteThroughCacheKey(TKey key)
        {
            return String.Format("{0}/{1}/{2}", CachePrefix, _typeFullName, key);
        }

        private string GetAllCacheKey(IQueryOptions<T> queryOptions)
        {
            return String.Format("{0}/{1}/{2}", CachePrefix, _typeFullName, Md5Helper.CalculateMd5("All:" + queryOptions));
        }

        private string FindAllCacheKey(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            return String.Format("{0}/{1}/{2}/{3}", CachePrefix, _typeFullName, "FindAll", Md5Helper.CalculateMd5(criteria + ":" + queryOptions));
        }

        private string FindCacheKey(ISpecification<T> criteria, IQueryOptions<T> queryOptions)
        {
            return String.Format("{0}/{1}/{2}/{3}", CachePrefix, _typeFullName, "Find", Md5Helper.CalculateMd5(criteria + ":" + queryOptions));
        }
    }
}
