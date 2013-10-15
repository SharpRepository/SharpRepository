using System;
using SharpRepository.Repository.Caching;
using Raven.Client.Document;

namespace SharpRepository.RavenDbRepository
{
    /// <summary>
    /// RavenDb repository layer
    /// </summary>
    /// <typeparam name="T">The type of object the repository acts on.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class RavenDbRepository<T, TKey> : RavenDbRepositoryBase<T, TKey> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" />.</param>
        public RavenDbRepository(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="url">The URL of the RavenDb instance.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" />.</param>
        public RavenDbRepository(string url, ICachingStrategy<T, TKey> cachingStrategy = null) : base(url, cachingStrategy) 
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="documentStore">The instantiated RavenDb Document Store.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" />.</param>
        public RavenDbRepository(DocumentStore documentStore, ICachingStrategy<T, TKey> cachingStrategy = null) : base(documentStore, cachingStrategy) 
        {
            if (documentStore == null) throw new ArgumentNullException("documentStore");
        }  
    }

    /// <summary>
    /// RavenDb repository layer
    /// </summary>
    /// <typeparam name="T">The type of object the repository acts on.</typeparam>
    public class RavenDbRepository<T> : RavenDbRepositoryBase<T, string> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public RavenDbRepository(ICachingStrategy<T, string> cachingStrategy = null) : base(cachingStrategy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="url">The URL of the RavenDb instance.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public RavenDbRepository(string url, ICachingStrategy<T, string> cachingStrategy = null) : base(url, cachingStrategy)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RavenDbRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="documentStore">The instantiated RavenDb Document Store.</param>
        /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public RavenDbRepository(DocumentStore documentStore, ICachingStrategy<T, string> cachingStrategy = null) : base(documentStore, cachingStrategy)
        {
            if (documentStore == null) throw new ArgumentNullException("documentStore");
        }  
    }
}
