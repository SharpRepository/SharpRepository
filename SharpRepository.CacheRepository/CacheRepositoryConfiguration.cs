using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using System;

namespace SharpRepository.CacheRepository
{
    public class CacheRepositoryConfiguration : RepositoryConfiguration
    {
        protected ICachingProvider CachingProviderInstance { get; set; }

        public CacheRepositoryConfiguration(string name, ICachingProvider cachingProviderInstance, string cachingStrategy = null, string cachingProvider = null)
            : this(name, "", cachingProviderInstance, cachingStrategy, cachingProvider)
        {
        }

        public CacheRepositoryConfiguration(string name, string prefix, ICachingProvider cachingProviderInstance, string cachingStrategy = null, string cachingProvider = null)
            : base(name)
        {
            Prefix = prefix;
            CachingStrategy = cachingStrategy;
            CachingProvider = cachingProvider;
            CachingProviderInstance = cachingProviderInstance;
            Factory = typeof(CacheConfigRepositoryFactory);
        }

        public string Prefix
        {
            set { Attributes["prefix"] = value; }
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            // load up the factory if it exists and use it
            var factory = new CacheConfigRepositoryFactory(this, CachingProviderInstance);

            return factory.GetInstance<T, TKey>();
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            // load up the factory if it exists and use it
            var factory = new CacheConfigRepositoryFactory(this, CachingProviderInstance);

            return factory.GetInstance<T, TKey, TKey2>();
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            // load up the factory if it exists and use it
            var factory = new CacheConfigRepositoryFactory(this, CachingProviderInstance);

            return factory.GetInstance<T, TKey, TKey2, TKey3>();
        }

        public override ICompoundKeyRepository<T> GetCompoundKeyInstance<T>()
        {
            // load up the factory if it exists and use it
            var factory = new CacheConfigRepositoryFactory(this, CachingProviderInstance);

            return factory.GetCompoundKeyInstance<T>();
        }
    }
}
