using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using Microsoft.ApplicationServer.Caching;

namespace SharpRepository.Caching.WindowsAzure
{
    public class WindowsAzureConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        DataCacheFactory ClientConfiguration { get; set; }

        public WindowsAzureConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
            return new WindowsAzureCachingProvider(ClientConfiguration);
        }
    }
}
