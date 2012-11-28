using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class TimeoutConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        public TimeoutConfigCachingStrategyFactory(CachingStrategyElement element)
            : base(element)
        {
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>()
        {
            int timeout;
            if (!Int32.TryParse(CachingStrategyElement["timeout"], out timeout))
            {
                throw new ArgumentException("timeout");
            }

           return new TimeoutCachingStrategy<T, TKey>(timeout);
        }
    }
}
