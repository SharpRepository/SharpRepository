using Microsoft.WindowsAzure.Storage.Table;
using SharpRepository.Repository.Caching;
using System;
using System.Linq;
using System.Reflection;

namespace SharpRepository.AzureTableRepository
{
    public class AzureTableRepository<T,TKey, TKey2> : AzureTableRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        public AzureTableRepository(string connectionString, string tableName = null, bool createIfNotExists = true, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(connectionString, tableName, createIfNotExists, cachingStrategy)
        {
            if (typeof(TKey) != typeof(string) && typeof(TKey2) != typeof(string))
            {
                throw new Exception("Type of keys TKey and TKey2 musts be string");
            }
            
            if (!typeof(T).GetTypeInfo().GetInterfaces().Contains(typeof(ITableEntity))) {
                throw new Exception("Type of T must implement ITableEntity");
            }
        }
    }
}
