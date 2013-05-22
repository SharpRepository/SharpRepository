using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;


// TODO: I want to use the ICanGet<> trait so that they aren't defined in 2 places but I can't because the GetAll is in IRepositoryQueryable and not in here, but it needs to be in ICanGet

namespace SharpRepository.Repository
{
    /// <summary>
    /// Repository that acesses <typeparamref name="T"/> entities and has a primary key of type <typeparamref name="TKey"/>
    /// </summary>
    /// <typeparam name="T">The entity type that the repository acts on.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public interface IRepository<T, TKey> : IRepositoryBase<T>, IRepositoryQueryable<T>  where T : class
    {
        IRepositoryConventions Conventions { get; set; }

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
        /// Returns true if the specified entity of type <typeparamref name="T"/> from the repository by the primary key exists
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool Exists(TKey key);

        /// <summary>
        /// Returns true if the specified entity of type <typeparamref name="T"/> from the repository by the primary key exists
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryGet(TKey key, out T entity);

        /// <summary>
        /// Returns true if the specified entity of type <typeparamref name="T"/> from the repository by the primary key exists
        /// </summary>
        /// <param name="key">The primary key.</param>
        /// <param name="selector">The mapping selector that returns the result type.</param>
        /// <param name="entity">The entity that was found</param>
        /// <returns>True if the entity exists, false if it does not</returns>
        bool TryGet<TResult>(TKey key, Expression<Func<T, TResult>> selector, out TResult entity);

        /// <summary>
        /// Deletes the specified entity by the primary key.
        /// </summary>
        /// <param name="key">The primary key.</param>
        void Delete(TKey key);

        ICachingStrategy<T, TKey> CachingStrategy { get; set; }

        bool CachingEnabled { get; set; }

        bool CacheUsed { get; }

        IDisableCache DisableCaching();
    }

    /// <summary>
    /// Defaults to int as the Primary Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> : IRepository<T, int> where T : class
    {
    }
}