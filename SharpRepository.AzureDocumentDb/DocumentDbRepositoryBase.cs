using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Nito.AsyncEx.Synchronous;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;
using SharpRepository.Repository.Helpers;
using SharpRepository.Repository.Specifications;

namespace SharpRepository.AzureDocumentDb
{
    public class DocumentDbRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        protected DocumentClient Client { get; set; }
        protected Database Database { get; set; }
        protected DocumentCollection BaseCollection { get; set; }
        protected string CollectionId { get; set; }

        internal DocumentDbRepositoryBase(string endpointUrl, string authorizationKey, string databaseId, bool createIfNotExists, string collectionId = null, ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            Client = new DocumentClient(new Uri(endpointUrl), authorizationKey);

            Initilize(databaseId, createIfNotExists, collectionId);
        }

        private void Initilize(string databaseId, bool createIfNotExists, string collectionId = null)
        {
            Database = Client.CreateDatabaseQuery().Where(db => db.Id == databaseId).AsEnumerable().FirstOrDefault();

            if (Database == null && createIfNotExists)
                Database = DocumentDbRepositoryManager.CreateDatabase(Client, databaseId).WaitAndUnwrapException();

            if (Database == null)
                throw new Exception(string.Format("No database {0} existed. Use createIfNotExists = true in order to create database.", databaseId));

            if (string.IsNullOrWhiteSpace(collectionId))
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                CollectionId = rgx.Replace(typeof(T).FullName, "");
            }
            else
                CollectionId = collectionId;

            BaseCollection =
                Client.CreateDocumentCollectionQuery(Database.SelfLink)
                    .Where(c => c.Id == CollectionId)
                    .ToArray()
                    .FirstOrDefault() ??
                DocumentDbRepositoryManager.CreateCollection(Client, Database, CollectionId).WaitAndUnwrapException();
        }

        protected override Specification<T> CreateSpecification(Expression<Func<T, bool>> lambda)
        {
            return new DocumentDbSpecification<T>(lambda);
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return Client.CreateDocumentQuery<T>(BaseCollection.DocumentsLink).AsQueryable();
        }

        protected virtual string GetSelfLink(TKey id)
        {
            var propInfo = GetPrimaryKeyPropertyInfo();

            var parameter = Expression.Parameter(typeof(T), "x");
            var lambda = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.PropertyOrField(parameter, propInfo.Name),
                        Expression.Constant(id)
                    ),
                    parameter
                );

            var query = Client.CreateDocumentQuery<Document>(BaseCollection.DocumentsLink, string.Format("SELECT * FROM {0} e WHERE e.{1}", CollectionId, lambda.ToMSSqlString()));

            var doc = query.AsEnumerable().FirstOrDefault();

            if (doc != null)
                return doc.SelfLink;

            return null;
        }

        protected override void AddItem(T entity)
        {
            Client.CreateDocumentAsync(BaseCollection.DocumentsLink, entity).WaitAndUnwrapException();
        }

        protected override void DeleteItem(T entity)
        {
            TKey id;
            if (!GetPrimaryKey(entity, out id))
                return;

            var selfLink = GetSelfLink(id);

            Client.DeleteDocumentAsync(selfLink).WaitAndUnwrapException();
        }

        protected override void UpdateItem(T entity)
        {
            TKey id;
            if (!GetPrimaryKey(entity, out id))
                return;

            var selfLink = GetSelfLink(id);

            Client.ReplaceDocumentAsync(selfLink, entity).WaitAndUnwrapException();
        }

        protected override PropertyInfo GetPrimaryKeyPropertyInfo()
        {
            // checks for the MongoDb BsonIdAttribute and if not there no the normal checks
            var type = typeof(T);

            return type.GetProperties().FirstOrDefault(x => x.HasAttribute<JsonPropertyAttribute>() && x.GetCustomAttribute<JsonPropertyAttribute>().PropertyName.ToLower() == "id" && x.PropertyType == typeof(string))
                ?? base.GetPrimaryKeyPropertyInfo();
        }

        protected override void SaveChanges()
        {
        }

        public override void Dispose()
        {
            Client.Dispose();
        }
    }
}