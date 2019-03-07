using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class NoCachingStrategyBase<T, TKey> : ICachingStrategy<T, TKey>
    {
        internal NoCachingStrategyBase()
        {
        }

        public bool TryGetResult(TKey key, out T result)
        {
            result = default(T);
            return false;
        }

        public void SaveGetResult(TKey key, T result)
        {

        }

        public bool TryGetResult<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult result)
        {
            result = default(TResult);
            return false;
        }

        public void SaveGetResult<TResult>(TKey key, Expression<Func<T, TResult>> selector, TResult result)
        {

        }
        
        public bool TryGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
        {
            result = default(IEnumerable<TResult>);
            return false;
        }

        public void SaveGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
        {

        }

        public bool TryFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
        {
            result = default(IEnumerable<TResult>);
            return false;
        }

        public void SaveFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
        {

        }

        public bool TryFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out TResult result)
        {
            result = default(TResult);
            return false;
        }

        public void SaveFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, TResult result)
        {

        }

        public virtual bool TryGroupResult<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector, ISpecification<T> criteria, out IEnumerable<TResult> result)
        {
            result = default(IEnumerable<TResult>);
            return false;
        }

        public virtual void SaveGroupResult<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector, ISpecification<T> criteria, IEnumerable<TResult> result)
        {

        }

        public virtual bool TryCountResult(ISpecification<T> criteria, out int count)
        {
            count = 0;
            return false;
        }

        public virtual void SaveCountResult(ISpecification<T> criteria, int count)
        {

        }

        public virtual bool TryLongCountResult(ISpecification<T> criteria, out long count)
        {
            count = 0;
            return false;
        }

        public virtual void SaveLongCountResult(ISpecification<T> criteria, long count)
        {

        }

        public virtual bool TrySumResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, out TResult sum)
        {
            sum = default(TResult);
            return false;
        }

        public virtual void SaveSumResult<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria, TResult sum)
        {

        }

        public virtual bool TryAverageResult<TSelector, TResult>(Expression<Func<T, TSelector>> selector, ISpecification<T> criteria, out TResult sum)
        {
            sum = default(TResult);
            return false;
        }

        public virtual void SaveAverageResult<TSelector, TResult>(Expression<Func<T, TSelector>> selector, ISpecification<T> criteria, TResult sum)
        {

        }

        public virtual bool TryMinResult<TSum>(Expression<Func<T, TSum>> selector, ISpecification<T> criteria, out TSum sum)
        {
            sum = default(TSum);
            return false;
        }

        public virtual void SaveMinResult<TSum>(Expression<Func<T, TSum>> selector, ISpecification<T> criteria, TSum sum)
        {

        }

        public virtual bool TryMaxResult<TSum>(Expression<Func<T, TSum>> selector, ISpecification<T> criteria, out TSum sum)
        {
            sum = default(TSum);
            return false;
        }

        public virtual void SaveMaxResult<TSum>(Expression<Func<T, TSum>> selector, ISpecification<T> criteria, TSum sum)
        {

        }

        public void Add(TKey key, T result)
        {

        }

        public void Update(TKey key, T result)
        {

        }

        public void Delete(TKey key, T result)
        {

        }

        public void Save()
        {

        }

        public string CachePrefix { set; private get; }

        public string FullCachePrefix { get; private set; }

        public void ClearAll()
        {
            
        }

        public ICachingProvider CachingProvider { get; set; }
    }
}
