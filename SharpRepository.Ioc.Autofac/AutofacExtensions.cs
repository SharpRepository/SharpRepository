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
        public static void RegisterSharpRepository(this ContainerBuilder container, ISharpRepositoryConfiguration configuration, string repositoryName = null, params object[] lifetimeScopeTag)
        {
            container.RegisterSource(new RepositoryRegistrationSource(configuration, repositoryName, lifetimeScopeTag));
        }
    }
}
