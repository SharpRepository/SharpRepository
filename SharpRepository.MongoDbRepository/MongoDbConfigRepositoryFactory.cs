using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["connectionString"]))
            {
                throw new ConfigurationErrorsException("The connectionString attribute is required in order to use the MongoDbRepository via the configuration file.");
            }

            return new MongoDbRepository<T, TKey>(RepositoryConfiguration["connectionString"]);
        }
    }
}
