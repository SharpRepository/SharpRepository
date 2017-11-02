using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.AppFabric
{
    public class AppFabricCachingProviderConfiguration : CachingProviderConfiguration
    {
        public AppFabricCachingProviderConfiguration(string name, string sectionName)
        {
            Name = name;
            SectionName = sectionName;
            Factory = typeof (AppFabricConfigCachingProviderFactory);
        }

        public string SectionName
        {
            set { Attributes["cacheName"] = value; }
        }
    }
}
