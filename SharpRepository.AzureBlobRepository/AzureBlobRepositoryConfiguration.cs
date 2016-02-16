using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.AzureBlobRepository
{
    public class AzureBlobRepositoryConfiguration : RepositoryConfiguration
    {
        public AzureBlobRepositoryConfiguration(string name) : this(name, null, null)
        {
        }

        public AzureBlobRepositoryConfiguration(string name, string connectionString)
            : this(name, connectionString, null)
        {
        }

        public AzureBlobRepositoryConfiguration(string name, string connectionString, string container, bool createIfNotExists = false, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            ConnectionString = connectionString;
            Container = container;
            CreateIfNotExists = createIfNotExists;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(AzureBlobConfigRepositoryFactory);
        }

        public string ConnectionString
        {
            set { Attributes["connectionString"] = value; }
        }

        public string Container
        {
            set { Attributes["container"] = String.IsNullOrEmpty(value) ? null : value; }
        }

        public bool CreateIfNotExists
        {
            set { Attributes["createIfNotExists"] = value.ToString(); }
        }
    }
}
