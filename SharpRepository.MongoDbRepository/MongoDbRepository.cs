using System;
using MongoDB.Driver;
using SharpRepository.Repository.Caching;

namespace SharpRepository.MongoDbRepository
{
    /// <summary>
    /// MongoDb repository layer
    /// </summary>
    /// <typeparam name="T">The type of object the repository acts on.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class MongoDbRepository<T, TKey> : MongoDbRepositoryBase<T, TKey> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy{T,TKey}" />.</param>
        public MongoDbRepository(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString of the MongoDb instance.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" />.</param>
        public MongoDbRepository(string connectionString, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(connectionString, cachingStrategy, null)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString of the MongoDb instance.</param>
        /// <param name="sslSettings">Ssl Settings for mongo connection</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" />.</param>
        public MongoDbRepository(string connectionString, SslSettings sslSettings, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(connectionString, cachingStrategy, sslSettings)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
        }
    }

    /// <summary>
    /// MongoDb repository layer
    /// </summary>
    /// <typeparam name="T">The type of object the repository acts on.</typeparam>
    public class MongoDbRepository<T> : MongoDbRepositoryBase<T, string> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public MongoDbRepository(ICachingStrategy<T, string> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString of the MongoDb instance.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public MongoDbRepository(string connectionString, ICachingStrategy<T, string> cachingStrategy = null)
            : base(connectionString, cachingStrategy, null)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="connectionString">The connectionString of the MongoDb instance.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public MongoDbRepository(string connectionString, SslSettings sslSettings, ICachingStrategy<T, string> cachingStrategy = null)
            : base(connectionString, cachingStrategy, sslSettings)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
        }
    }
}