using System;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.AzureDocumentDb
{
    public class DocumentDbConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public DocumentDbConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            throw new NotImplementedException("DocumentDbRepository does not support using IRepository<T> directly to reference a IRepository<T, string>");
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            if (typeof(TKey) != typeof(string))
                throw new NotImplementedException(string.Format("DocumentDbRepository does not support using {0} as a Key. {1} only supported as a Key.", typeof(TKey), typeof(string)));

            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["endpointUrl"])
                && String.IsNullOrEmpty(RepositoryConfiguration["authorizationKey"])
                && String.IsNullOrEmpty(RepositoryConfiguration["databaseId"]))
            {
                throw new ConfigurationErrorsException("The endpointUrl, authorizationKey, databaseId attribute is required in order to use the MongoDbRepository via the configuration file.");
            }

            var createIfNotExists = false;
            Boolean.TryParse(RepositoryConfiguration["createIfNotExists"], out createIfNotExists);

            return new DocumentDbRepository<T, TKey>(RepositoryConfiguration["endpointUrl"], RepositoryConfiguration["authorizationKey"], RepositoryConfiguration["databaseId"], createIfNotExists);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            throw new NotImplementedException();
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            throw new NotImplementedException();
        }

        public override ICompoundKeyRepository<T> GetCompoundKeyInstance<T>()
        {
            throw new NotImplementedException();
        }
    }
}