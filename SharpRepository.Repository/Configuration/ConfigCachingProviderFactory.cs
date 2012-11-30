using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface IConfigCachingProviderFactory
    {
        ICachingProvider GetInstance();
    }

    public abstract class ConfigCachingProviderFactory : IConfigCachingProviderFactory
    {
        protected ICachingProviderConfiguration CachingProviderConfiguration;

        protected ConfigCachingProviderFactory(ICachingProviderConfiguration config)
        {
            CachingProviderConfiguration = config;
        }

        public abstract ICachingProvider GetInstance();
    }
}
