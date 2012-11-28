using System.Configuration;

namespace SharpRepository.Repository.Configuration
{
    public class SharpRepositorySection : ConfigurationSection
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
    }
}
