using System;
using System.Configuration;
using System.Linq;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Helpers;

namespace SharpRepository.Repository
{
    public static class RepositoryFactory
    {
        private const string DefaultConfigSection = "sharpRepository";

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
            return GetInstance<T, TKey>(DefaultConfigSection, repositoryName);
        }

        public static object GetInstance(Type entityType, string repositoryName = null)
        {
            return GetInstance(entityType, DefaultConfigSection, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, DefaultConfigSection, repositoryName);
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(string configSection, string repositoryName) where T : class, new()
        {
            return GetInstance<T, TKey>(GetConfiguration(configSection), repositoryName);
        }

        public static object GetInstance(Type entityType, string configSection, string repositoryName)
        {
            return GetInstance(entityType, GetConfiguration(configSection), repositoryName);
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

        public static object GetInstance(Type entityType, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(entityType);
            }

            var method = typeof(ISharpRepositoryConfiguration).GetMethods().First(m => m.Name == "GetInstance" && m.ReturnType.Name == "IRepository`1");
            var genericMethod = method.MakeGenericMethod(entityType);
            return genericMethod.Invoke(configuration, new object[] { repositoryName });
        }

        public static object GetInstance(Type entityType, Type keyType, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(entityType);
            }

            var method = typeof(ISharpRepositoryConfiguration).GetMethods().First(m => m.Name == "GetInstance" && m.ReturnType.Name == "IRepository`2");
            var genericMethod = method.MakeGenericMethod(entityType, keyType);
            return genericMethod.Invoke(configuration, new object[] { repositoryName });
        }

        // compound key methods

        public static ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2>(DefaultConfigSection, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, key2Type, DefaultConfigSection, repositoryName);
        }

        public static ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(string configSection, string repositoryName) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2>(GetConfiguration(configSection), repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, string configSection, string repositoryName)
        {
            return GetInstance(entityType, keyType, key2Type, GetConfiguration(configSection), repositoryName);
        }

        public static ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(ISharpRepositoryConfiguration configuration, string repositoryName = null) where T : class, new()
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(typeof(T));
            }

            return configuration.GetInstance<T, TKey, TKey2>(repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type,ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(entityType);
            }

            var method = typeof(ISharpRepositoryConfiguration).GetMethods().First(m => m.Name == "GetInstance" && m.ReturnType.Name == "ICompoundKeyRepository`3");
            var genericMethod = method.MakeGenericMethod(entityType, keyType, key2Type);
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
