using System;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.XmlRepository
{
    public class XmlConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public XmlConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["directory"]))
            {
                throw new ConfigurationErrorsException("The directory attribute is required in order to use the XmlRepository via the configuration file.");
            }

            return new XmlRepository<T>(RepositoryConfiguration["directory"]);
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["directory"]))
            {
                throw new ConfigurationErrorsException("The directory attribute is required in order to use the XmlRepository via the configuration file.");
            }

            return new XmlRepository<T, TKey>(RepositoryConfiguration["directory"]);
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
