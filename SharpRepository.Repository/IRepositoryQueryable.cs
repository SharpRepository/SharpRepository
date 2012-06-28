using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository
{
    public interface IRepositoryQueryable<T, in TKey> : IDisposable where T : class
    {
        IQueryable<T> AsQueryable(); // don't want this public, just for POC

        IRepositoryQueryable<TResult, TResultKey> Join<TOuterKey, TInner, TResult, TResultKey>(IRepositoryQueryable<TInner, TOuterKey> innerRepository, Expression<Func<T, TOuterKey>> outerKeySelector, Expression<Func<TInner, TOuterKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
            where TInner : class
            where TResult : class, new();
            
        /// <summary>
        /// Gets the specified entity of type <typeparamref name="T"/> from the repository by the primary key.
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <returns>The entity that matches on the primary key</returns>
        T Get(TKey key);

        /// <summary>
        /// Gets the specified entity of type <typeparamref name="T"/> from the repository by the primary key and maps it to the result of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="key">The primary key.</param>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <returns>The mapped entity based on the selector that matches on the primary key.</returns>
        TResult Get<TResult>(TKey key, Expression<Func<T, TResult>> selector);

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <returns>All of the entities</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets all entities from the repository and applies the query options (e.g. paging or sorting options)
        /// </summary>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<T> GetAll(IQueryOptions<T> queryOptions);

        /// <summary>
        /// Gets all entities from the repository, applies the query options (e.g. paging or sorting options) and maps to a new types based on the selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options, and maps it to a new type based on the selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <returns></returns>
        TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds a single entity that matches the specification criteria using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <returns></returns>
        T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds a single entity that matches the specification criteria using FirstOrDefault with an optional query option, like sorting options, and maps to a new type based on the selector.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <returns></returns>
        TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds all entities that match the predicate with an optional query option applied (like paging or sorting).
        /// </summary>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds all entities that match the predicate with an optional query option applied (like paging or sorting), and maps to a new type based on the selector provided.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds all entities that match the specification criteria with an optional query option applied (like paging or sorting).
        /// </summary>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null);

        /// <summary>
        /// Finds all entities that match the specification criteria with an optional query option applied (like paging or sorting), and maps to a new type based on the selector provided.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="queryOptions">The query options to apply like paging or sorting.</param>
        /// <returns></returns>
        IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null);
    }
}
