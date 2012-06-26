using System;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Db4o
{
    /// <summary>
    /// XML Repository layer
    /// </summary>
    /// <typeparam name="T">The object type that is stored as XML.</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class Db4oRepository<T, TKey> : Db4oRepositoryBase<T, TKey> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Db4oRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="storagePath">Path to the directory where the yap files are stored.  The yap filename is determined by the TypeName</param>
        /// <param name="cachingStrategy">The caching strategy. Defaults to <see cref="NoCachingStrategy{T,TKey}" />.</param>
        public Db4oRepository(string storagePath, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(storagePath, cachingStrategy)
        {
            if (String.IsNullOrEmpty(storagePath)) throw new ArgumentNullException("storagePath");
        }
    }

    /// <summary>
    /// XML Repository layer
    /// </summary>
    /// <typeparam name="T">The object type that is stored.</typeparam>
    public class Db4oRepository<T> : Db4oRepositoryBase<T, int> where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Db4oRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="storagePath">Path to the directory where the yap files are stored.  The yap filename is determined by the TypeName</param>
        /// <param name="cachingStrategy">The caching strategy. Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
        public Db4oRepository(string storagePath, ICachingStrategy<T, int> cachingStrategy = null)
            : base(storagePath, cachingStrategy)
        {
        }
    }
}
