using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using Microsoft.Extensions.Logging;
using Enyim.Caching.Configuration;

namespace SharpRepository.Caching.Memcached
{
    public class MemCachedConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        ILoggerFactory LoggerFactory { get; set; }
        IMemcachedClientConfiguration ClientConfiguration { get; set; }

        public MemCachedConfigCachingProviderFactory(ICachingProviderConfiguration config, ILoggerFactory loggerFactory)
            : base(config)
        {
            LoggerFactory = loggerFactory;
        }

        public override ICachingProvider GetInstance()
        {
            
            return new MemcachedCachingProvider(LoggerFactory, ClientConfiguration);
        }
    }
}
