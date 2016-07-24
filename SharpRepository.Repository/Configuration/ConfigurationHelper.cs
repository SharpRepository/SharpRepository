using System;
using System.Linq;
using System.Reflection;

namespace SharpRepository.Repository.Configuration
{
    public static class ConfigurationHelper
    {
        public static void CheckForInterface(Type type, Type interfaceType)
        {
            if (type == null || interfaceType == null) return;

#if NET451
            if (Array.IndexOf<Type>(type.GetInterfaces(), interfaceType) == -1)
                throw new System.Configuration.ConfigurationErrorsException("The type " + type.AssemblyQualifiedName + " must implement " + interfaceType.AssemblyQualifiedName);
#elif NETSTANDARD1_6
            if (type.GetTypeInfo().GetRuntimeInterfaceMap(interfaceType).InterfaceType == interfaceType)
                throw new Exception("The type " + type.AssemblyQualifiedName + " must implement " + interfaceType.AssemblyQualifiedName);
#endif
        }

        public static IRepository<T> GetInstance<T>(ISharpRepositoryConfiguration configuration, string repositoryName) where T : class, new()
        {
            if (!configuration.HasRepository)
            {
                throw new Exception("There are no repositories configured");
            }

            var repositoryConfiguration = configuration.GetRepository(repositoryName);
            var repository = repositoryConfiguration.GetInstance<T>();

            if (repository == null)
                return null;

            var strategyConfiguration = configuration.GetCachingStrategy(repositoryConfiguration.CachingStrategy);
            if (strategyConfiguration == null)
            {
                return repository;
            }

            var cachingStrategy = strategyConfiguration.GetInstance<T, int>();
            if (cachingStrategy == null)
            {
                return repository;
            }

            var providerConfiguration = configuration.GetCachingProvider(repositoryConfiguration.CachingProvider);
            if (providerConfiguration != null)
            {
                cachingStrategy.CachingProvider = providerConfiguration.GetInstance();
            }

            repository.CachingStrategy = cachingStrategy;

            return repository;
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(ISharpRepositoryConfiguration configuration, string repositoryName) where T : class, new()
        {
            if (!configuration.HasRepository)
            {
                throw new Exception("There are no repositories configured");
            }

            var repositoryConfiguration = configuration.GetRepository(repositoryName);
            var repository = repositoryConfiguration.GetInstance<T, TKey>();

            if (repository == null)
                return null;

            var strategyConfiguration = configuration.GetCachingStrategy(repositoryConfiguration.CachingStrategy);
            if (strategyConfiguration == null)
            {
                return repository;
            }

            var cachingStrategy = strategyConfiguration.GetInstance<T, TKey>();
            if (cachingStrategy == null)
            {
                return repository;
            }

            var providerConfiguration = configuration.GetCachingProvider(repositoryConfiguration.CachingProvider);
            if (providerConfiguration != null)
            {
                cachingStrategy.CachingProvider = providerConfiguration.GetInstance();
            }

            repository.CachingStrategy = cachingStrategy;

            return repository;
        }

        public static ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(ISharpRepositoryConfiguration configuration, string repositoryName) where T : class, new()
        {
            if (!configuration.HasRepository)
            {
                throw new Exception("There are no repositories configured");
            }

            var repositoryConfiguration = configuration.GetRepository(repositoryName);
            var repository = repositoryConfiguration.GetInstance<T, TKey, TKey2>();

            if (repository == null)
                return null;

            var strategyConfiguration = configuration.GetCachingStrategy(repositoryConfiguration.CachingStrategy);
            if (strategyConfiguration == null)
            {
                return repository;
            }

            var cachingStrategy = strategyConfiguration.GetInstance<T, TKey, TKey2>();
            if (cachingStrategy == null)
            {
                return repository;
            }

            var providerConfiguration = configuration.GetCachingProvider(repositoryConfiguration.CachingProvider);
            if (providerConfiguration != null)
            {
                cachingStrategy.CachingProvider = providerConfiguration.GetInstance();
            }

            repository.CachingStrategy = cachingStrategy;

            return repository;
        }

        public static ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(ISharpRepositoryConfiguration configuration, string repositoryName) where T : class, new()
        {
            if (!configuration.HasRepository)
            {
                throw new Exception("There are no repositories configured");
            }

            var repositoryConfiguration = configuration.GetRepository(repositoryName);
            var repository = repositoryConfiguration.GetInstance<T, TKey, TKey2, TKey3>();

            if (repository == null)
                return null;

            var strategyConfiguration = configuration.GetCachingStrategy(repositoryConfiguration.CachingStrategy);
            if (strategyConfiguration == null)
            {
                return repository;
            }

            var cachingStrategy = strategyConfiguration.GetInstance<T, TKey, TKey2, TKey3>();
            if (cachingStrategy == null)
            {
                return repository;
            }

            var providerConfiguration = configuration.GetCachingProvider(repositoryConfiguration.CachingProvider);
            if (providerConfiguration != null)
            {
                cachingStrategy.CachingProvider = providerConfiguration.GetInstance();
            }

            repository.CachingStrategy = cachingStrategy;

            return repository;
        }

        public static ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(ISharpRepositoryConfiguration configuration, string repositoryName) where T : class, new()
        {
            if (!configuration.HasRepository)
            {
                throw new Exception("There are no repositories configured");
            }

            var repositoryConfiguration = configuration.GetRepository(repositoryName);
            var repository = repositoryConfiguration.GetCompoundKeyInstance<T>();

            if (repository == null)
                return null;

            var strategyConfiguration = configuration.GetCachingStrategy(repositoryConfiguration.CachingStrategy);
            if (strategyConfiguration == null)
            {
                return repository;
            }

            var cachingStrategy = strategyConfiguration.GetCompoundKeyInstance<T>();
            if (cachingStrategy == null)
            {
                return repository;
            }

            var providerConfiguration = configuration.GetCachingProvider(repositoryConfiguration.CachingProvider);
            if (providerConfiguration != null)
            {
                cachingStrategy.CachingProvider = providerConfiguration.GetInstance();
            }

            repository.CachingStrategy = cachingStrategy;

            return repository;
        }
    }
}
