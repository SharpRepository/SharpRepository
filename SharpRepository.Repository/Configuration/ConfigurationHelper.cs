using System;
using SharpRepository.Repository.Caching;

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

        public static IRepository<T, TKey> GetDefaultInstance<T, TKey>(ISharpRepositoryConfiguration configuration) where T : class, new()
        {
            // I thought repositoriesSection.Sections was just ones that the user added to their configuration but it is all that are defined at the top section and sectionGroups part

            // see if there is a default repository section
            var defaultSection = configuration.Repositories.Sections["default"];

            // get the first one that is of type IRepositoryElement
            //  this is totally hacky right now
            var defaultRepositoryElement = configuration.Repositories.Sections[0] as IRepositoryElement;
            if (defaultRepositoryElement == null && configuration.Repositories.Sections.Count > 1)
            {
                defaultRepositoryElement = configuration.Repositories.Sections[1] as IRepositoryElement;
            }

            if (defaultSection != null)
            {
                var defaultName = ((DefaultSection)defaultSection).Name;

                if (!String.IsNullOrEmpty(defaultName) && defaultName != defaultRepositoryElement.Name)
                {
                    foreach (var section in configuration.Repositories.Sections)
                    {
                        var tmp = section as IRepositoryElement;
                        if (tmp == null || tmp.Name != defaultName) continue;
                        defaultRepositoryElement = tmp;
                        break;
                    }
                }
            }

            if (defaultRepositoryElement == null)
                return null;

            var repository =  defaultRepositoryElement.GetInstance<T, TKey>();

            // now let's find the caching strategy 
            ICachingStrategy<T, TKey> strategy = null;
            string cachingProviderName = null;

            var cachingStrategyName = defaultRepositoryElement.CachingStrategy;

            foreach (var section in configuration.CachingStrategies.Sections)
            {
                var tmp = section as ICachingStrategyElement;
                if (tmp == null || tmp.Name != cachingStrategyName) continue;
                strategy = tmp.GetInstance<T, TKey>();
                cachingProviderName = tmp.CachingProvider;

                break;
            }

            // what caching provider should we use
            if (strategy != null && !String.IsNullOrEmpty(cachingProviderName))
            {
                ICachingProvider provider = null;

                foreach (var section in configuration.CachingProviders.Sections)
                {
                    var tmp = section as ICachingProviderElement;
                    if (tmp == null || tmp.Name != cachingProviderName) continue;
                    provider = tmp.GetInstance();

                    strategy.CachingProvider = provider;

                    break;
                }
            }

            repository.CachingStrategy = strategy;

            return repository;
        }
    }
}
