using System;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using Microsoft.WindowsAzure.Storage.Table;

namespace SharpRepository.AzureTableRepository
{
    public class AzureTableConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public AzureTableConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            throw new NotImplementedException();
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            throw new NotImplementedException("azure table only implements ICompoundKeyRepository<T, string, string> using GetAzureTableInstanc");
        }

        public override ICompoundKeyRepository<T> GetCompoundKeyInstance<T>()
        {
            throw new NotImplementedException("azure table only implements ICompoundKeyRepository<T, string, string> using GetAzureTableInstance");
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            throw new NotImplementedException("azure table only implements ICompoundKeyRepository<T, TKey, TKey2>");
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            Boolean.TryParse(RepositoryConfiguration["createIfNotExists"], out bool createIfNotExists);

            return new AzureTableRepository<T, TKey, TKey2>(RepositoryConfiguration["connectionString"], RepositoryConfiguration["tableName"], createIfNotExists);
        }
    }
}
