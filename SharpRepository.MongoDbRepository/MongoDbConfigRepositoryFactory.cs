using System;
using System.Security.Authentication;
using MongoDB.Driver;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public MongoDbConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            throw new NotImplementedException("MongoDbRepository does not support using IRepository<T> directly to reference a IRepository<T, string>");
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["connectionString"]))
            {
                throw new ConfigurationErrorsException("The connectionString attribute is required in order to use the MongoDbRepository via the configuration file.");
            }

            SslSettings sslSettings = null;
            if (!String.IsNullOrEmpty(RepositoryConfiguration["sslEnabled"]) && Boolean.Parse(RepositoryConfiguration["sslEnabled"]))
                sslSettings = new SslSettings() { EnabledSslProtocols = (SslProtocols)Enum.Parse(typeof(SslProtocols), RepositoryConfiguration["sslProtocol"]) };

            return new MongoDbRepository<T, TKey>(RepositoryConfiguration["connectionString"], sslSettings);
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
