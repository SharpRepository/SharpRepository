using System;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.Redis
{
    public class RedisConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public RedisConfigCachingProviderFactory(CachingProviderElement element)
            : base(element)
        {
        }

        public override ICachingProvider GetInstance()
        {
            int port;

            // this seems like a dumb way to do this :)
            if (!String.IsNullOrEmpty(CachingProviderElement["password"]))
            {
                if (!Int32.TryParse(CachingProviderElement["port"], out port))
                {
                    throw new ArgumentException("port");
                }

                return new RedisCachingProvider(CachingProviderElement["host"], port, CachingProviderElement["password"]);
            }

            if (Int32.TryParse(CachingProviderElement["port"], out port))
            {
                return new RedisCachingProvider(CachingProviderElement["host"], port);
            }

            if (!String.IsNullOrEmpty(CachingProviderElement["host"]))
            {
                return new RedisCachingProvider(CachingProviderElement["host"]);
            }

            return new RedisCachingProvider();
        }
    }
}
