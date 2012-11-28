using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class InMemoryConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public InMemoryConfigCachingProviderFactory(CachingProviderElement element) 
            : base(element) 
        {
        }

        public override ICachingProvider GetInstance()
        {
            return new InMemoryCachingProvider();
        }
    }
}
