using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public class SharpRepositoryConfiguration : ISharpRepositoryConfiguration
    {
        public IList<IRepositoryConfiguration> Repositories { get; private set; }
        public string DefaultRepository { get; set; }
        public bool HasRepository
        {
            get { return Repositories != null && Repositories.Any(); }
        }

        public IRepositoryConfiguration GetRepository(string repositoryName)
        {
            if (!HasRepository)
                throw new Exception("There are no repositories configured.");

            if (String.IsNullOrEmpty(repositoryName))
            {
                repositoryName = DefaultRepository;
            }

            // no default provided, so return the first
            if (String.IsNullOrEmpty(repositoryName))
            {
                return Repositories.First();
            }

            var repositoryConfiguration = Repositories.FirstOrDefault(r => r.Name == repositoryName);

            // if this is null and they provided an actual repository name then throw an error, else just pick the first one
            if (repositoryConfiguration == null)
            {
                throw new Exception(String.Format("There is no repository configured with the name '{0}'", repositoryName));
            }

            return repositoryConfiguration;
        }

        public IList<ICachingStrategyConfiguration> CachingStrategies { get; private set; }
        public string DefaultCachingStrategy { get; set; }
        public bool HasCachingStrategies
        {
            get { return CachingStrategies != null && CachingStrategies.Any(); }
        }

        public ICachingStrategyConfiguration GetCachingStrategy(string strategyName)
        {
            if (!HasCachingStrategies) return null;

            if (String.IsNullOrEmpty(strategyName))
            {
                strategyName = DefaultCachingStrategy;
            }

            return String.IsNullOrEmpty(strategyName) ? null : CachingStrategies.FirstOrDefault(s => s.Name == strategyName);
        }

        public IList<ICachingProviderConfiguration> CachingProviders { get; private set; }
        public string DefaultCachingProvider { get; set; }
        public bool HasCachingProviders
        {
            get { return CachingProviders != null && CachingProviders.Any(); }
        }

        public ICachingProviderConfiguration GetCachingProvider(string providerName = null)
        {
            if (!HasCachingProviders) return null;

            // caching providers
            //  2nd check to see if this particular repository has a provider declared
            //      if so, find it and use it with this strategy
            //      if not, check for a default declaration to use

            if (String.IsNullOrEmpty(providerName))
            {
                providerName = DefaultCachingProvider;
            }

            return String.IsNullOrEmpty(providerName) ? null : CachingProviders.FirstOrDefault(s => s.Name == providerName);
        }

        public SharpRepositoryConfiguration()
        {
            Repositories = new List<IRepositoryConfiguration>();
            CachingStrategies = new List<ICachingStrategyConfiguration>();
            CachingProviders = new List<ICachingProviderConfiguration>();
        }

        public void AddRepository(IRepositoryConfiguration repositoryConfiguration)
        {
            Repositories.Add(repositoryConfiguration);
        }

        public void AddRepository(string name, Type factory, string cachingStrategy = null, string cachingProvider = null,
                                  IDictionary<string, string> attributes = null)
        {
            AddRepository(new RepositoryConfiguration
                              {
                                  Name = name,
                                  Factory = factory,
                                  CachingStrategy = cachingStrategy,
                                  CachingProvider = cachingProvider,
                                  Attributes = attributes ?? new Dictionary<string, string>()
                              });
        }

        public void AddCachingStrategy(ICachingStrategyConfiguration cachingStrategyConfiguration)
        {
            CachingStrategies.Add(cachingStrategyConfiguration);
        }

        public void AddCachingStrategy(string name, Type factory, IDictionary<string, string> attributes = null)
        {
            AddCachingStrategy(new CachingStrategyConfiguration
                                   {
                                       Name = name,
                                       Factory = factory,
                                       Attributes = attributes ?? new Dictionary<string, string>()
                                   });
        }

        public void AddCachingProvider(ICachingProviderConfiguration cachingProviderConfiguration)
        {
            CachingProviders.Add(cachingProviderConfiguration);
        }

        public void AddCachingProvider(string name, Type factory, IDictionary<string, string> attributes = null)
        {
            AddCachingProvider(new CachingProviderConfiguration
                                   {
                                       Name = name,
                                       Factory = factory,
                                       Attributes = attributes ?? new Dictionary<string, string>()
                                   });
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
        public ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetCompoundKeyInstance<T>(this, repositoryName);
        }
    }
}
