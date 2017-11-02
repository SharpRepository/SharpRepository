using SharpRepository.Repository.Configuration;

namespace SharpRepository.InMemoryRepository
{
    public class InMemoryRepositoryConfiguration : RepositoryConfiguration
    {
        public InMemoryRepositoryConfiguration(string name, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(InMemoryConfigRepositoryFactory);
        }
    }
}
