using System;
using Autofac;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ioc.Autofac
{
    public static class AutofacExtensions
    {
        public static void RegisterSharpRepository(this ContainerBuilder container, string repositoryName = null)
        {
            throw new NotImplementedException();
        }

        public static void RegisterSharpRepository(this ContainerBuilder container, ISharpRepositoryConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
