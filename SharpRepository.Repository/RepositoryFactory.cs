using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Reflection;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Helpers;
using Microsoft.Extensions.Options;
using System.IO;

namespace SharpRepository.Repository
{
    public static class RepositoryFactory
    {
        private const string DefaultConfigSection = "sharpRepository";
        
        public static IRepository<T, int> GetInstance<T>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, int>(repositoryName);
        }

        public static IRepository<T, int> GetInstance<T>(IOptions<SharpRepositoryConfiguration> configuration, string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, int>(configuration.Value, repositoryName);
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

        public static IRepository<T, TKey> GetInstance<T, TKey>(IOptions<SharpRepositoryConfiguration> configuration, string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey>(configuration.Value, repositoryName);
        }

        public static object GetInstance(Type entityType, string repositoryName = null)
        {
            return GetInstance(entityType, DefaultConfigSection, repositoryName);
        }

        public static object GetInstance(Type entityType, IOptions<SharpRepositoryConfiguration> configuration, string repositoryName = null)
        {
            return GetInstance(entityType, configuration.Value, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, DefaultConfigSection, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, IOptions<SharpRepositoryConfiguration> configuration, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, configuration.Value, repositoryName);
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

            var methods = typeof(ISharpRepositoryConfiguration).GetRuntimeMethods();
            var method = methods.First(m => m.Name == "GetInstance" && m.ReturnType.Name == "IRepository`1");
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

            var methods = typeof(ISharpRepositoryConfiguration).GetRuntimeMethods();
            var method = methods.First(m => m.Name == "GetInstance" && m.ReturnType.Name == "IRepository`2");
            var genericMethod = method.MakeGenericMethod(entityType, keyType);
            return genericMethod.Invoke(configuration, new object[] { repositoryName });
        }

        // compound key methods
        public static ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2>(DefaultConfigSection, repositoryName);
        }

        public static ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(IOptions<SharpRepositoryConfiguration> configuration, string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2>(configuration.Value, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, key2Type, DefaultConfigSection, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, IOptions<SharpRepositoryConfiguration> configuration, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, key2Type, configuration.Value, repositoryName);
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

            var methods = typeof(ISharpRepositoryConfiguration).GetRuntimeMethods();
            var method = methods.First(m => m.Name == "GetInstance" && m.ReturnType.Name == "ICompoundKeyRepository`3");
            var genericMethod = method.MakeGenericMethod(entityType, keyType, key2Type);
            return genericMethod.Invoke(configuration, new object[] { repositoryName });
        }

        // triple compound key methods
        public static ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2, TKey3>(DefaultConfigSection, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, Type key3Type, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, key2Type, key3Type, DefaultConfigSection, repositoryName);
        }

        public static ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(string configSection, string repositoryName) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2, TKey3>(GetConfiguration(configSection), repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, Type key3Type, string configSection, string repositoryName)
        {
            return GetInstance(entityType, keyType, key2Type, key3Type, GetConfiguration(configSection), repositoryName);
        }

        public static ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(IOptions<SharpRepositoryConfiguration> configuration, string repositoryName) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2, TKey3>(configuration, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, Type key3Type, IOptions<SharpRepositoryConfiguration> configuration, string repositoryName)
        {
            return GetInstance(entityType, keyType, key2Type, key3Type, configuration, repositoryName);
        }
        
        public static ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(ISharpRepositoryConfiguration configuration, string repositoryName = null) where T : class, new()
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(typeof(T));
            }

            return configuration.GetInstance<T, TKey, TKey2, TKey3>(repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, Type key3Type, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(entityType);
            }

            var method = typeof(ISharpRepositoryConfiguration).GetTypeInfo().DeclaredMethods.First(m => m.Name == "GetInstance" && m.ReturnType.Name == "ICompoundKeyRepository`4");
            var genericMethod = method.MakeGenericMethod(entityType, keyType, key2Type, key3Type);
            return genericMethod.Invoke(configuration, new object[] { repositoryName });
        }

        /// compound key no generics methods
        public static ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(string repositoryName = null) where T : class, new()
        {
            return GetCompoundKeyInstance<T>(DefaultConfigSection, repositoryName);
        }

        public static object GetCompoundKeyInstance(Type entityType, string repositoryName = null)
        {
            return GetCompoundKeyInstance(entityType, DefaultConfigSection, repositoryName);
        }

        public static ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(string configSection, string repositoryName) where T : class, new()
        {
            return GetCompoundKeyInstance<T>(GetConfiguration(configSection), repositoryName);
        }

        public static object GetCompoundKeyInstance(Type entityType, string configSection, string repositoryName)
        {
            return GetCompoundKeyInstance(entityType, GetConfiguration(configSection), repositoryName);
        }

        public static ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(ISharpRepositoryConfiguration configuration, string repositoryName = null) where T : class, new()
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(typeof(T));
            }

            return configuration.GetCompoundKeyInstance<T>(repositoryName);
        }

        public static object GetCompoundKeyInstance(Type entityType, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(entityType);
            }

            var method = typeof(ISharpRepositoryConfiguration).GetTypeInfo().DeclaredMethods.First(m => m.Name == "GetCompoundKeyInstance");
            var genericMethod = method.MakeGenericMethod(entityType);
            return genericMethod.Invoke(configuration, new object[] { repositoryName });
        }

        //helper methods for configuration
        private static SharpRepositorySection GetConfiguration(string sectionName)
        {
            var config  = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile("app.config")
                .Build();

            var section = config.GetSection(sectionName) as SharpRepositorySection;
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

            return section;
        }

        private static string GetAttributeRepositoryName(Type entityType)
        {
            var attribute = entityType.GetTypeInfo().GetOneAttribute<SharpRepositoryConfigurationAttribute>();
            return attribute?.RepositoryName;
        }
    }
}
