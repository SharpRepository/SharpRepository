using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Transactions;

namespace SharpRepository.Repository
{
    /// <summary>
    /// Repository that acesses <typeparamref name="T"/> entities and has a primary key of type <typeparamref name="TKey"/>
    /// </summary>
    /// <typeparam name="T">The entity type that the repository acts on.</typeparam>
    /// <typeparam name="TKey">The type of the first part of the compound primary key.</typeparam>
    /// <typeparam name="TKey2">The type of the second part of the compound  primary key.</typeparam>
    public interface ICompoundKeyRepository<T, TKey, TKey2> : IRepositoryQueryable<T> where T : class
    {
        /// <summary>
        /// Gets the specified entity of type <typeparamref name="T"/> from the repository by the primary key.
        /// </summary>
        /// <param name="key">The first part of the compound primary key.</param>
        /// <param name="key2">The second part of the compound primary key.</param>
        /// <returns>The entity that matches on the primary key</returns>
        T Get(TKey key, TKey2 key2);

        /// <summary>
        /// Gets the specified entity of type <typeparamref name="T"/> from the repository by the primary key and maps it to the result of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="key">The first part of the compound primary key.</param>
        /// <param name="key2">The second part of the compound primary key.</param>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <returns>The mapped entity based on the selector that matches on the primary key.</returns>
        TResult Get<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector);

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Add(T entity);

        /// <summary>
        /// Adds the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Update(T entity);

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Update(IEnumerable<T> entities);

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
        /// Deletes the specified entity by the primary key.
        /// </summary>
        /// <param name="key">The first part of the primary key.</param>
        /// <param name="key">The second part of the primary key.</param>
        void Delete(TKey key, TKey2 key2);

        /// <summary>
        /// Begins a batch mode process.  This allows multiple operations against the repository with the ability to commit or rollback.
        /// </summary>
        /// <returns></returns>
        IBatch<T> BeginBatch();

        ICompoundKeyCachingStrategy<T, TKey, TKey2> CachingStrategy { get; set; }
    }
}