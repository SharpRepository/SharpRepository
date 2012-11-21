using System.Configuration;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public class TimeoutCachingStrategySection : ConfigurationSection, ICachingStrategyElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        // TODO: add validator
        [ConfigurationProperty("timeout", IsRequired = true)]
        public int Timeout
        {
            get { return (int)base["timeout"]; }
            set { base["timeout"] = value; }
        }

        [ConfigurationProperty("cachingProvider")]
        public string CachingProvider
        {
            get { return (string)base["cachingProvider"]; }
            set { base["cachingProvider"] = value; }
        }

        public ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class
        {
            return new TimeoutCachingStrategy<T, TKey>(Timeout);
        }
    }
}
