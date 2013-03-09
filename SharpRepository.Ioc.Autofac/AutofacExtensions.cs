using Autofac;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ioc.Autofac
{
    public static class AutofacExtensions
    {
        public static void RegisterSharpRepository(this ContainerBuilder container, string repositoryName = null)
        {

        }

        public static void RegisterSharpRepository(this ContainerBuilder container, ISharpRepositoryConfiguration configuration)
        {

        }
    }
}
