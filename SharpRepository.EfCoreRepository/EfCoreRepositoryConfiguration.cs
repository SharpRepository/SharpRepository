using Microsoft.EntityFrameworkCore;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;

namespace SharpRepository.EfCoreRepository
{
    public class EfCoreRepositoryConfiguration : RepositoryConfiguration
    {
        protected DbContext DbContext { get; set; }

        public EfCoreRepositoryConfiguration(string name)
            : this(name, null, null)
        {
        }

        public EfCoreRepositoryConfiguration(string name, DbContext dbContext)
            : this(name, null, null)
        {
            DbContext = dbContext;
        }

        public EfCoreRepositoryConfiguration(string name, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            Factory = typeof(EfCoreConfigRepositoryFactory);
        }

        public EfCoreRepositoryConfiguration(string name, DbContext dbContext, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            DbContext = dbContext;
            Factory = typeof(EfCoreConfigRepositoryFactory);
        }

        public string ConnectionStringOrName
        {
            set { Attributes["connectionString"] = value; }
        }

        public override IRepository<T> GetInstance<T>()
        {
            // load up the factory if it exists and use it
            var factory = DbContext != null ?
                                (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this, DbContext) :
                                (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T>();
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // load up the factory if it exists and use it
            var factory = DbContext != null ?
                               (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this, DbContext) :
                               (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey>();
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            // load up the factory if it exists and use it
            var factory = DbContext != null ?
                                (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this, DbContext) :
                                (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2>();
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            // load up the factory if it exists and use it
            var factory = DbContext != null ?
                               (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this, DbContext) :
                               (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2, TKey3>();
        }
    }
}