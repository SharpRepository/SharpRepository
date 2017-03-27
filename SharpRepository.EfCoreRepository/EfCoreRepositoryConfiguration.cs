using SharpRepository.Repository.Configuration;
using System;

namespace SharpRepository.EfCoreRepository
{
    public class EfCoreRepositoryConfiguration : RepositoryConfiguration
    {
        public EfCoreRepositoryConfiguration(string name) : this(name, null, null)
        {
        }

        public EfCoreRepositoryConfiguration(string name, string connectionStringOrName)
            : this(name, connectionStringOrName, null)
        {
        }

        public EfCoreRepositoryConfiguration(string name, string connectionStringOrName, Type dbContextType, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            ConnectionStringOrName = connectionStringOrName;
            DbContextType = dbContextType;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(EfCoreConfigRepositoryFactory);
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