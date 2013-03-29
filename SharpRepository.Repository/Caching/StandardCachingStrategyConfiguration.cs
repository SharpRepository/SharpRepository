using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository.Caching
{
    public class StandardCachingStrategyConfiguration : CachingStrategyConfiguration
    {
        public StandardCachingStrategyConfiguration(string name) : this(name, true, true)
        {
        }

        public StandardCachingStrategyConfiguration(string name, bool writeThroughCachingEnabled, bool generationalCachingEnabled)
            : this(name, writeThroughCachingEnabled, generationalCachingEnabled, null)
        {
        }

        public StandardCachingStrategyConfiguration(string name, bool writeThroughCachingEnabled, bool generationalCachingEnabled, int? maxResults)
        {
            Name = name;
            WriteThroughCachingEnabled = writeThroughCachingEnabled;
            GeneraltionalCachingEnabled = generationalCachingEnabled;
            MaxResults = maxResults;
            Factory = typeof(StandardConfigCachingStrategyFactory);
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
