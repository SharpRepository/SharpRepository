using System;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.AzureTableRepository
{
    public class AzureTableRepositoryConfiguration : RepositoryConfiguration
    {
        public AzureTableRepositoryConfiguration(string name) : this(name, null, null)
        {
        }

        public AzureTableRepositoryConfiguration(string name, string connectionString)
            : this(name, connectionString, null)
        {
        }

        public AzureTableRepositoryConfiguration(string name, string connectionString, string tableName, bool createIfNotExists = false, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            ConnectionString = connectionString;
            TableName = tableName;
            CreateIfNotExists = createIfNotExists;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(AzureTableConfigRepositoryFactory);
        }

        public string ConnectionString
        {
            set { Attributes["connectionString"] = value; }
        }

        public string TableName
        {
            set { Attributes["tableName"] = String.IsNullOrEmpty(value) ? null : value; }
        }

        public bool CreateIfNotExists
        {
            set { Attributes["createIfNotExists"] = value.ToString(); }
        }
    }
}
