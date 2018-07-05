using System;
using System.Collections.Generic;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace SharpRepository.Ioc.Unity
{
    public static class UnityExtensions
    {
        /// <summary>
        /// Registers in unity container all IRepository and ICompoundKeyRepository resolutions.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configuration"></param>
        /// <param name="repositoryName"></param>
        /// <param name="lifetimeScopeTag">Accepts any MatchingScopeLifetimeTags scope enum tag</param>
        public static void RegisterSharpRepository(this IUnityContainer container, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            container.RegisterType(typeof(IRepository<>), new InjectionFactory((c, t, n) => RepositoryFactory.GetInstance(t.GetGenericArguments()[0], configuration, repositoryName)));
            container.RegisterType(typeof(IRepository<,>), new InjectionFactory((c, t, n) => RepositoryFactory.GetInstance(t.GetGenericArguments()[0], t.GetGenericArguments()[1], configuration, repositoryName)));
            container.RegisterType(typeof(ICompoundKeyRepository<,,>), new InjectionFactory((c, t, n) => RepositoryFactory.GetInstance(t.GetGenericArguments()[0], t.GetGenericArguments()[1], t.GetGenericArguments()[2], configuration, repositoryName)));
            container.RegisterType(typeof(ICompoundKeyRepository<,,,>), new InjectionFactory((c, t, n) => RepositoryFactory.GetInstance(t.GetGenericArguments()[0], t.GetGenericArguments()[1], t.GetGenericArguments()[2], t.GetGenericArguments()[3], configuration, repositoryName)));
            container.RegisterType(typeof(ICompoundKeyRepository<>), new InjectionFactory((c, t, n) => RepositoryFactory.GetInstance(t.GetGenericArguments()[0], configuration, repositoryName)));
        }
    }
}
