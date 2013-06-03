using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    public class NoCompoundKeyCachingStrategy<T> : ICompoundKeyCachingStrategy<T>
    {
        internal NoCompoundKeyCachingStrategy()
        {
        }

        public bool TryGetResult<TResult>(object[] keys, Expression<Func<T, TResult>> selector, out TResult result)
        {
            result = default(TResult);
            return false;
        }

        public void SaveGetResult<TResult>(object[] keys, Expression<Func<T, TResult>> selector, TResult result)
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

        public void Add(object[] keys, T result)
        {

        }

        public void Update(object[] keys, T result)
        {

        }

        public void Delete(object[] keys, T result)
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
