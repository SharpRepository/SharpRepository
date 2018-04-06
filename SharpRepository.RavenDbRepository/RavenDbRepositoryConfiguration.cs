using SharpRepository.Repository.Configuration;

namespace SharpRepository.RavenDbRepository
{
    public class RavenDbRepositoryConfiguration : RepositoryConfiguration
    {
        public RavenDbRepositoryConfiguration(string name)
        {
            Name = name;
            Factory = typeof (RavenDbConfigRepositoryFactory);
        }

        public string Url
        {
            set { Attributes["url"] = value; }
        }
    }
}
