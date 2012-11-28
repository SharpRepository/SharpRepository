using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.InMemoryRepository
{
    public class InMemoryConfigRepositoryFactory : ConfigRepositoryFactory
    {
        public InMemoryConfigRepositoryFactory(RepositoryElement repositoryElement) : base(repositoryElement)
        {
        }

        public override IRepository<T, TKey> GetInstance<T, TKey>()
        {
            return new InMemoryRepository<T, TKey>();
        }
    }
}
