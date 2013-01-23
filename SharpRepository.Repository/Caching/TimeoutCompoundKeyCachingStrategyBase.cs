using System;
using System.Linq.Expressions;
using System.Runtime.Caching;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class TimeoutCompoundKeyCachingStrategyBase<T, TKey, TKey2> : CompoundKeyCachingStrategyBase<T, TKey, TKey2> where T : class
    {
        public int TimeoutInSeconds { get; set;  }

        internal TimeoutCompoundKeyCachingStrategyBase(int timeoutInSeconds, ICachingProvider cachingProvider = null)
            : base(cachingProvider)
        {
            TimeoutInSeconds = timeoutInSeconds;
        }


        public override void Add(TKey key, TKey2 key2, T result)
        {
            // nothing to do
        }

        public override void Update(TKey key, TKey2 key2, T result)
        {
            // nothing to do
        }

        public override void Delete(TKey key, TKey2 key2, T result)
        {
            // nothing to do
        }

        public override void Save()
        {
            // nothing to do
        }

        // helpers
        protected new void SetCache<TCacheItem>(string cacheKey, TCacheItem result, IQueryOptions<T> queryOptions = null)
        {
            try
            {
                CachingProvider.Set(cacheKey, result, CacheItemPriority.Default, TimeoutInSeconds);

                if (queryOptions is PagingOptions<T>)
                {
                    CachingProvider.Set(cacheKey + "=>pagingTotal", ((PagingOptions<T>)queryOptions).TotalItems, CacheItemPriority.Default, TimeoutInSeconds);
                }
            }
            catch (Exception)
            {
                // don't let caching errors mess with the repository
            }
        }

        protected override string GetAllCacheKey<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}", CachePrefix, TypeFullName, Md5Helper.CalculateMd5("All::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }

        protected override string FindAllCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}/{3}", CachePrefix, TypeFullName, "FindAll", Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }

        protected override string FindCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
        {
            return String.Format("{0}/{1}/{2}/{3}", CachePrefix, TypeFullName, "Find", Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null")));
        }
    }
}
