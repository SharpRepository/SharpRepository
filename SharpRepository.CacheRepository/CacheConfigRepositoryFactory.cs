using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.CacheRepository
{
    public class CacheConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public CacheConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            return new CacheRepository<T>(RepositoryConfiguration["prefix"]);
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new CacheRepository<T, TKey>(RepositoryConfiguration["prefix"]);
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            return new CacheRepository<T, TKey, TKey2>(RepositoryConfiguration["prefix"]);
        }
    }
}
