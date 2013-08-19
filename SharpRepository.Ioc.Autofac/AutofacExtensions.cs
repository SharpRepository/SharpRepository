using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using SharpRepository.Repository;

namespace SharpRepository.Ioc.Autofac
{
    public static class AutofacExtensions
    {
        public static void RegisterSharpRepository(this ContainerBuilder container, string repositoryName = null)
        {
            container.RegisterGeneric(typeof (ConfigurationBasedRepository<,>))
                .As(typeof (IRepository<,>))
                .WithParameter("repositoryName", repositoryName)
                ;
        }

        public static void RegisterSharpRepository(this ContainerBuilder container, string configSection, string repositoryName)
        {
            container.RegisterGeneric(typeof (ConfigurationBasedRepository<,>))
                     .As(typeof (IRepository<,>))
                     .WithParameters(new List<Parameter>()
                                         {
                                             new NamedParameter("configSection", configSection),
                                             new NamedParameter("repositoryName", repositoryName)
                                         });
        }
    }
}
