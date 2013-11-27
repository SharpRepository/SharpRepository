using System;
using System.Linq;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Table;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.AzureTableRepository
{
    public class AzureTableRepositoryBase<T> : LinqCompoundKeyRepositoryBase<T, string, string> where T : class, ITableEntity, new()
    {
        protected CloudTableClient TableClient { get; private set; }
        protected CloudTable Table { get; private set; }
        protected string TableName { get; private set; }

        internal AzureTableRepositoryBase(string connectionString, string tableName, bool createIfNotExists, ICompoundKeyCachingStrategy<T, string, string> cachingStrategy = null)
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
                Table.CreateIfNotExists();
            }
        }

        protected override T GetQuery(string key, string key2)
        {
            var result = Table.Execute(TableOperation.Retrieve<T>(key, key2));
           return result.Result as T;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            throw new NotImplementedException();
        }

        protected override void AddItem(T entity)
        {
            Table.Execute(TableOperation.Insert(entity));
        }

        // TODO: override Add(IEnumerable<T> entities) to use the TableSet.Add(entities) isntead of looping ourselves and having AddItem() called multiple times

        protected override void DeleteItem(T entity)
        {
            Table.Execute(TableOperation.Delete(entity));
        }

        protected override void UpdateItem(T entity)
        {
            Table.Execute(TableOperation.Replace(entity));
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
