using Microsoft.Azure.Documents;
using SharpRepository.Repository.Caching;

namespace SharpRepository.AzureDocumentDb
{
    public class DocumentDbRepository<T, TKey> : DocumentDbRepositoryBase<T, TKey> where T : class, new()
    {
        public DocumentDbRepository(string endpointUrl, string authorizationKey, string databaseId, bool createIfNotExists, string collectionId = null, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(endpointUrl, authorizationKey, databaseId, createIfNotExists, collectionId, cachingStrategy)
        {
        }
    }
}