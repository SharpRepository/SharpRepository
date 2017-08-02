using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.CacheRepository
{
    public class CacheConfigRepositoryFactory : ConfigRepositoryFactory
    {
        ICachingProvider CacheProvider { get; set; }

        public CacheConfigRepositoryFactory(IRepositoryConfiguration config, ICachingProvider cacheProvider)
            : base(config)
        {
            CacheProvider = cacheProvider;
        }

        public override IRepository<T> GetInstance<T>()
        {
            return new CacheRepository<T>(RepositoryConfiguration["prefix"], CacheProvider);
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new CacheRepository<T, TKey>(RepositoryConfiguration["prefix"], CacheProvider);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            return new CacheRepository<T, TKey, TKey2>(RepositoryConfiguration["prefix"], CacheProvider);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
        {
            return new CacheRepository<T, TKey, TKey2, TKey3>(RepositoryConfiguration["prefix"], CacheProvider);
        }

        public override ICompoundKeyRepository<T> GetCompoundKeyInstance<T>()
        {
            return new CacheCompoundKeyRepository<T>(RepositoryConfiguration["prefix"], CacheProvider);
        }
    }
}
