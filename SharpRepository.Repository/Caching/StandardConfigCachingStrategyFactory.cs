using SharpRepository.Repository.Configuration;
using System;

namespace SharpRepository.Repository.Caching
{
    public class StandardConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        public StandardConfigCachingStrategyFactory(ICachingStrategyConfiguration config)
           : base(config)
        {
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>(ICachingProvider cachingProvider)
        {
            var strategy = new StandardCachingStrategy<T, TKey>(cachingProvider)
                               {
                                   MaxResults = CachingStrategyConfiguration.MaxResults
                               };

            if (Boolean.TryParse(CachingStrategyConfiguration["generational"], out bool enabled))
            {
                strategy.GenerationalCachingEnabled = enabled;
            }

            if (Boolean.TryParse(CachingStrategyConfiguration["writeThrough"], out enabled))
            {
                strategy.WriteThroughCachingEnabled = enabled;
            }

            return strategy;
        }

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(ICachingProvider cachingProvider)
        {
            var strategy = new StandardCachingStrategy<T, TKey, TKey2>(cachingProvider)
                               {
                                   MaxResults = CachingStrategyConfiguration.MaxResults
                               };

            if (Boolean.TryParse(CachingStrategyConfiguration["generational"], out bool enabled))
            {
                strategy.GenerationalCachingEnabled = enabled;
            }

            if (Boolean.TryParse(CachingStrategyConfiguration["writeThrough"], out enabled))
            {
                strategy.WriteThroughCachingEnabled = enabled;
            }

            return strategy;
        }

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(ICachingProvider cachingProvider)
        {
            var strategy = new StandardCachingStrategy<T, TKey, TKey2, TKey3>(cachingProvider)
            {
                MaxResults = CachingStrategyConfiguration.MaxResults
            };

            if (Boolean.TryParse(CachingStrategyConfiguration["generational"], out bool enabled))
            {
                strategy.GenerationalCachingEnabled = enabled;
            }

            if (Boolean.TryParse(CachingStrategyConfiguration["writeThrough"], out enabled))
            {
                strategy.WriteThroughCachingEnabled = enabled;
            }

            return strategy;
        }
        
        public override ICompoundKeyCachingStrategy<T> GetCompoundKeyInstance<T>(ICachingProvider cachingProvider)
        {
            var strategy = new StandardCompoundKeyCachingStrategy<T>(cachingProvider)
            {
                MaxResults = CachingStrategyConfiguration.MaxResults
            };

            if (Boolean.TryParse(CachingStrategyConfiguration["generational"], out bool enabled))
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
