using System.Configuration;
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
    public class InMemoryCachingProviderSection : ConfigurationSection, ICachingProviderElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        public ICachingProvider GetInstance()
        {
            return new InMemoryCachingProvider();
        }
    }
}
