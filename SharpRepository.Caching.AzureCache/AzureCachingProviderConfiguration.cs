using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.AzureCache
{
    public class AzureCachingProviderConfiguration : CachingProviderConfiguration
    {
        public AzureCachingProviderConfiguration(string name) : this(name, null)
        {
        }

        public AzureCachingProviderConfiguration(string name, string cacheName)
        {
            Name = name;
            CacheName = cacheName;
            Factory = typeof(AzureConfigCachingProviderFactory);
        }

        public string CacheName
        {
            set { Attributes["cacheName"] = value; }
        }
    }
}
