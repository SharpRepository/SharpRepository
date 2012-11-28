using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public interface IConfigCachingProviderFactory
    {
        ICachingProvider GetInstance();
    }

    public abstract class ConfigCachingProviderFactory : IConfigCachingProviderFactory
    {
        protected CachingProviderElement CachingProviderElement;

        protected ConfigCachingProviderFactory(CachingProviderElement element)
        {
            CachingProviderElement = element;
        }

        public abstract ICachingProvider GetInstance();
    }
}
