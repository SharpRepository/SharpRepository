using System;
using SharpRepository.Repository.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace SharpRepository.Repository.Caching
{
    public class StandardConfigCachingStrategyFactory : ConfigCachingStrategyFactory
    {
        protected IMemoryCache Cache;

        public StandardConfigCachingStrategyFactory(ICachingStrategyConfiguration config, IMemoryCache cache)
            : base(config)
        {
            Cache = cache;
        }

        public override ICachingStrategy<T, TKey> GetInstance<T, TKey>()
        {
            var strategy = new StandardCachingStrategy<T, TKey>(new InMemoryCachingProvider(Cache))
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

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            var strategy = new StandardCachingStrategy<T, TKey, TKey2>(new InMemoryCachingProvider(Cache))
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

        public override ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            var strategy = new StandardCachingStrategy<T, TKey, TKey2, TKey3>(new InMemoryCachingProvider(Cache))
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
        
        public override ICompoundKeyCachingStrategy<T> GetCompoundKeyInstance<T>()
        {
            var strategy = new StandardCompoundKeyCachingStrategy<T>(new InMemoryCachingProvider(Cache))
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
