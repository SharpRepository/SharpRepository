using System;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Caching
{
    public abstract class CompoundKeyCachingStrategyBase<T> : CompoundKeyCachingStrategyCommon<T>, ICompoundKeyCachingStrategy<T> where T : class
    {
        internal CompoundKeyCachingStrategyBase()
        {
        }
        internal CompoundKeyCachingStrategyBase(int? maxResults)
            : base(maxResults)
        {
        }

        internal CompoundKeyCachingStrategyBase(int? maxResults, ICachingProvider cachingProvider)
            : base(maxResults, cachingProvider)
        {
        }

        public virtual bool TryGetResult<TResult>(object[] keys, Expression<Func<T, TResult>> selector, out TResult result)
        {
            return IsInCache(GetWriteThroughCacheKey(keys, selector), out result);
        }

        public virtual void SaveGetResult<TResult>(object[] keys, Expression<Func<T, TResult>> selector, TResult result)
        {
            SetCache(GetWriteThroughCacheKey(keys, selector), result);
        }

        public abstract void Add(object[] keys, T result);

        public abstract void Update(object[] keys, T result);

        public abstract void Delete(object[] keys, T result);
    }

    public abstract class CompoundKeyCachingStrategyBase<T, TKey, TKey2> : CompoundKeyCachingStrategyCommon<T>, ICompoundKeyCachingStrategy<T, TKey, TKey2> where T : class
    {
       internal CompoundKeyCachingStrategyBase()
       {
       }

        internal CompoundKeyCachingStrategyBase(int? maxResults)
          : base(maxResults)
        {
        }
        
        internal CompoundKeyCachingStrategyBase(int? maxResults, ICachingProvider cachingProvider)
           : base(maxResults, cachingProvider)
       {
       }

       public virtual bool TryGetResult<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, out TResult result)
       {
           return IsInCache(GetWriteThroughCacheKey(key, key2, selector), out result);
       }

       public virtual void SaveGetResult<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, TResult result)
       {
           SetCache(GetWriteThroughCacheKey(key, key2, selector), result);
       }

       public abstract void Add(TKey key, TKey2 key2, T result);

       public abstract void Update(TKey key, TKey2 key2, T result);

       public abstract void Delete(TKey key, TKey2 key2, T result);

       protected string GetWriteThroughCacheKey<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector)
       {
           return GetWriteThroughCacheKey(new object[] {key, key2}, selector);
       }
    }

    public abstract class CompoundKeyCachingStrategyBase<T, TKey, TKey2, TKey3> : CompoundKeyCachingStrategyCommon<T>, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> where T : class
    {
        internal CompoundKeyCachingStrategyBase()
        {
        }

        internal CompoundKeyCachingStrategyBase(int? maxResults)
            : base(maxResults)
        {
        }

        internal CompoundKeyCachingStrategyBase(int? maxResults, ICachingProvider cachingProvider)
            : base(maxResults, cachingProvider)
        {
        }

        public virtual bool TryGetResult<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, out TResult result)
        {
            return IsInCache(GetWriteThroughCacheKey(key, key2, key3, selector), out result);
        }

        public virtual void SaveGetResult<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, TResult result)
        {
            SetCache(GetWriteThroughCacheKey(key, key2, key3, selector), result);
        }

        public abstract void Add(TKey key, TKey2 key2, TKey3 key3, T result);

        public abstract void Update(TKey key, TKey2 key2, TKey3 key3, T result);

        public abstract void Delete(TKey key, TKey2 key2, TKey3 key3, T result);

        protected string GetWriteThroughCacheKey<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector)
        {
            return GetWriteThroughCacheKey(new object[] { key, key2 }, selector);
        }
    }
}
