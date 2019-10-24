using Microsoft.Extensions.Configuration;
using SharpRepository.Ioc.StructureMap;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Web.Http;
using System.Web.Mvc;

namespace SharpRepository.Ioc.Mvc
{
    public static class MvcDependencyResolver
    {
        /// <summary>
        /// Configures DependencyResolver with configured SharpRepository as implementation of IRepository and ICompoundRepository instances
        /// </summary>
        /// <param name="jsonConfigurationFileName"></param>
        /// <param name="sharpRepositoryConfigurationSectionName"></param>
        /// <param name="repositoryName">name of repository implementation in configuration, null tell to use default in configuration</param>
        /// <param name="lifecycle">StructureMap coping of variables default is Lifecycle.Transient</param>
        public static void ForRepositoriesUseSharpRepository(string jsonConfigurationFileName, string sharpRepositoryConfigurationSectionName, string repoisitoryName = null, ILifecycle lifecycle = null)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile(jsonConfigurationFileName)
              .Build();

            var section = config.GetSection(sharpRepositoryConfigurationSectionName);
            var sharpConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(section);
            
            ForRepositoriesUseSharpRepository(sharpConfig, repoisitoryName, lifecycle);
        }

        /// <summary>
        /// Configures DependencyResolver with configured SharpRepository as implementation of IRepository and ICompoundRepository instances
        /// </summary>
        /// <param name="sharpConfig"></param>
        /// <param name="repositoryName">name of repository implementation in configuration, null tell to use default in configuration</param>
        /// <param name="lifecycle">StructureMap coping of variables default is Lifecycle.Transient</param>
        public static void ForRepositoriesUseSharpRepository(ISharpRepositoryConfiguration sharpConfig, string repositoryName = null, ILifecycle lifecycle = null)
        {
            var container = new Container(c =>
            {
                c.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.AssembliesAndExecutablesFromApplicationBaseDirectory();
                    s.LookForRegistries();
                });

                c.ForRepositoriesUseSharpRepository(sharpConfig, repositoryName, lifecycle);
            });
            
            var dependencyResolver = new StructureMapDependencyResolver(container);
            DependencyResolver.SetResolver(dependencyResolver);
            GlobalConfiguration.Configuration.DependencyResolver = dependencyResolver;
            RepositoryDependencyResolver.SetDependencyResolver(dependencyResolver);
        }
    }
}
