using System;
using System.Configuration;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class TimeoutConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        public TimeoutConfigCachingStrategyFactory(ICachingStrategyConfiguration config)
            : base(config)
        {
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>()
        {
            int timeout;
            if (!Int32.TryParse(CachingStrategyConfiguration["timeout"], out timeout))
            {

                throw new ConfigurationErrorsException("The timeout attribute is required in order to use the TimeoutCachingStrategy via the configuration file.");
            }

           return new TimeoutCachingStrategy<T, TKey>(timeout);
        }
    }
}
