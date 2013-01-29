using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Implements no caching within the repository.
    /// </summary>
    /// <typeparam name="T">The type of the repository entity.</typeparam>
    /// <typeparam name="TKey">The type of the first part of the compound primary key.</typeparam>
    /// <typeparam name="TKey2">The type of the second part of the compound primary key.</typeparam>
    public class NoCachingStrategy<T, TKey, TKey2> : NoCompoundKeyCachingStrategyBase<T, TKey, TKey2>
    {

    }

    /// <summary>
    /// Implements no caching within the repository.
    /// </summary>
    /// <typeparam name="T">The type of the repository entity.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class NoCachingStrategy<T, TKey> : NoCachingStrategyBase<T, TKey>
    {
        
    }

    /// <summary>
    /// Implements no caching within the repository with the primary key of the entity being Int32
    /// </summary>
    /// <typeparam name="T">The type of the repository entity.</typeparam>
    public class NoCachingStrategy<T> : NoCachingStrategyBase<T, int>
    {

    }

    public class NoCompoundKeyCachingStrategy<T> :  ICompoundKeyCachingStrategy<T>
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

        public ICachingProvider CachingProvider { get; set; }
    }
}
