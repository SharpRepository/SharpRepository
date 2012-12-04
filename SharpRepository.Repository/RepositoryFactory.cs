using System;
using System.Configuration;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Repository
{
    public static class RepositoryFactory
    {
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
            return GetInstance<T, TKey>(GetConfiguration(configSection), repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, string configSection, string repositoryName)
        {
            return GetInstance(entityType, keyType, GetConfiguration(configSection), repositoryName);
        }

        public static IRepository<T, TKey> GetInstance<T, TKey>(ISharpRepositoryConfiguration configuration, string repositoryName=null) where T : class, new()
        {
            return configuration.GetInstance<T, TKey>(repositoryName);
        }

        public static object GetInstance(Type entityType, Type keyType, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
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
    }
}
