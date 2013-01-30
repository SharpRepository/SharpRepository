using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class StandardConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        public StandardConfigCachingStrategyFactory(ICachingStrategyConfiguration config)
            : base(config)
        {
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>()
        {
            var strategy = new StandardCachingStrategy<T, TKey>();

            bool enabled;
            if (Boolean.TryParse(CachingStrategyConfiguration["generational"], out enabled))
            {
                strategy.GenerationalCachingEnabled = enabled;
            }

            if (Boolean.TryParse(CachingStrategyConfiguration["writeThrough"], out enabled))
            {
                strategy.WriteThroughCachingEnabled = enabled;
            }

            return strategy;
        }
    }
}
