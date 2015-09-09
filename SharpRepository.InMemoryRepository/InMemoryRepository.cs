using SharpRepository.Repository;
using SharpRepository.Repository.Caching;

namespace SharpRepository.InMemoryRepository
{
    public class InMemoryRepository<T, TKey> : InMemoryRepositoryBase<T, TKey> where T : class, new()
    {
        public InMemoryRepository(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {   
        }
    }

    // The IRepository<T> is needed here and not on the one above in order to allow programming against IRepository<T> when using an int as the PK
    public class InMemoryRepository<T> : InMemoryRepositoryBase<T, int>, IRepository<T> where T : class, new()
    {
        public InMemoryRepository(ICachingStrategy<T, int> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }
    }

    public class InMemoryCompoundKeyRepository<T> : InMemoryCompoundKeyRepositoryBase<T> where T : class, new()
    {
        public InMemoryCompoundKeyRepository(ICompoundKeyCachingStrategy<T> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }
    }

    public class InMemoryRepository<T, TKey, TKey2> : InMemoryCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        public InMemoryRepository(ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }
    }

    public class InMemoryRepository<T, TKey, TKey2, TKey3> : InMemoryCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> where T : class, new()
    {
        public InMemoryRepository(ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }
    }
}