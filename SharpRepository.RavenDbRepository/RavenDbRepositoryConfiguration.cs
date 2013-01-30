using SharpRepository.Repository.Configuration;

namespace SharpRepository.RavenDbRepository
{
    public class RavenDbRepositoryConfiguration : RepositoryConfiguration
    {
        public RavenDbRepositoryConfiguration(string name) 
            : this(name, null)
        {
        }

        public RavenDbRepositoryConfiguration(string name, string connectionStringName)
        {
            Name = name;
            ConnectionStringName = connectionStringName;
            Factory = typeof (RavenDbConfigRepositoryFactory);
        }

        public string Url
        {
            set { Attributes["url"] = value; }
        }

        public string ConnectionStringName
        {
            set { Attributes["connectionStringName"] = value; }
        }
    }
}
