using Microsoft.Extensions.Configuration;
using SharpRepository.Ioc.StructureMap;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;
using StructureMap;
using System;
using System.Web.Http;
using System.Web.Mvc;

namespace SharpRepository.Ioc.Mvc
{
    public static class MvcDependencyResolver
    {
        public static void ForRepositoriesUseSharpRepository(string jsonConfigurationFileName, string sharpRepositoryConfigurationSectionName, string repoisitoryName = null)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile(jsonConfigurationFileName)
              .Build();

            var section = config.GetSection(sharpRepositoryConfigurationSectionName);
            var sharpConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(section);
            
            ForRepositoriesUseSharpRepository(sharpConfig, repoisitoryName);
        }

        public static void ForRepositoriesUseSharpRepository(ISharpRepositoryConfiguration sharpConfig, string repositoryName = null)
        {
            var container = new Container(c =>
            {
                c.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.AssembliesAndExecutablesFromApplicationBaseDirectory();
                    s.LookForRegistries();
                    s.WithDefaultConventions();
                });

                c.ForRepositoriesUseSharpRepository(sharpConfig, repositoryName);
            });

            var dependencyResolver = new StructureMapDependencyResolver(container);
            DependencyResolver.SetResolver(dependencyResolver);
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;

            RepositoryDependencyResolver.SetDependencyResolver(new StructureMapRepositoryDependencyResolver(container));
        }
    }
}
