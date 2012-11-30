using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ef5Repository
{
    public class Ef5RepositoryConfiguration : RepositoryConfiguration
    {
        public Ef5RepositoryConfiguration(string name) : this(name, null, null)
        {
        }

        public Ef5RepositoryConfiguration(string name, string connectionStringOrName)
            : this(name, connectionStringOrName, null)
        {
        }

        public Ef5RepositoryConfiguration(string name, string connectionStringOrName, Type dbContextType, string cachingStrategy=null, string cachingProvider=null)
        {
            Name = name;
            ConnectionStringOrName = connectionStringOrName;
            DbContextType = dbContextType;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(Ef5ConfigRepositoryFactory);
        }

        public string ConnectionStringOrName
        {
            set { Attributes["connectionString"] = value; }
        }

        public Type DbContextType
        {
            set
            {
                if (value == null)
                    return;

                Attributes["dbContextType"] = value.FullName;
            }
        }
    }
}
