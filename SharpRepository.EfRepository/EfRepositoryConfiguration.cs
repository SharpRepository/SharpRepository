using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.EfRepository
{
    public class EfRepositoryConfiguration : RepositoryConfiguration
    {
        public EfRepositoryConfiguration(string name) : this(name, null, null)
        {
        }

        public EfRepositoryConfiguration(string name, string connectionStringOrName)
            : this(name, connectionStringOrName, null)
        {
        }

        public EfRepositoryConfiguration(string name, string connectionStringOrName, Type dbContextType, string cachingStrategy=null, string cachingProvider=null)
            : base(name)
        {
            ConnectionStringOrName = connectionStringOrName;
            DbContextType = dbContextType;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(EfConfigRepositoryFactory);
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

                Attributes["dbContextType"] = value.AssemblyQualifiedName;
            }
        }
    }
}
