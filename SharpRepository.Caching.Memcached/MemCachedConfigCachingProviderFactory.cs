using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using Enyim.Caching.Configuration;
#if NETSTANDARD2_0
using Microsoft.Extensions.Logging;
#endif

namespace SharpRepository.Caching.Memcached
{
    public class MemCachedConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        IMemcachedClientConfiguration ClientConfiguration { get; set; }
#if NETSTANDARD2_0
        ILoggerFactory LoggerFactory { get; set; }
#endif

        public MemCachedConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
#if NET451
            return new MemcachedCachingProvider(ClientConfiguration);
#elif NETSTANDARD2_0
            return new MemcachedCachingProvider(LoggerFactory ?? new Microsoft.Extensions.Logging.LoggerFactory(), ClientConfiguration);
#endif
        }
    }
}
