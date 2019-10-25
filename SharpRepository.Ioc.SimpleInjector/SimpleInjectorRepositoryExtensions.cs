using Microsoft.Extensions.Configuration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SimpleInjector;
using System.Reflection;

namespace SharpRepository.Ioc.SimpleInjector
{
    public static class SimpleInjectorRepositoryExtensions
    {
        public static void RegisterSharpRepository(this Container container, IConfigurationSection configurationSection, string repositoryName = null)
        {
            if (configurationSection == null)
                throw new ConfigurationErrorsException("Configuration section not found.");

            var configuration = RepositoryFactory.BuildSharpRepositoryConfiguation(configurationSection);

            container.RegisterSharpRepository(configuration, repositoryName);
        }

        public static void RegisterSharpRepository(this Container container, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {

            container.ResolveUnregisteredType += (s, e) =>
            {
                var type = e.UnregisteredServiceType;
                if (type.GetTypeInfo().IsGenericType)
                {
                    var args = type.GetGenericArguments();
                    var typedef = type.GetGenericTypeDefinition();
                    if (typedef == typeof(IRepository<>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], configuration, repositoryName));
                    }
                    else if (typedef == typeof(IRepository<,>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], args[1], configuration, repositoryName));
                    }
                    else if (typedef == typeof(ICompoundKeyRepository<,,>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], args[1], args[2], configuration, repositoryName));
                    }
                    else if (typedef == typeof(ICompoundKeyRepository<,,,>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], args[1], args[2], args[2], configuration, repositoryName));
                    }
                    else if (typedef == typeof(ICompoundKeyRepository<>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], configuration, repositoryName));
                    }
                }
            };
        }
    }
}
