using System;
using System.Linq.Expressions;

namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Implements Write-Through caching for all CRUD operations (writing to the database and cache at the same time), and Generational caching for all queries (FindAll, GetAll, Find).
    /// </summary>
    /// <typeparam name="T">Type of the entity the corresponding repository queries against.</typeparam>
    public class StandardCachingStrategy<T> : StandardCachingStrategyBase<T, int, int> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCachingStrategy&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="cachingProvider">The caching provider to use (e.g. <see cref="InMemoryCachingProvider"/>, <see cref="MemcachedCachingProvider"/>, etc.).  Defaults to <see cref="InMemoryCachingProvider"/>.</param>
        public StandardCachingStrategy(ICachingProvider cachingProvider) 
            : base(null, cachingProvider)
        {
            Partition = null;
        }
    }

    /// <summary>
    /// Implements Write-Through caching for all CRUD operations (writing to the database and cache at the same time), and Generational caching for all queries (FindAll, GetAll, Find).
    /// </summary>
    /// <typeparam name="T">Type of the entity the corresponding repository queries against.</typeparam>
    /// <typeparam name="TKey">The primary key type of the entity</typeparam>
    public class StandardCachingStrategy<T, TKey> : StandardCachingStrategyBase<T, TKey, int> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCachingStrategy&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="cachingProvider">The caching provider to use (e.g. <see cref="InMemoryCachingProvider"/>, <see cref="MemcachedCachingProvider"/>, etc.).  Defaults to <see cref="InMemoryCachingProvider"/>.</param>
        public StandardCachingStrategy(ICachingProvider cachingProvider)
            : base(null, cachingProvider)
        {
            Partition = null;
        }
    }

    /// <summary>
    /// Implements Write-Through caching for all CRUD operations (writing to the database and cache at the same time), and Generational caching for all queries (FindAll, GetAll, Find) with the option to partition the Generational Cache based on a specific entity property for better performance in certain situations.
    /// </summary>
    /// <typeparam name="T">Type of the entity the corresponding repository queries against.</typeparam>
    /// <typeparam name="TKey">The first part of the compound primary key type of the entity</typeparam>
    /// <typeparam name="TKey2">The second part of the compound primary key type of the entity</typeparam>
    public class StandardCachingStrategy<T, TKey, TKey2> : StandardCompoundKeyCachingStrategyBase<T, TKey, TKey2, int> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCachingStrategy&lt;T, TKey, TKey2&gt;"/> class.
        /// </summary>
        /// <param name="cachingProvider">The caching provider to use (e.g. <see cref="InMemoryCachingProvider"/>, <see cref="MemcachedCachingProvider"/>, etc.).  Defaults to <see cref="InMemoryCachingProvider"/>.</param>
        public StandardCachingStrategy(ICachingProvider cachingProvider)
            : base(null, cachingProvider)
        {
            Partition = null;
        }
    }

    /// <summary>
    /// Implements Write-Through caching for all CRUD operations (writing to the database and cache at the same time), and Generational caching for all queries (FindAll, GetAll, Find) with the option to partition the Generational Cache based on a specific entity property for better performance in certain situations.
    /// </summary>
    /// <typeparam name="T">Type of the entity the corresponding repository queries against.</typeparam>
    /// <typeparam name="TKey">The first part of the compound primary key type of the entity</typeparam>
    /// <typeparam name="TKey2">The second part of the compound primary key type of the entity</typeparam>
    public class StandardCachingStrategy<T, TKey, TKey2, TKey3> : StandardCompoundKeyCachingStrategyBase<T, TKey, TKey2, TKey3, int> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCachingStrategy&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="cachingProvider">The caching provider to use (e.g. <see cref="InMemoryCachingProvider"/>, <see cref="MemcachedCachingProvider"/>, etc.).  Defaults to <see cref="InMemoryCachingProvider"/>.</param>
        public StandardCachingStrategy(ICachingProvider cachingProvider)
            : base(null, cachingProvider)
        {
            Partition = null;
        }
    }
}
