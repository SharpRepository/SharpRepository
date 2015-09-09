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
            var strategy = new StandardCachingStrategy<T, TKey>()
                               {
                                   MaxResults = CachingStrategyConfiguration.MaxResults
                               };

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

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            var strategy = new StandardCachingStrategy<T, TKey, TKey2>()
                               {
                                   MaxResults = CachingStrategyConfiguration.MaxResults
                               };

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

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            var strategy = new StandardCachingStrategy<T, TKey, TKey2, TKey3>()
            {
                MaxResults = CachingStrategyConfiguration.MaxResults
            };

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
