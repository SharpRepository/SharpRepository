using System;
using System.Configuration;
using System.Data.Entity;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ef5Repository
{
    public class Ef5ConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public Ef5ConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // check for required parameters
            if (RepositoryDependencyResolver.Current == null && String.IsNullOrEmpty(RepositoryConfiguration["connectionString"]))
            {
                throw new ConfigurationErrorsException("The connectionString attribute is required in order to use the Ef5Repository via the configuration file, unless you set the RepositoryDependencyResolver to use an Ioc container.");
            }

            Type dbContextType = null;

            if (!String.IsNullOrEmpty(RepositoryConfiguration["dbContextType"]))
            {
                dbContextType = Type.GetType(RepositoryConfiguration["dbContextType"]);
            }

            var connectionString = RepositoryConfiguration["connectionString"];

            // TODO: look at dbContextType (from Enyim.Caching configuration bits) and how it caches, see about implementing cache or expanding FastActivator to take parameters
            DbContext dbContext = null;

            // if there is an IOC dependency resolver configured then use that one to get the DbContext, this will allow sharing of context across multiple repositories if the IOC is configured that way
            if (RepositoryDependencyResolver.Current != null)
            {
                dbContext = dbContextType == null
                                ? RepositoryDependencyResolver.Current.Resolve<DbContext>()
                                : (DbContext)RepositoryDependencyResolver.Current.Resolve(dbContextType);

                // if the Ioc container doesn't throw an error but still returns null we need to alert the consumer
                if (dbContext == null)
                {
                    throw new RepositoryDependencyResolverException(typeof(DbContext));
                }
            }

            // the default way of getting a DbContext if there is no Ioc container setup
            dbContext = dbContextType == null
                            ? new DbContext(connectionString)
                            : (DbContext) Activator.CreateInstance(dbContextType, connectionString);

            return new Ef5Repository<T, TKey>(dbContext);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            // check for required parameters
            if (String.IsNullOrEmpty(RepositoryConfiguration["connectionString"]))
            {
                throw new ConfigurationErrorsException("The connectionString attribute is required in order to use the Ef5Repository via the configuration file.");
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

            return new Ef5Repository<T, TKey, TKey2>(dbContext);
        }
    }
}
