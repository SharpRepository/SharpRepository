using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.Memcached
{
    public class MemCachedCachingProviderConfiguration : CachingProviderConfiguration
    {
        public MemCachedCachingProviderConfiguration(string name, string sectionName)
        {
            Name = name;
            SectionName = sectionName;
            Factory = typeof (MemCachedConfigCachingProviderFactory);
        }

        public string SectionName
        {
            set { Attributes["sectionName"] = value; }
        }
    }
}
