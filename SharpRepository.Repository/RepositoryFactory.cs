using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
using System.Configuration;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Helpers;

namespace SharpRepository.Repository
{
    public static class RepositoryFactory
    {
//        private static readonly IDictionary<string, ISharpRepositoryConfiguration> _cache;
//        private static ISharpRepositoryConfiguration _configuration = null;
//
//        static RepositoryFactory()
//        {
//            _cache = new ConcurrentDictionary<string, ISharpRepositoryConfiguration>();
//        }

        public static IRepository<T, int> GetInstance<T>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, int>(repositoryName);
        }

        public static IRepository<T, int> GetInstance<T>(string configSection, string repositoryName) where T : class, new()
        {
            return GetInstance<T, int>(configSection, repositoryName);
        }

        public static IRepository<T, int> GetInstance<T>(ISharpRepositoryConfiguration configuration, string repositoryName=null) where T : class, new()
        {
            return GetInstance<T, int>(configuration, repositoryName);
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey>("sharpRepository", repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, "sharpRepository", repositoryName);
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(string configSection, string repositoryName) where T : class, new()
        {
//            ISharpRepositoryConfiguration configuration;
//            var key = String.Format("{0}::{1}::{2}", typeof (T).FullName, typeof (TKey).Name, configSection);
//            // first check cache
//            if (_cache.ContainsKey(key))
//            {
//                configuration = _cache[key];
//            }
//            else
//            {
//                configuration = GetConfiguration(configSection);
//                _cache[key] = configuration;
//            }

//            if (_configuration == null)
//                _configuration = GetConfiguration(configSection);

            return GetInstance<T, TKey>(GetConfiguration(configSection), repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, string configSection, string repositoryName)
        {
            return GetInstance(entityType, keyType, GetConfiguration(configSection), repositoryName);
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(ISharpRepositoryConfiguration configuration, string repositoryName=null) where T : class, new()
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(typeof (T));
            }

            return configuration.GetInstance<T, TKey>(repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(entityType);
            }

            var method = typeof(ISharpRepositoryConfiguration).GetMethod("GetInstance", new [] { typeof(string) });
            var genericMethod = method.MakeGenericMethod(entityType, keyType);
            return genericMethod.Invoke(configuration, new object[] { repositoryName });
        }

        private static SharpRepositorySection GetConfiguration(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName) as SharpRepositorySection;
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            return section;
        }

        private static string GetAttributeRepositoryName(Type entityType)
        {
            var attribute = entityType.GetOneAttribute<SharpRepositoryConfigurationAttribute>();
            return attribute == null ? null : attribute.RepositoryName;
        }
    }
}
