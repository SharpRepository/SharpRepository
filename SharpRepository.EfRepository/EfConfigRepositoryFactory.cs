using System;
using System.Configuration;
using System.Data.Entity;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;
using ConfigurationErrorsException = SharpRepository.Repository.Configuration.ConfigurationErrorsException;

namespace SharpRepository.EfRepository
{
    public class EfConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public EfConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            return new EfRepository<T>(GetDbContext());
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new EfRepository<T, TKey>(GetDbContext());
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            return new EfRepository<T, TKey, TKey2>(GetDbContext());
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            return new EfRepository<T, TKey, TKey2, TKey3>(GetDbContext());
        }

        public override ICompoundKeyRepository<T> GetCompoundKeyInstance<T>()
        {
            return new EfCompoundKeyRepository<T>(GetDbContext());
        }

        private DbContext GetDbContext()
        {
            var connectionString = RepositoryConfiguration["connectionString"];
            var dbContextTypeString = RepositoryConfiguration["dbContextType"];
            Type dbContextType = null;
            if (!String.IsNullOrEmpty(dbContextTypeString))
            {
                dbContextType = Type.GetType(dbContextTypeString);
            }

            // TODO: look at dbContextType (from Enyim.Caching configuration bits) and how it caches, see about implementing cache or expanding FastActivator to take parameters
            DbContext dbContext = null;

            // the default way of getting a DbContext if there is no Ioc container setup
            if (dbContextType != null)
            {
                dbContext = String.IsNullOrEmpty(connectionString) ?
                        (DbContext)Activator.CreateInstance(dbContextType) :
                        (DbContext)Activator.CreateInstance(dbContextType, connectionString);

                return dbContext;
            }

            // check for required parameters
            if (RepositoryDependencyResolver.Current == null)
            {
                throw new ConfigurationErrorsException("The connectionString and dbContextType attribute are required in order to use the EfRepository via the configuration file, unless you set the RepositoryDependencyResolver to use an Ioc container.");
            }
            else
            {
                // if there is an IOC dependency resolver configured then use that one to get the DbContext, this will allow sharing of context across multiple repositories if the IOC is configured that way
                return dbContextType == null
                                    ? RepositoryDependencyResolver.Current.GetService<DbContext>()
                                    : (DbContext)RepositoryDependencyResolver.Current.GetService(dbContextType);
            }
        }
    }
}
