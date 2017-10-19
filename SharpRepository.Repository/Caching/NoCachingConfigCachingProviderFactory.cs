using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class NoCachingConfigCachingProviderFactory : ConfigCachingProviderFactory
    {
        public NoCachingConfigCachingProviderFactory(ICachingProviderConfiguration config)
            : base(config)
        {
        }

        public override ICachingProvider GetInstance()
        {
            return new NoCachingProvider();
        }
    }
}
