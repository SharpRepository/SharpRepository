using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public class SharpRepositorySection : ConfigurationSection, ISharpRepositoryConfiguration
    {
        [ConfigurationProperty("repositories", IsRequired = true)]
        public RepositoriesCollection Repositories
        {
            get { return (RepositoriesCollection)this["repositories"]; }
        }

        public bool HasRepository
        {
            get { return Repositories != null && Repositories.Count != 0; }
        }

        public IRepositoryConfiguration GetRepository(string repositoryName)
        {
            if (!HasRepository)
                throw new Exception("There are no repositories configured.");

            if (String.IsNullOrEmpty(repositoryName))
            {
                repositoryName = ((ISharpRepositoryConfiguration) this).DefaultRepository;
            }

            IRepositoryConfiguration repositoryConfiguration = null;

            if (String.IsNullOrEmpty(repositoryName))
            {
                // return the first one
                foreach (RepositoryElement element in Repositories)
                {
                    repositoryConfiguration = element;
                    break;
                }

                return repositoryConfiguration;
            }
            
            // find the repository element by name
            // NOTE: i've intentionally left it as this loop instead of using LINQ because the .Cast<> slows down performance and I think this is just as readable
            foreach (RepositoryElement element in Repositories)
            {
                if (element.Name == repositoryName)
                {
                    repositoryConfiguration = element;
                    break;
                }
            }

            // if this is null then throw an error
            if (repositoryConfiguration == null)
            {
                throw new Exception(String.Format("There is no repository configured with the name '{0}'", repositoryName));
            }

            return repositoryConfiguration;
        }

        [ConfigurationProperty("cachingStrategies")]
        public CachingStrategyCollection CachingStrategies
        {
            get { return (CachingStrategyCollection)this["cachingStrategies"]; }
        }

        public bool HasCachingStrategies
        {
            get { return CachingStrategies != null && CachingStrategies.Count != 0; }
        }

        public ICachingStrategyConfiguration GetCachingStrategy(string strategyName)
        {
            if (!HasCachingStrategies) return null;

            if (String.IsNullOrEmpty(strategyName))
            {
                strategyName = ((ISharpRepositoryConfiguration) this).DefaultCachingStrategy;
            }

            if (String.IsNullOrEmpty(strategyName))
            {
                return null;
            }

            ICachingStrategyConfiguration strategyConfiguration = null;
            foreach (CachingStrategyElement element in CachingStrategies)
            {
                if (element.Name == strategyName)
                {
                    strategyConfiguration = element;
                    break;
                }
            }

            return strategyConfiguration;
        }

        [ConfigurationProperty("cachingProviders")]
        public CachingProviderCollection CachingProviders
        {
            get { return (CachingProviderCollection)this["cachingProviders"]; }
        }

        public bool HasCachingProviders
        {
            get { return CachingProviders != null && CachingProviders.Count != 0; }
        }

        public ICachingProviderConfiguration GetCachingProvider(string providerName)
        {
            if (!HasCachingProviders) return null;

            if (String.IsNullOrEmpty(providerName))
            {
                providerName = ((ISharpRepositoryConfiguration)this).DefaultCachingProvider;
            }

            if (String.IsNullOrEmpty(providerName))
            {
                return null;
            }

            ICachingProviderConfiguration providerConfiguration = null;
            foreach (CachingProviderElement element in CachingProviders)
            {
                if (element.Name == providerName)
                {
                    providerConfiguration = element;
                    break;
                }
            }

            return providerConfiguration;
        }

        public IRepository<T> GetInstance<T>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T>(this, repositoryName);
        }

        public IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey>(this, repositoryName);
        }

        public ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey, TKey2>(this, repositoryName);
        }

        public ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey, TKey2, TKey3>(this, repositoryName);
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
