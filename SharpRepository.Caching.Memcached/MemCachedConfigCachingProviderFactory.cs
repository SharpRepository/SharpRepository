using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using Enyim.Caching.Configuration;

namespace SharpRepository.Caching.Memcached
{
    public class MemCachedConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        IMemcachedClientConfiguration ClientConfiguration { get; set; }

        public MemCachedConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
            
            return new MemcachedCachingProvider(ClientConfiguration);
        }
    }
}
