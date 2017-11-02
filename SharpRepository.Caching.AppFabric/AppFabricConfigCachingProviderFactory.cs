using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using Microsoft.ApplicationServer.Caching;

namespace SharpRepository.Caching.AppFabric
{
    public class AppFabricConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        DataCacheFactoryConfiguration ClientConfiguration { get; set; }

        public AppFabricConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
            return new AppFabricCachingProvider(ClientConfiguration);
        }
    }
}
