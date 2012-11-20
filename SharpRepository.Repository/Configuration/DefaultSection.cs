using System.Configuration;

namespace SharpRepository.Repository.Configuration
{
    public class DefaultSection : ConfigurationSection
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string) base["name"]; }
            set { base["name"] = value; }
        }
    }
}
