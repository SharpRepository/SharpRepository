using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Traits;

namespace SharpRepository.Repository
{
    public interface IRepositoryQueryable<T> : ICanFind<T>, IDisposable where T : class
    {
        IQueryable<T> AsQueryable(); // don't want this public, just for POC

        IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class;

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <returns>All of the entities</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets all entities from the repository and applies the query options (e.g. paging or sorting options)
        /// </summary>
        /// <param name="fetchStrategy">The include syntax to eager load data.</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy);

        IEnumerable<T> GetAll(params string[] includePaths);

        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths);

        /// <summary>
        /// Gets all entities from the repository and applies the query options (e.g. paging or sorting options)
        /// </summary>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(IQueryOptions<T> queryOptions);

        /// <summary>
        /// Gets all entities from the repository and applies the query options (e.g. paging or sorting options)
        /// </summary>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <param name="fetchStrategy">The include syntax to eager load data.</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

        IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths);

        IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths);

        /// <summary>
        /// Gets all entities from the repository, applies the query options (e.g. paging or sorting options) and maps to a new types based on the selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector);

        /// <summary>
        /// Gets all entities from the repository, applies the query options (e.g. paging or sorting options) and maps to a new types based on the selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions);

        /// <summary>
        /// Gets all entities from the repository, applies the query options (e.g. paging or sorting options) and maps to a new types based on the selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="fetchStrategy">The include syntax to eager load data.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy);

        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths);

        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths);
        
        /// <summary>
        /// Gets all entities from the repository, applies the query options (e.g. paging or sorting options) and maps to a new types based on the selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <param name="fetchStrategy">The include syntax to eager load data.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths);

        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths);
    }
}
