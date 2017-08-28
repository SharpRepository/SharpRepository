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
    public class RepositoryFactory
    {
        protected ISharpRepositoryConfiguration Configuration;

        public RepositoryFactory(IConfigurationSection configurationSection) : this(BuildSharpRepositoryConfiguation(configurationSection))
        {
        }

        public RepositoryFactory(ISharpRepositoryConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static ISharpRepositoryConfiguration BuildSharpRepositoryConfiguation(IConfigurationSection configurationSection)
        {
            var conf = new SharpRepositoryConfiguration();

            IConfigurationSection repositories = configurationSection.GetSection("repositories");
            string defaultRepository = repositories.GetSection("default").Value;
            conf.DefaultRepository = defaultRepository;

            return conf;
        }

        public IRepository<T, int> GetInstance<T>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, int>(Configuration, repositoryName);
        }

        public static IRepository<T, int> GetInstance<T>(ISharpRepositoryConfiguration configuration, string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, int>(configuration, repositoryName);
        }

        public IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey>(Configuration, repositoryName);
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(ISharpRepositoryConfiguration configuration, string repositoryName = null) where T : class, new()
        {
            if (String.IsNullOrEmpty(repositoryName))
            {
                // if no specific repository is provided then check to see if the SharpRepositoryConfigurationAttribute is used
                repositoryName = GetAttributeRepositoryName(typeof(T));
            }

            return configuration.GetInstance<T, TKey>(repositoryName);
        }

        public object GetInstance(Type entityType, string repositoryName = null)
        {
            return GetInstance(entityType, typeof(int), Configuration, repositoryName);
        }

        public object GetInstance(Type entityType, Type keyType, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, Configuration, repositoryName);
        }

        public static object GetInstance(Type entityType, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            return GetInstance(entityType, typeof(int), configuration, repositoryName);
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

        public ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(string repositoryName = null) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2>(Configuration, repositoryName);
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
        public object GetInstance(Type entityType, Type keyType, Type key2Type, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, key2Type, Configuration, repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, Type key2Type, ISharpRepositoryConfiguration configuration, string repositoryName = null)
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
        public ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(string repositoryName) where T : class, new()
        {
            return GetInstance<T, TKey, TKey2, TKey3>(Configuration, repositoryName);
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
        
        public object GetInstance(Type entityType, Type keyType, Type key2Type, Type key3Type, string repositoryName = null)
        {
            return GetInstance(entityType, keyType, key2Type, key3Type, Configuration, repositoryName);
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
        public ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(string repositoryName = null) where T : class, new()
        {
            return GetCompoundKeyInstance<T>(Configuration, repositoryName);
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

        public  object GetCompoundKeyInstance(Type entityType, string repositoryName = null)
        {
            return GetCompoundKeyInstance(entityType, Configuration, repositoryName);
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
        

        private static string GetAttributeRepositoryName(Type entityType)
        {
            var attribute = entityType.GetTypeInfo().GetOneAttribute<SharpRepositoryConfigurationAttribute>();
            return attribute?.RepositoryName;
        }
    }
}
