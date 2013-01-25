
namespace SharpRepository.Repository.Configuration
{
    public interface IConfigRepositoryFactory
    {
        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
        ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();
    }

    public abstract class ConfigRepositoryFactory : IConfigRepositoryFactory
    {
        protected IRepositoryConfiguration RepositoryConfiguration;

        protected ConfigRepositoryFactory(IRepositoryConfiguration config)
        {
            RepositoryConfiguration = config;
        }

        public abstract IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
        public abstract ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();
    }
}
