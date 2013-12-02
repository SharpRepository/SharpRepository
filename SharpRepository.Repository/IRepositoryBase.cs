using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Specifications;
using SharpRepository.Repository.Traits;

namespace SharpRepository.Repository
{
    // TODO: I want to use the ICanDelete<> trait so that they aren't defined in 2 places but I can't because the Delete(TKey) can't be in RepositoryBase because it can't be in the CompoundKeyRepository
    public interface IRepositoryBase<T> : ICanAdd<T>, ICanUpdate<T>, ICanBatch<T> where T : class
    {
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
