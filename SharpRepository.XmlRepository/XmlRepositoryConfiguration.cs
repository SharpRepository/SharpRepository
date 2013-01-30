using SharpRepository.Repository.Configuration;

namespace SharpRepository.XmlRepository
{
    public class XmlRepositoryConfiguration : RepositoryConfiguration
    {
        public XmlRepositoryConfiguration(string name, string directory)
        {
            Name = name;
            Directory = directory;
            Factory = typeof (XmlConfigRepositoryFactory);
        }

        public string Directory
        {
            set { Attributes["directory"] = value; }
        }
    }
}
