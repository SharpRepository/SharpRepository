using System;
using System.Linq.Expressions;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository
{
    /// <summary>
    /// Repository that acesses <typeparamref name="T"/> entities and has a primary key of type <typeparamref name="TKey"/>
    /// </summary>
    /// <typeparam name="T">The entity type that the repository acts on.</typeparam>
    /// <typeparam name="TKey">The type of the first part of the compound primary key.</typeparam>
    /// <typeparam name="TKey2">The type of the second part of the compound  primary key.</typeparam>
    public interface ICompoundKeyRepository<T, TKey, TKey2> : IRepositoryBase<T>, IRepositoryQueryable<T> where T : class
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
        /// Deletes the specified entity by the compound primary key.
        /// </summary>
        /// <param name="key">The first part of the compound primary key.</param>
        /// <param name="key2">The second part of the compound primary key.</param>
        void Delete(TKey key, TKey2 key2);

        ICompoundKeyCachingStrategy<T, TKey, TKey2> CachingStrategy { get; set; }
    }
}