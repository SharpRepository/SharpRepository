using SharpRepository.Repository.Caching;

namespace SharpRepository.AzureBlobRepository
{
    public class AzureBlobRepository<T, TKey> : AzureBlobRepositoryBase<T, TKey> where T: class, new()
    {
        public AzureBlobRepository(string connectionString, string containerName = null, bool createIfNotExists = true, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(connectionString, containerName, createIfNotExists, cachingStrategy)
        {
            
        }
    }

    public class AzureBlobRepository<T> : AzureBlobRepositoryBase<T, string> where T : class, new()
    {
        public AzureBlobRepository(string connectionString, string containerName = null, bool createIfNotExists = true, ICachingStrategy<T, string> cachingStrategy = null)
            : base(connectionString, containerName, createIfNotExists, cachingStrategy)
        {

        }
    }
}
