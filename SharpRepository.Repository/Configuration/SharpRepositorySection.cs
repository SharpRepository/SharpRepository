using System.Collections.Generic;
using System.Configuration;

namespace SharpRepository.Repository.Configuration
{
    public class SharpRepositorySection : ConfigurationSection, ISharpRepositoryConfiguration
    {
        [ConfigurationProperty("repositories", IsRequired = true)]
        public RepositoriesCollection Repositories
        {
            get { return (RepositoriesCollection)this["repositories"]; }
        }

        [ConfigurationProperty("cachingStrategies")]
        public CachingStrategyCollection CachingStrategies
        {
            get { return (CachingStrategyCollection)this["cachingStrategies"]; }
        }

        [ConfigurationProperty("cachingProviders")]
        public CachingProviderCollection CachingProviders
        {
            get { return (CachingProviderCollection)this["cachingProviders"]; }
        }

        public IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey>(this, repositoryName);
        }

        IList<IRepositoryConfiguration> ISharpRepositoryConfiguration.Repositories
        {
            get { return Repositories.ToRepositoryConfigurationList(); }
        }

        string ISharpRepositoryConfiguration.DefaultRepository
        {
            get { return Repositories.Default; }
            set { Repositories.Default = value; }
        }


        IList<ICachingStrategyConfiguration> ISharpRepositoryConfiguration.CachingStrategies
        {
            get { return CachingStrategies.ToCachingStrategyConfigurationList(); }
        }

        string ISharpRepositoryConfiguration.DefaultCachingStrategy
        {
            get { return CachingStrategies.Default; }
            set { CachingStrategies.Default = value; }
        }


        IList<ICachingProviderConfiguration> ISharpRepositoryConfiguration.CachingProviders
        {
            get { return CachingProviders.ToCachingProviderConfigurationList(); }
        }

        string ISharpRepositoryConfiguration.DefaultCachingProvider
        {
            get { return CachingProviders.Default; }
            set { CachingProviders.Default = value; }
        }
    }
}
