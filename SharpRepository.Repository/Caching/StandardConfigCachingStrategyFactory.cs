using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class StandardConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        public StandardConfigCachingStrategyFactory(CachingStrategyElement element) 
            : base(element)
        {
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>()
        {
            var strategy = new StandardCachingStrategy<T, TKey>();

            bool enabled;
            if (Boolean.TryParse(CachingStrategyElement["generational"], out enabled))
            {
                strategy.GenerationalCachingEnabled = enabled;
            }

            if (Boolean.TryParse(CachingStrategyElement["writeThrough"], out enabled))
            {
                strategy.WriteThroughCachingEnabled = enabled;
            }

            return strategy;
        }
    }
}
