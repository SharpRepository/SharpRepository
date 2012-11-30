using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class NoCachingStrategyConfiguration : CachingStrategyConfiguration
    {
        public NoCachingStrategyConfiguration(string name)
        {
            Name = name;
            Factory = typeof (NoCachingConfigCachingStrategyFactory);
        }
    }
}
