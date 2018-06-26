using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.Redis
{
    public class RedisConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public RedisConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
            int port;
            int? defaultDatabase = null;

            if (!Boolean.TryParse(CachingProviderConfiguration["ssl"], out bool ssl))
            {
                ssl = true;
            }

            if (!String.IsNullOrEmpty(CachingProviderConfiguration["defaultDatabase"]))
            {
                Int32.TryParse(CachingProviderConfiguration["defaultDatabase"], out int parsedDefaultDatabase);
                defaultDatabase = parsedDefaultDatabase;
            }

            // this seems like a dumb way to do this :)
            if (!String.IsNullOrEmpty(CachingProviderConfiguration["password"]))
            {
                if (!Int32.TryParse(CachingProviderConfiguration["port"], out port))
                {
                    throw new ArgumentException("port");
                }

                return new RedisCachingProvider(CachingProviderConfiguration["host"], port, CachingProviderConfiguration["password"], ssl, defaultDatabase);
            }

            if (Int32.TryParse(CachingProviderConfiguration["port"], out port))
            {
                return new RedisCachingProvider(CachingProviderConfiguration["host"], port, ssl, defaultDatabase);
            }

            if (!String.IsNullOrEmpty(CachingProviderConfiguration["host"]))
            {
                return new RedisCachingProvider(CachingProviderConfiguration["host"], ssl, defaultDatabase);
            }

            return new RedisCachingProvider();
        }
    }
}
