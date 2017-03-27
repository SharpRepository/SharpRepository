using Microsoft.EntityFrameworkCore;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;

namespace SharpRepository.EfCoreRepository
{
    public class EfCoreRepository<T, TKey, TKey2, TKey3> : EfCoreCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> where T : class
    {
        public EfCoreRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
            : base(dbContext, cachingStrategy)
        {
        }
    }

    public class EfCoreRepository<T, TKey, TKey2> : EfCoreCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class
    {
        public EfCoreRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(dbContext, cachingStrategy)
        {
        }
    }

    public class EfCoreCompoundKeyRepository<T> : EfCoreCompoundKeyRepositoryBase<T> where T : class
    {
        public EfCoreCompoundKeyRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T> cachingStrategy = null)
            : base(dbContext, cachingStrategy)
        {
        }
    }

    /// <summary>
    /// Entity Framework repository layer
    /// </summary>
    /// <typeparam name="T">The Entity type</typeparam>
    /// <typeparam name="TKey">The type of the primary key.</typeparam>
    public class EfCoreRepository<T, TKey> : EfCoreRepositoryBase<T, TKey> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository&lt;T, TKey&gt;"/> class.
        /// </summary>
        /// <param name="dbContext">The Entity Framework DbContext.</param>
        /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" /></param>
        public EfCoreRepository(DbContext dbContext, ICachingStrategy<T, TKey> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }
    }

    /// <summary>
    /// Entity Framework repository layer
    /// </summary>
    /// <typeparam name="T">The Entity type</typeparam>
    public class EfCoreRepository<T> : EfCoreRepositoryBase<T, int>, IRepository<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="dbContext">The Entity Framework DbContext.</param>
        /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" /></param>
        public EfCoreRepository(DbContext dbContext, ICachingStrategy<T, int> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }
    }
}