using System;
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
    public static class ConfigurationHelper
    {
        public static void CheckForInterface(Type type, Type interfaceType)
        {
            if (type == null || interfaceType == null) return;

            if (Array.IndexOf<Type>(type.GetInterfaces(), interfaceType) == -1)
                throw new System.Configuration.ConfigurationErrorsException("The type " + type.AssemblyQualifiedName + " must implement " + interfaceType.AssemblyQualifiedName);
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(ISharpRepositoryConfiguration configuration, string repositoryName) where T : class, new()
        {
            // get the repositories collection
            var repositories = configuration.Repositories;

            if (repositories == null || repositories.Count == 0)
            {
                throw new Exception("There are no repositories configured");
            }

            if (String.IsNullOrEmpty(repositoryName))
            {
                repositoryName = configuration.DefaultRepository;
            }

            var repositoryConfiguration = repositories.FirstOrDefault(r => r.Name == repositoryName);

            // if this is null and they provided an actual repository name then throw an error, else just pick the first one
            if (repositoryConfiguration == null)
            {
                if (!String.IsNullOrEmpty(repositoryName))
                {
                    throw new Exception(String.Format("There is no repository configured with the name '{0}'", repositoryName));
                }

                repositoryConfiguration = repositories.First();
            }

            var repository = repositoryConfiguration.GetInstance<T, TKey>();

            if (repository == null)
                return null;

            var strategies = configuration.CachingStrategies;
            var providers = configuration.CachingProviders;

            // caching strategies
            //  1st check to see if this particular repository has a strategy declared
            //      if so, find it and use it
            //      if not, check for a default delcaration to use
            var strategyName = repositoryConfiguration.CachingStrategy;
            if (String.IsNullOrEmpty(strategyName) && strategies != null)
            {
                strategyName = configuration.DefaultCachingStrategy;
            }

            // no caching strategy so do it without one
            if (String.IsNullOrEmpty(strategyName) || strategies == null)
            {
                return repository;
            }

            var strategyConfiguration = strategies.FirstOrDefault(s => s.Name == strategyName);
            if (strategyConfiguration == null)
            {
                return repository;
            }

            var cachingStrategy = strategyConfiguration.GetInstance<T, TKey>();

            if (cachingStrategy == null)
            {
                return repository;
            }

            // caching providers
            //  2nd check to see if this particular repository has a provider declared
            //      if so, find it and use it with this strategy
            //      if not, check for a default declaration to use
            var providerName = repositoryConfiguration.CachingProvider;
            if (String.IsNullOrEmpty(providerName) && providers != null)
            {
                providerName = configuration.DefaultCachingProvider;
            }

            if (!String.IsNullOrEmpty(providerName) || providers == null)
            {
                var providerConfiguration = providers.FirstOrDefault(s => s.Name == providerName);
                if (providerConfiguration != null)
                {
                    cachingStrategy.CachingProvider = providerConfiguration.GetInstance();
                }
            }

            repository.CachingStrategy = cachingStrategy;

            return repository;
            // I thought repositoriesSection.Sections was just ones that the user added to their configuration but it is all that are defined at the top section and sectionGroups part

            // see if there is a default repository section
//            var defaultSection = configuration.Repositories.Sections["default"];
//
//            // get the first one that is of type IRepositoryElement
//            //  this is totally hacky right now
//            var defaultRepositoryElement = configuration.Repositories.Sections[0] as IRepositoryElement;
//            if (defaultRepositoryElement == null && configuration.Repositories.Sections.Count > 1)
//            {
//                defaultRepositoryElement = configuration.Repositories.Sections[1] as IRepositoryElement;
//            }
//
//            if (defaultSection != null)
//            {
//                var defaultName = ((DefaultSection)defaultSection).Name;
//
//                if (!String.IsNullOrEmpty(defaultName) && defaultName != defaultRepositoryElement.Name)
//                {
//                    foreach (var section in configuration.Repositories.Sections)
//                    {
//                        var tmp = section as IRepositoryElement;
//                        if (tmp == null || tmp.Name != defaultName) continue;
//                        defaultRepositoryElement = tmp;
//                        break;
//                    }
//                }
//            }
//
//            if (defaultRepositoryElement == null)
//                return null;
//
//            var repository =  defaultRepositoryElement.GetInstance<T, TKey>();
//
//            // now let's find the caching strategy 
//            ICachingStrategy<T, TKey> strategy = null;
//            string cachingProviderName = null;
//
//            var cachingStrategyName = defaultRepositoryElement.CachingStrategy;
//
//            foreach (var section in configuration.CachingStrategies.Sections)
//            {
//                var tmp = section as ICachingStrategyElement;
//                if (tmp == null || tmp.Name != cachingStrategyName) continue;
//                strategy = tmp.GetInstance<T, TKey>();
//                cachingProviderName = tmp.CachingProvider;
//
//                break;
//            }
//
//            // what caching provider should we use
//            if (strategy != null && !String.IsNullOrEmpty(cachingProviderName))
//            {
//                ICachingProvider provider = null;
//
//                foreach (var section in configuration.CachingProviders.Sections)
//                {
//                    var tmp = section as ICachingProviderElement;
//                    if (tmp == null || tmp.Name != cachingProviderName) continue;
//                    provider = tmp.GetInstance();
//
//                    strategy.CachingProvider = provider;
//
//                    break;
//                }
//            }
//
//            repository.CachingStrategy = strategy;
//
//            return repository;
        }
    }
}
