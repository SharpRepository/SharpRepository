using System.Collections.Generic;

namespace SharpRepository.Repository.Configuration
{
    public interface ISharpRepositoryConfiguration
    {
        IList<IRepositoryConfiguration> Repositories { get; }
        string DefaultRepository { get; set; }

        IList<ICachingStrategyConfiguration> CachingStrategies { get; }
        string DefaultCachingStrategy { get; set; }

        IList<ICachingProviderConfiguration> CachingProviders { get; }
        string DefaultCachingProvider { get; set; }

        IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new();
    }
}
