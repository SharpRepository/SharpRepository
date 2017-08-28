using Microsoft.Extensions.Configuration;
using SharpRepository.Ioc.StructureMap;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap;
using StructureMap.Graph;
using System;

namespace SharpRepository.Benchmarks.Configuration
{
    public static class Bootstrapper
    {
        public static Container Run()
        {
            var container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                var config = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();

                var sectionName = "sharpRepository";

                IConfigurationSection sharpRepoSection = config.GetSection(sectionName);

                if (sharpRepoSection == null)
                    throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");

                var sharpRepoConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(sharpRepoSection);

                x.ForRepositoriesUseSharpRepository(sharpRepoConfig);
            });

            return container;
        }
    }
}
