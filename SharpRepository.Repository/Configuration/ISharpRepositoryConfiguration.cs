using System.Collections.Generic;

namespace SharpRepository.Repository.Configuration
{
    public interface ISharpRepositoryConfiguration
    {
        bool HasRepository { get; }
        IList<IRepositoryConfiguration> Repositories { get; }
        string DefaultRepository { get; set; }
        IRepositoryConfiguration GetRepository(string repositoryName);

        bool HasCachingStrategies { get; }
        IList<ICachingStrategyConfiguration> CachingStrategies { get; }
        string DefaultCachingStrategy { get; set; }
        ICachingStrategyConfiguration GetCachingStrategy(string strategyName);

        bool HasCachingProviders { get; }
        IList<ICachingProviderConfiguration> CachingProviders { get; }
        string DefaultCachingProvider { get; set; }
        ICachingProviderConfiguration GetCachingProvider(string providerName);

        IRepository<T> GetInstance<T>(string repositoryName = null) where T : class, new();
        IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new();
        ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(string repositoryName = null) where T : class, new();
        ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(string repositoryName = null) where T : class, new();
        ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(string repositoryName = null) where T : class, new();
    }
}
