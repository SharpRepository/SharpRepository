using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WindowsAzure.Table;
using WindowsAzure.Table.Attributes;
using Microsoft.WindowsAzure.Storage.Table;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.AzureTableRepository
{
    public class AzureTableRepositoryBase<T> : LinqCompoundKeyRepositoryBase<T, string, string> where T : class, new()
    {
        protected CloudTableClient TableClient { get; private set; }
        protected TableSet<T> TableSet { get; private set; }

        internal AzureTableRepositoryBase(string connectionString, bool createIfNotExists, ICompoundKeyCachingStrategy<T, string, string> cachingStrategy = null)
            : base(cachingStrategy)
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);

            TableClient = storageAccount.CreateCloudTableClient();
            TableSet = new TableSet<T>(TableClient);

            if (createIfNotExists)
            {
                var tableName = typeof(T).Name;
                var tableRef = TableClient.GetTableReference(tableName);

                tableRef.CreateIfNotExists();
            }
        }

        protected override T GetQuery(string key, string key2)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var lambda = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, "PartitionKey"),
                        Expression.Constant(key)
                    ),
                    parameter
                );
            var lambda2 = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, "RowKey"),
                        Expression.Constant(key2)
                    ),
                    parameter
                );

            var entity = FindQuery(new Specification<T>(lambda).AndAlso(lambda2));

            if (entity != null)
            {
                // we need to set the actual key values
                SetPrimaryKey(entity, key, key2);
            }

            return entity;
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return TableSet;
        }

        protected override void AddItem(T entity)
        {
            TableSet.Add(entity);
        }

        // TODO: override Add(IEnumerable<T> entities) to use the TableSet.Add(entities) isntead of looping ourselves and having AddItem() called multiple times

        protected override void DeleteItem(T entity)
        {
            TableSet.Remove(entity);
        }

        protected override void UpdateItem(T entity)
        {
            TableSet.Update(entity);
        }

        protected override void SaveChanges()
        {
            
        }

        protected override PropertyInfo[] GetPrimaryKeyPropertyInfo()
        {
            var type = typeof(T);

            var partitionKey = type.GetProperty("PartitionKey") ??
                               type.GetProperties().FirstOrDefault(x => x.HasAttribute<PartitionKeyAttribute>() && x.PropertyType == typeof(string));

            var rowKey = type.GetProperty("RowKey") ??
                                type.GetProperties().FirstOrDefault(x => x.HasAttribute<RowKeyAttribute>() && x.PropertyType == typeof(string));

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
