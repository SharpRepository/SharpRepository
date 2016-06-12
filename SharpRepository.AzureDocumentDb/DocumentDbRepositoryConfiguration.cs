using SharpRepository.Repository.Configuration;

namespace SharpRepository.AzureDocumentDb
{
    public class DocumentDbRepositoryConfiguration : RepositoryConfiguration
    {
        public DocumentDbRepositoryConfiguration(string endpointUrl, string authorizationKey, string databaseId, string collectionId = null, bool createIfNotExists = false, string cachingStrategy = null, string cachingProvider = null)
        {
            EndpointUrl = endpointUrl;
            AuthorizationKey = authorizationKey;
            DatabaseId = databaseId;
            CollectionId = collectionId;
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

        public string CollectionId
        {
            set { Attributes["collectionId"] = value; }
        }

        public bool CreateIfNotExists
        {
            set { Attributes["createIfNotExists"] = value.ToString(); }
        }

    }
}