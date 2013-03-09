using SharpRepository.Repository;
using SharpRepository.Repository.Caching;

namespace SharpRepository.CacheRepository
{
    public class CacheRepository<T, TKey> : CacheRepositoryBase<T, TKey> where T : class, new()
    {
        public CacheRepository(string prefix, ICachingProvider cachingProvider, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(prefix, cachingProvider, cachingStrategy)
        {
        }

        public CacheRepository(string prefix, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(prefix, cachingStrategy)
        {
        }
    }

    public class CacheRepository<T> : CacheRepositoryBase<T, int>, IRepository<T> where T : class, new()
    {
        public CacheRepository(string prefix, ICachingProvider cachingProvider, ICachingStrategy<T, int> cachingStrategy = null)
            : base(prefix, cachingProvider, cachingStrategy)
        {
        }

        public CacheRepository(string prefix, ICachingStrategy<T, int> cachingStrategy = null)
            : base(prefix, cachingStrategy)
        {
        }
    }

    public class CacheCompoundKeyRepository<T> : CacheCompoundKeyRepositoryBase<T> where T : class, new()
    {
        public CacheCompoundKeyRepository(string prefix, ICompoundKeyCachingStrategy<T> cachingStrategy = null)
            : base(prefix, cachingStrategy)
        {
        }
    }

    public class CacheRepository<T, TKey, TKey2> : CacheCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        public CacheRepository(string prefix, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(prefix, cachingStrategy)
        {
        }
    }
}
