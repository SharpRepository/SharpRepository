using System;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.AzureBlobRepository
{
    public class AzureBlobConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public AzureBlobConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            throw new NotImplementedException("AzureBlobRepository does not support using IRepository<T> directly to reference a IRepository<T, string>");           
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new AzureBlobRepository<T, TKey>(RepositoryConfiguration["connectionString"], RepositoryConfiguration["container"]);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            throw new NotImplementedException();
        }
    }
}
