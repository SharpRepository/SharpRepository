using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class InMemoryCachingProviderConfiguration : CachingProviderConfiguration
    {
        public InMemoryCachingProviderConfiguration(string name)
        {
            Name = name;
            Factory = typeof (InMemoryConfigCachingProviderFactory);
        }
    }
}
