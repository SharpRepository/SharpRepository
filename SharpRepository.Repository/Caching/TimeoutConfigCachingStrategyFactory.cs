using SharpRepository.Repository.Configuration;
using System;

namespace SharpRepository.Repository.Caching
{
    public class TimeoutConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {

        public TimeoutConfigCachingStrategyFactory(ICachingStrategyConfiguration config)
            : base(config)
        {
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>(ICachingProvider cachingProvider)
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCachingStrategy<T, TKey>(timeout, cachingProvider)
                       {
                           MaxResults = CachingStrategyConfiguration.MaxResults
                       };
        }

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(ICachingProvider cachingProvider)
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCachingStrategy<T, TKey, TKey2>(timeout, cachingProvider)
                       {
                           MaxResults = CachingStrategyConfiguration.MaxResults
                       };
        }

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(ICachingProvider cachingProvider)
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCachingStrategy<T, TKey, TKey2, TKey3>(timeout, cachingProvider)
            {
                MaxResults = CachingStrategyConfiguration.MaxResults
            };
        }

        public override ICompoundKeyCachingStrategy<T> GetCompoundKeyInstance<T>(ICachingProvider cachingProvider)
        {
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out int timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

            return new TimeoutCompoundKeyCachingStrategy<T>(timeout, cachingProvider)
            {
                MaxResults = CachingStrategyConfiguration.MaxResults
            };
        }
    }
}
