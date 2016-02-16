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
            bool ssl;

            if (!Boolean.TryParse(CachingProviderConfiguration["ssl"], out ssl))
            {
                ssl = true;
            }

            // this seems like a dumb way to do this :)
            if (!String.IsNullOrEmpty(CachingProviderConfiguration["password"]))
            {
                if (!Int32.TryParse(CachingProviderConfiguration["port"], out port))
                {
                    throw new ArgumentException("port");
                }

                return new RedisCachingProvider(CachingProviderConfiguration["host"], port, CachingProviderConfiguration["password"], ssl);
            }

            if (Int32.TryParse(CachingProviderConfiguration["port"], out port))
            {
                return new RedisCachingProvider(CachingProviderConfiguration["host"], port, ssl);
            }

            if (!String.IsNullOrEmpty(CachingProviderConfiguration["host"]))
            {
                return new RedisCachingProvider(CachingProviderConfiguration["host"], ssl);
            }

            return new RedisCachingProvider();
        }
    }
}
