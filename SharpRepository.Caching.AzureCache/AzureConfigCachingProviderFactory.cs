using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.AzureCache
{
    public class AzureConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public AzureConfigCachingProviderFactory(ICachingProviderConfiguration config) : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
            return new AzureCachingProvider(CachingProviderConfiguration["cacheName"]);
        }
    }
}
