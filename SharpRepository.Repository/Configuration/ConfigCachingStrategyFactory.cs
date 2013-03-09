using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface IConfigCachingStrategyFactory
    {
        ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class;
        ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class;
    }

    public abstract class ConfigCachingStrategyFactory : IConfigCachingStrategyFactory
    {
        protected ICachingStrategyConfiguration CachingStrategyConfiguration;

        protected ConfigCachingStrategyFactory(ICachingStrategyConfiguration config)
        {
            CachingStrategyConfiguration = config;
        }

        public abstract ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class;
        public abstract ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class;
    }
}
