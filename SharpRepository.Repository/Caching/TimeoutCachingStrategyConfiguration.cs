using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class TimeoutCachingStrategyConfiguration : CachingStrategyConfiguration
    {
        public TimeoutCachingStrategyConfiguration(string name, int timeoutInSeconds)
        {
            Name = name;
            Timeout = timeoutInSeconds;
            Factory = typeof (TimeoutConfigCachingStrategyFactory);
        }

        public int Timeout
        {
            set { Attributes["timeout"] = value.ToString(); }
        }
    }
}
