using SharpRepository.Repository.Configuration;

namespace SharpRepository.Caching.WindowsAzure
{
    public class WindowsAzureCachingProviderConfiguration : CachingProviderConfiguration
    {
        public WindowsAzureCachingProviderConfiguration(string name, string sectionName)
        {
            Name = name;
            SectionName = sectionName;
            Factory = typeof (WindowsAzureConfigCachingProviderFactory);
        }

        public string SectionName
        {
            set { Attributes["cacheName"] = value; }
        }
    }
}
