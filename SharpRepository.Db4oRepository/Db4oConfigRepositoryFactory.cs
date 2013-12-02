using System;
using System.Configuration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Db4oRepository
{
    public class Db4oConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public Db4oConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            throw new NotImplementedException("Db4oRepository does not support using IRepository<T> directly to reference a IRepository<T, string>");
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["directory"]))
            {
                throw new ConfigurationErrorsException("The directory attribute is required in order to use the Db4oRepository via the configuration file.");
            }

            return new Db4oRepository<T, TKey>(RepositoryConfiguration["directory"]);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            throw new NotImplementedException();
        }
    }
}
