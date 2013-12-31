using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SimpleInjector;

namespace SharpRepository.Ioc.SimpleInjector
{
    public static class SimpleInjectorRepositoryExtensions
    {
        public static void RegisterSharpRepository(this Container container, string repositoryName = null)
        {
            container.ResolveUnregisteredType += (s, e) =>
            {
                var type = e.UnregisteredServiceType;
                if (type.IsGenericType)
                {
                    var args = type.GetGenericArguments();
                    var typedef = type.GetGenericTypeDefinition();
                    if (typedef == typeof (IRepository<>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], repositoryName));
                    }
                    else if (typedef == typeof (IRepository<,>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], args[1], repositoryName));
                    }
                    else if (typedef == typeof (ICompoundKeyRepository<,,>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], args[1], args[2], repositoryName));
                    }
                }
            };
        }

        public static void RegisterSharpRepository(this Container container, ISharpRepositoryConfiguration configuration)
        {

            container.ResolveUnregisteredType += (s, e) =>
            {
                var type = e.UnregisteredServiceType;
                if (type.IsGenericType)
                {
                    var args = type.GetGenericArguments();
                    var typedef = type.GetGenericTypeDefinition();
                    if (typedef == typeof(IRepository<>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], configuration));
                    }
                    else if (typedef == typeof(IRepository<,>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], args[1], configuration));
                    }
                    else if (typedef == typeof(ICompoundKeyRepository<,,>))
                    {
                        e.Register(() => RepositoryFactory.GetInstance(args[0], args[1], args[2], configuration));
                    }
                }
            };
        }
    }
}
