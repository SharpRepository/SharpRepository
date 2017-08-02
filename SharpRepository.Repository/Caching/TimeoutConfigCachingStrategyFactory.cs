using System;
using Microsoft.Extensions.Caching.Memory;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class TimeoutConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        protected IMemoryCache Cache;

        public TimeoutConfigCachingStrategyFactory(ICachingStrategyConfiguration config, IMemoryCache cache)
            : base(config)
        {
            Cache = cache;
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>()
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCachingStrategy<T, TKey>(timeout, new InMemoryCachingProvider(Cache))
                       {
                           MaxResults = CachingStrategyConfiguration.MaxResults
                       };
        }

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCachingStrategy<T, TKey, TKey2>(timeout, new InMemoryCachingProvider(Cache))
                       {
                           MaxResults = CachingStrategyConfiguration.MaxResults
                       };
        }

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCachingStrategy<T, TKey, TKey2, TKey3>(timeout, new InMemoryCachingProvider(Cache))
            {
                MaxResults = CachingStrategyConfiguration.MaxResults
            };
        }

        public override ICompoundKeyCachingStrategy<T> GetCompoundKeyInstance<T>()
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCompoundKeyCachingStrategy<T>(timeout, new InMemoryCachingProvider(Cache))
            {
                MaxResults = CachingStrategyConfiguration.MaxResults
            };
        }
    }
}
