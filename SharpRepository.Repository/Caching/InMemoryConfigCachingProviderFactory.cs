using Microsoft.Extensions.Caching.Memory;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class InMemoryConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        protected IMemoryCache cache;
        public InMemoryConfigCachingProviderFactory(ICachingProviderConfiguration config, IMemoryCache memoryCache)
            : base(config) 
        {
            cache = memoryCache;
        }

        public override ICachingProvider GetInstance()
        {
            return new InMemoryCachingProvider(cache);
        }
    }
}
