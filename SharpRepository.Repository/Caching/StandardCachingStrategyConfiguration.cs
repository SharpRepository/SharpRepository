using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class StandardCachingStrategyConfiguration : CachingStrategyConfiguration
    {
        public StandardCachingStrategyConfiguration(string name) : this(name, true, true)
        {
        }

        public StandardCachingStrategyConfiguration(string name, bool writeThroughCachingEnabled, bool generationalCachingEnabled)
        {
            Name = name;
            WriteThroughCachingEnabled = writeThroughCachingEnabled;
            GeneraltionalCachingEnabled = generationalCachingEnabled;
            Factory = typeof (StandardConfigCachingStrategyFactory);
        }

        public bool WriteThroughCachingEnabled
        {
            set { Attributes["writeThrough"] = value.ToString(); }
        }

        public bool GeneraltionalCachingEnabled
        {
            set { Attributes["generational"] = value.ToString(); }
        }
    }
}
