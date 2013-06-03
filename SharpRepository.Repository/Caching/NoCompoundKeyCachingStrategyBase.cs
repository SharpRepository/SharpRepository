using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public abstract class NoCompoundKeyCachingStrategyBase<T, TKey, TKey2> : ICompoundKeyCachingStrategy<T, TKey, TKey2>
    {
        internal NoCompoundKeyCachingStrategyBase()
        {
        }

        public bool TryGetResult<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, out TResult result)
        {
            result = default(TResult);
            return false;
        }

        public void SaveGetResult<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, TResult result)
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

        public void Add(TKey key, TKey2 key2, T result)
        {

        }

        public void Update(TKey key, TKey2 key2, T result)
        {

        }

        public void Delete(TKey key, TKey2 key2, T result)
        {

        }

        public void Save()
        {

        }

        public string FullCachePrefix { get; private set; }

        public void ClearAll()
        {
            
        }

        public ICachingProvider CachingProvider { get; set; }
    }

    public abstract class NoCompoundKeyCachingStrategyBase<T, TKey, TKey2, TKey3> : ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3>
    {
        internal NoCompoundKeyCachingStrategyBase()
        {
        }

        public bool TryGetResult<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, out TResult result)
        {
            result = default(TResult);
            return false;
        }

        public void SaveGetResult<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, TResult result)
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

        public void Add(TKey key, TKey2 key2, TKey3 key3, T result)
        {

        }

        public void Update(TKey key, TKey2 key2, TKey3 key3, T result)
        {

        }

        public void Delete(TKey key, TKey2 key2, TKey3 key3, T result)
        {

        }

        public void Save()
        {

        }

        public string FullCachePrefix { get; private set; }

        public void ClearAll()
        {
            
        }

        public ICachingProvider CachingProvider { get; set; }
    }
}
