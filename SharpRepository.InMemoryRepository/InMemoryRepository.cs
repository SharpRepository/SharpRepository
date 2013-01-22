using SharpRepository.Repository.Caching;

namespace SharpRepository.InMemoryRepository
{
    public class InMemoryRepository<T, TKey, TKey2> : InMemoryCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        public InMemoryRepository(ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }
    }

    public class InMemoryRepository<T, TKey> : InMemoryRepositoryBase<T, TKey> where T : class, new()
    {
        public InMemoryRepository(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {   
        }
    }

    public class InMemoryRepository<T> : InMemoryRepositoryBase<T, int> where T : class, new()
    {
        public InMemoryRepository(ICachingStrategy<T, int> cachingStrategy = null)
            : base(cachingStrategy)
        {
        }
    }
}