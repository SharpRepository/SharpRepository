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
}