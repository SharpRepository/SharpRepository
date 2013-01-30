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

            // this seems like a dumb way to do this :)
            if (!String.IsNullOrEmpty(CachingProviderConfiguration["password"]))
            {
                if (!Int32.TryParse(CachingProviderConfiguration["port"], out port))
                {
                    throw new ArgumentException("port");
                }

                return new RedisCachingProvider(CachingProviderConfiguration["host"], port, CachingProviderConfiguration["password"]);
            }

            if (Int32.TryParse(CachingProviderConfiguration["port"], out port))
            {
                return new RedisCachingProvider(CachingProviderConfiguration["host"], port);
            }

            if (!String.IsNullOrEmpty(CachingProviderConfiguration["host"]))
            {
                return new RedisCachingProvider(CachingProviderConfiguration["host"]);
            }

            return new RedisCachingProvider();
        }
    }
}
