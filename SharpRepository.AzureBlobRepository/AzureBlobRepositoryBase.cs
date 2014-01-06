using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.AzureBlobRepository
{
    public class AzureBlobRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T: class, new()
    {
        protected CloudBlobClient BlobClient { get; private set; }
        protected CloudBlobContainer BlobContainer { get; private set; }
        protected string ContainerName { get; private set; }
        protected bool CreateIfNotExists { get; private set; }

        internal AzureBlobRepositoryBase(string connectionString, string containerName, bool createIfNotExists,
            ICachingStrategy<T, TKey> cachingStrategy = null)
            : base(cachingStrategy)
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(connectionString);

            ContainerName = TypeName.ToLower();

            if (!String.IsNullOrEmpty(containerName))
            {
                ContainerName = containerName;
            }

            CreateIfNotExists = createIfNotExists;
            BlobClient = storageAccount.CreateCloudBlobClient();
            SetContainer(containerName);
        }

        protected void SetContainer(string containerName)
        {
            ContainerName = containerName;
            BlobContainer = BlobClient.GetContainerReference(containerName);

            if (CreateIfNotExists)
            {
                BlobContainer.CreateIfNotExists();
            }
        }

        protected override T GetQuery(TKey key)
        {
            var blob = BlobContainer.GetBlockBlobReference(key.ToString());

            return blob == null ? null : JsonConvert.DeserializeObject<T>(blob.DownloadText());
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            throw new NotImplementedException();
        }

        private CloudBlockBlob GetBlobReference(T entity)
        {
            TKey key;
            GetPrimaryKey(entity, out key);

            return BlobContainer.GetBlockBlobReference(key.ToString());
        }

        protected override void AddItem(T entity)
        {
            var blob = GetBlobReference(entity);
            blob.UploadText(JsonConvert.SerializeObject(entity));
        }

        protected override void DeleteItem(T entity)
        {
            var blob = GetBlobReference(entity);
            blob.DeleteIfExists();
        }

        protected override void UpdateItem(T entity)
        {
            var blob = GetBlobReference(entity);
            blob.UploadText(JsonConvert.SerializeObject(entity));
        }

        protected override void SaveChanges()
        {
            
        }

        public override void Dispose()
        {
            
        }
    }
}
