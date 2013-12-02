using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.Repository.Traits
{
    /// <summary>
    /// Based on the Interface Segregation Principle (ISP), the
    /// ICanDelete interface exposes only the "Delete" methods of the
    /// Repository.    
    /// <see cref="http://richarddingwall.name/2009/01/19/irepositoryt-one-size-does-not-fit-all/"/>      
    /// </summary>
    /// <typeparam name="T">Generic repository entity type</typeparam>
    /// <typeparam name="TKey">Generic repository entity key type</typeparam>
    public interface ICanDelete<T, in TKey>
    {
        void Delete(TKey key);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Deletes all matching entities
        /// </summary>
        /// <param name="predicate">Query</param>
        void Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Deletes all matching entities
        /// </summary>
        /// <param name="criteria">Query</param>
        void Delete(ISpecification<T> criteria);
    }
}