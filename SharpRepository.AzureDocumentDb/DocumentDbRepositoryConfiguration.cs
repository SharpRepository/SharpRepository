using SharpRepository.Repository.Configuration;

namespace SharpRepository.AzureDocumentDb
{
    public class DocumentDbRepositoryConfiguration : RepositoryConfiguration
    {
        public DocumentDbRepositoryConfiguration(string endpointUrl, string authorizationKey, string databaseId, bool createIfNotExists = false, string cachingStrategy = null, string cachingProvider = null)
        {
            EndpointUrl = endpointUrl;
            AuthorizationKey = authorizationKey;
            DatabaseId = databaseId;
            CreateIfNotExists = createIfNotExists;

            CachingProvider = cachingProvider;
            CachingStrategy = cachingStrategy;
            Factory = typeof(DocumentDbConfigRepositoryFactory);
        }

        public string EndpointUrl
        {
            set { Attributes["endpointUrl"] = value; }
        }

        public string AuthorizationKey
        {
            set { Attributes["authorizationKey"] = value; }
        }

        public string DatabaseId
        {
            set { Attributes["databaseId"] = value; }
        }

        public bool CreateIfNotExists
        {
            set { Attributes["createIfNotExists"] = value.ToString(); }
        }

    }
}