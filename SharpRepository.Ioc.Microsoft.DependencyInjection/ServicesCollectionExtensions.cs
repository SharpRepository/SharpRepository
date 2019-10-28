using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpRepository.Ioc.StructureMap;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;
using StructureMap;
using System;

namespace SharpRepository.Ioc.Microsoft.DependencyInjection
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceProvider UseSharpRepository(this IServiceCollection services, IConfigurationSection configurationSection, string repositoryName = null)
        {
            if (configurationSection == null)
                throw new ConfigurationErrorsException("Configuration section not found.");

            var sharpRepoConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(configurationSection);

            return services.UseSharpRepository(sharpRepoConfig, repositoryName);
        }
        
        public static IServiceProvider UseSharpRepository(this IServiceCollection services, ISharpRepositoryConfiguration sharpRepoConfig, string repositoryName = null)
        {
            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.AssembliesAndExecutablesFromApplicationBaseDirectory();
                    x.LookForRegistries();
                });

                config.ForRepositoriesUseSharpRepository(sharpRepoConfig, repositoryName);
                config.Populate(services);
            });

            var resolver = container.GetInstance<IServiceProvider>();

            RepositoryDependencyResolver.SetDependencyResolver(resolver);

            // Finally, make sure we return an IServiceProvider. This makes
            // ASP.NET use the StructureMap container to resolve its services.
            return resolver;
        }
    }
}
