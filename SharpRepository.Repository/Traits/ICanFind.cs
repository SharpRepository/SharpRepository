using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanFind interface exposes only the "Find" methods of the
    /// Repository.   
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/>       
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    public interface ICanFind<T>
    {
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
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault.
        /// </summary>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool Exists(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault.
        /// </summary>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind(Expression<Func<T, bool>> predicate, out T entity);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="predicate">The predicate used to match entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity);

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
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault.
        /// </summary>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool Exists(ISpecification<T> criteria);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault.
        /// </summary>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind(ISpecification<T> criteria, out T entity);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity);

        /// <summary>
        /// Returns true if there is a single entity that matches the predicate supplied using FirstOrDefault with an optional query option, like sorting options.
        /// </summary>
        /// <param name="criteria">The specification criteria that is used for matching entities against.</param>
        /// <param name="selector">The mapping selector that returns the result type</param>
        /// <param name="queryOptions">The query options to use (usually sorting) since this uses FirstOrDefault.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity);


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