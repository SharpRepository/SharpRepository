using Microsoft.WindowsAzure.Storage.Table;
using SharpRepository.Repository.Caching;

namespace SharpRepository.AzureTableRepository
{
    public class AzureTableRepository<T> : AzureTableRepositoryBase<T> where T : class, ITableEntity, new()
    {
        public AzureTableRepository(string connectionString, string tableName = null, bool createIfNotExists = true, ICompoundKeyCachingStrategy<T, string, string> cachingStrategy = null)
            : base(connectionString, tableName, createIfNotExists, cachingStrategy)
        {
        }
    }
}
