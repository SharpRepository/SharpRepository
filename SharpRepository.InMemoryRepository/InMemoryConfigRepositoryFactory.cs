using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.InMemoryRepository
{
    public class InMemoryConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public InMemoryConfigRepositoryFactory(IRepositoryConfiguration config)
            : base(config)
        {
        }

        public override IRepository<T> GetInstance<T>()
        {
            return new InMemoryRepository<T>();
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new InMemoryRepository<T, TKey>();
        }

        public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
        {
            return new InMemoryRepository<T, TKey, TKey2>();
        }
    }
}
