
namespace SharpRepository.Repository.Configuration
{
    public interface IConfigRepositoryFactory
    {
        IRepository<T> GetInstance<T>() where T : class, new();
        IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
        ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();
        ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new();
        ICompoundKeyRepository<T> GetCompoundKeyInstance<T>() where T : class, new();
    }

    public abstract class ConfigRepositoryFactory : IConfigRepositoryFactory
    {
        protected IRepositoryConfiguration RepositoryConfiguration;

        protected ConfigRepositoryFactory(IRepositoryConfiguration config)
        {
            RepositoryConfiguration = config;
        }

        public abstract IRepository<T> GetInstance<T>() where T : class, new();
        public abstract IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
        public abstract ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();
        public abstract ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new();
        public abstract ICompoundKeyRepository<T> GetCompoundKeyInstance<T>() where T : class, new();
    }
}
