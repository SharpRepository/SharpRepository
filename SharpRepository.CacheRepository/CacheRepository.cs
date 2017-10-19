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
    }

    public class CacheRepository<T> : CacheRepositoryBase<T, int>, IRepository<T> where T : class, new()
    {
        public CacheRepository(string prefix, ICachingProvider cachingProvider, ICachingStrategy<T, int> cachingStrategy = null)
            : base(prefix, cachingProvider, cachingStrategy)
        {
        }
    }

    public class CacheCompoundKeyRepository<T> : CacheCompoundKeyRepositoryBase<T> where T : class, new()
    {
        public CacheCompoundKeyRepository(string prefix, ICachingProvider cachingProvider, ICompoundKeyCachingStrategy<T> cachingStrategy = null)
            : base(prefix, cachingProvider, cachingStrategy)
        {
        }
    }

    public class CacheRepository<T, TKey, TKey2> : CacheCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        public CacheRepository(string prefix, ICachingProvider cachingProvider, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(prefix, cachingProvider, cachingStrategy)
        {
        }
    }

    public class CacheRepository<T, TKey, TKey2, TKey3> : CacheCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> where T : class, new()
    {
        public CacheRepository(string prefix, ICachingProvider cachingProvider, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
            : base(prefix, cachingProvider, cachingStrategy)
        {
        }
    }
}
