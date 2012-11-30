using System;
using System.Collections.Generic;

namespace SharpRepository.Repository.Configuration
{
    public class SharpRepositoryConfiguration : ISharpRepositoryConfiguration
    {
        public IList<IRepositoryConfiguration> Repositories { get; private set; }
        public string DefaultRepository { get; set; }

        public IList<ICachingStrategyConfiguration> CachingStrategies { get; private set; }
        public string DefaultCachingStrategy { get; set; }

        public IList<ICachingProviderConfiguration> CachingProviders { get; private set; }
        public string DefaultCachingProvider { get; set; }

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


        public IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey>(this, repositoryName);
        }
    }
}
