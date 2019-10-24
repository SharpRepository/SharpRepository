using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;

#if NETSTANDARD2_0
using Autofac.Extensions.DependencyInjection;
#endif

namespace SharpRepository.Ioc.Autofac
{
    public static class AutofacExtensions
    {
        /// <summary>
        /// Registers in autofac container all IRepository and ICompoundKeyRepository resolutions.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configuration"></param>
        /// <param name="repositoryName"></param>
        /// <param name="lifetimeScopeTag">Accepts any MatchingScopeLifetimeTags scope enum tag</param>
        public static void RegisterSharpRepository(this ContainerBuilder containerBuilder, ISharpRepositoryConfiguration configuration, string repositoryName = null, params object[] lifetimeScopeTag)
        {
            containerBuilder.RegisterSource(new RepositoryRegistrationSource(configuration, repositoryName, lifetimeScopeTag));

            //var containerCopyBuilder = new ContainerBuilder();

//#if NETSTANDARD2_0
//            RepositoryDependencyResolver.SetDependencyResolver(new AutofacServiceProvider(containerBuilder.Build()));
//#else
//            RepositoryDependencyResolver.SetDependencyResolver(new AutofacRepositoryDependencyResolver(containerBuilder.Build()));
//#endif
        }
    }
}
