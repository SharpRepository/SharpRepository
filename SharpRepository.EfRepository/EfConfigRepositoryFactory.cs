using System;
using System.Configuration;
using System.Data.Entity;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.EfRepository
{
    public class EfConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public EfConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["connectionString"]))
            {
                throw new ConfigurationErrorsException("The connectionString attribute is required in order to use the EfRepository via the configuration file.");
            }

            Type dbContextType = null;

            if (!String.IsNullOrEmpty(RepositoryConfiguration["dbContextType"]))
            {
                dbContextType = Type.GetType(RepositoryConfiguration["dbContextType"]);
            }

            var connectionString = RepositoryConfiguration["connectionString"];

            // TODO: look at dbContextType (from Enyim.Caching configuration bits) and how it caches, see about implementing cache or expanding FastActivator to take parameters
            var dbContext = dbContextType == null ?
                new DbContext(connectionString) :
                (DbContext)Activator.CreateInstance(dbContextType, connectionString);

            return new EfRepository<T, TKey>(dbContext);
        }
    }
}
