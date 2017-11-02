using System;
using System.Linq;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Table;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.AzureTableRepository
{
    public class AzureTableRepositoryBase<T,TKey,TKey2> : LinqCompoundKeyRepositoryBase<T, TKey, TKey2> where T : class, new()
    {
        protected CloudTableClient TableClient { get; private set; }
        protected CloudTable Table { get; private set; }
        protected string TableName { get; private set; }

        internal AzureTableRepositoryBase(string connectionString, string tableName, bool createIfNotExists, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
            : base(cachingStrategy)
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);

            TableName = TypeName;

            if (!String.IsNullOrEmpty(tableName))
            {
                TableName = tableName;
            }

            TableClient = storageAccount.CreateCloudTableClient();
            Table = TableClient.GetTableReference(TableName);

            if (createIfNotExists)
            {
                Table.CreateIfNotExistsAsync();
            }
        }

        protected override T GetQuery(TKey key, TKey2 key2)
        {
            //TODO: Evaluate if this query without generic type works
            var operation = TableOperation.Retrieve(key.ToString(), key2.ToString());

            var result = Table.ExecuteAsync(operation).Result;
            return result.Result as T;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            throw new NotImplementedException();
        }

        protected override void AddItem(T entity)
        {
            Table.ExecuteAsync(TableOperation.InsertOrReplace((ITableEntity)entity));
        }

        // TODO: override Add(IEnumerable<T> entities) to use the TableSet.Add(entities) isntead of looping ourselves and having AddItem() called multiple times

        protected override void DeleteItem(T entity)
        {
            Table.ExecuteAsync(TableOperation.Delete((ITableEntity)entity));
        }

        protected override void UpdateItem(T entity)
        {
            Table.ExecuteAsync(TableOperation.Replace((ITableEntity)entity));
        }

        protected override void SaveChanges()
        {
        }

        protected override PropertyInfo[] GetPrimaryKeyPropertyInfo()
        {
            var type = typeof(T);

            var partitionKey = type.GetProperty("PartitionKey") ;
            var rowKey = type.GetProperty("RowKey");

            if (partitionKey != null && rowKey != null)
            {
                return new [] { partitionKey, rowKey};
            }

            return base.GetPrimaryKeyPropertyInfo();
        }

        public override void Dispose()
        {
        }
    }
}
