using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.Memcached
{
    public class MemCachedConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public MemCachedConfigCachingProviderFactory(CachingProviderElement element)
            : base(element)
        {
        }

        public override ICachingProvider GetInstance()
        {
            if (String.IsNullOrEmpty(CachingProviderElement["sectionName"]))
            {
                throw new ArgumentException("sectionName is required to load the MemCachedCachingProvider");
            }

            return new MemcachedCachingProvider(CachingProviderElement["sectionName"]);
        }
    }
}
