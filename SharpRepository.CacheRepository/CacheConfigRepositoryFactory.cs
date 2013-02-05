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

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new CacheRepository<T, TKey>(RepositoryConfiguration["prefix"]);
        }
    }
}
