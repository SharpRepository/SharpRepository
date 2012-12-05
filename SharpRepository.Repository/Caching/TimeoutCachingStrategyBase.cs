using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Caching;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class TimeoutCachingStrategyBase<T, TKey> : ICachingStrategy<T, TKey> where T : class
    {
        private ICachingProvider _cachingProvider;
        
        public string CachePrefix { get; set; }
        public int TimeoutInSeconds { get; set;  }

        private readonly string _typeFullName;

        internal TimeoutCachingStrategyBase(int timeoutInSeconds, ICachingProvider cachingProvider = null)
        {
            CachingProvider = cachingProvider;
            CachePrefix = "#Repo";
            TimeoutInSeconds = timeoutInSeconds;

            _typeFullName = typeof(T).FullName ?? typeof(T).Name; // sometimes FullName returns null in certain derived type situations, so I added the check to use the Name property if FullName is null
        }

        public ICachingProvider CachingProvider
        {
            get { return _cachingProvider; }
            set { _cachingProvider = value ?? new InMemoryCachingProvider(); }
        }

        public bool TryGetResult<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult result)
        {
            return IsInCache(GetWriteThroughCacheKey(key, selector), out result);
        }

        public void SaveGetResult<TResult>(TKey key, Expression<Func<T, TResult>> selector, TResult result)
        {
            SetCache(GetWriteThroughCacheKey(key, selector), result);
        }

        public bool TryGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
        {
            return IsInCache(GetAllCacheKey(queryOptions, selector), out result);
        }

        public void SaveGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
        {
            SetCache(GetAllCacheKey(queryOptions, selector), result);
        }

        public bool TryFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
        {
            return IsInCache(FindAllCacheKey(criteria, queryOptions, selector), out result);
        }

        public void SaveFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
        {
            SetCache(FindAllCacheKey(criteria, queryOptions, selector), result);
        }

        public bool TryFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out TResult result)
        {
            return IsInCache(FindCacheKey(criteria, queryOptions, selector), out result);
        }

        public void SaveFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, TResult result)
        {
            SetCache(FindCacheKey(criteria, queryOptions, selector), result);
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

        private string GetWriteThroughCacheKey<TResult>(TKey key, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}::{3}", CachePrefix, _typeFullName, key, (selector != null ? selector.ToString() : "null"));
        }

        private string GetAllCacheKey<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}", CachePrefix, _typeFullName, Md5Helper.CalculateMd5("All::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }

        private string FindAllCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}/{3}", CachePrefix, _typeFullName, "FindAll", Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }

        private string FindCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}/{3}", CachePrefix, _typeFullName, "Find", Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }
    }
}
