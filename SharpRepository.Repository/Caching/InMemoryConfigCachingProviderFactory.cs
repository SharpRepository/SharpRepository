using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class InMemoryConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public InMemoryConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config) 
        {
        }

        public override ICachingProvider GetInstance()
        {
            return new InMemoryCachingProvider();
        }
    }
}
