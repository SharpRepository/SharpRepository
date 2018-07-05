using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

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
        public static void RegisterSharpRepository(this ContainerBuilder container, ISharpRepositoryConfiguration configuration, string repositoryName = null, params object[] lifetimeScopeTag)
        {
            container.RegisterSource(new RepositoryRegistrationSource(configuration, repositoryName, lifetimeScopeTag));
        }
    }
}
