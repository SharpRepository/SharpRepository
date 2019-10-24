using Microsoft.EntityFrameworkCore;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;
using System;

namespace SharpRepository.EfCoreRepository
{
    public class EfCoreConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public EfCoreConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public EfCoreConfigRepositoryFactory(IRepositoryConfiguration config, DbContext dbContext)
            : base(config)
        {
            DbContext = dbContext;
        }

        protected DbContext DbContext { get; set; }

        public override IRepository<T> GetInstance<T>()
        {
            return new EfCoreRepository<T>(GetDbContext());
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new EfCoreRepository<T, TKey>(GetDbContext());
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            return new EfCoreRepository<T, TKey, TKey2>(GetDbContext());
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            return new EfCoreRepository<T, TKey, TKey2, TKey3>(GetDbContext());
        }

        public override ICompoundKeyRepository<T> GetCompoundKeyInstance<T>()
        {
            return new EfCoreCompoundKeyRepository<T>(GetDbContext());
        }

        protected DbContext GetDbContext()
        {
            // check for required parameters
            if (RepositoryDependencyResolver.Current == null && DbContext == null)
            {
                throw new ConfigurationErrorsException("The EfCore repository Factory gets DbContext or DbContextOptionBuilder from RepositoryDependencyResolver containing the Ioc container passing directly DbContextOptions");
            }

            if (DbContext != null)
            {
                return DbContext;
            }

            Type dbContextType = null;

            var tmpDbContextType = RepositoryConfiguration["dbContextType"];
            if (!String.IsNullOrEmpty(tmpDbContextType))
            {
                dbContextType = Type.GetType(tmpDbContextType);

                if (dbContextType == null)
                {
                    throw new NotImplementedException("Unable to find " + tmpDbContextType + " class");
                }
            }
            
            // TODO: look at dbContextType (from Enyim.Caching configuration bits) and how it caches, see about implementing cache or expanding FastActivator to take parameters
            DbContext dbContext = dbContextType == null
                            ? RepositoryDependencyResolver.Current?.GetService<DbContext>()
                            : (DbContext)RepositoryDependencyResolver.Current?.GetService(dbContextType);

            // if the Ioc container doesn't throw an error but still returns null we need to alert the consumer
            if (dbContext != null)
            {
                return dbContext;
            }

            var options = RepositoryDependencyResolver.Current.GetService<DbContextOptions>();
            if (options == null)
            {
                throw new RepositoryDependencyResolverException(typeof(DbContext));
            }

            dbContext = dbContextType == null
                        ? new DbContext(options)
                        : (DbContext)Activator.CreateInstance(dbContextType, options);

            return dbContext;
        }
    }
}