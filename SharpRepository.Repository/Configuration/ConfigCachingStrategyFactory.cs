using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface IConfigCachingStrategyFactory
    {
        ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class;
    }

    public abstract class ConfigCachingStrategyFactory : IConfigCachingStrategyFactory
    {
        protected CachingStrategyElement CachingStrategyElement;

        protected ConfigCachingStrategyFactory(CachingStrategyElement element)
        {
            CachingStrategyElement = element;
        }

        public abstract ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class;
    }
}
