
namespace SharpRepository.Repository.Configuration
{
    public interface IConfigRepositoryFactory
    {
        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
    }

    public abstract class ConfigRepositoryFactory : IConfigRepositoryFactory
    {
        protected RepositoryElement RepositoryElement;

        protected ConfigRepositoryFactory(RepositoryElement repositoryElement)
        {
            RepositoryElement = repositoryElement;
        }

        public abstract IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
    }
}
