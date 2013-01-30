using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.Memcached
{
    public class MemCachedConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public MemCachedConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
            if (String.IsNullOrEmpty(CachingProviderConfiguration["sectionName"]))
            {
                throw new ArgumentException("sectionName is required to load the MemCachedCachingProvider");
            }

            return new MemcachedCachingProvider(CachingProviderConfiguration["sectionName"]);
        }
    }
}
