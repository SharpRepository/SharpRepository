using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;

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
        }

        /// <summary>
        /// Registers in autofac container all IRepository and ICompoundKeyRepository resolutions.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configuration"></param>
        /// <param name="repositoryName"></param>
        /// <param name="lifetimeScopeTag">Accepts any MatchingScopeLifetimeTags scope enum tag</param>
        public static void RegisterSharpRepository(this ContainerBuilder containerBuilder, IConfigurationSection configurationSection, string repositoryName = null, params object[] lifetimeScopeTag)
        {
            if (configurationSection == null)
                throw new ConfigurationErrorsException("Configuration section not found.");

            var configuration = RepositoryFactory.BuildSharpRepositoryConfiguation(configurationSection);

            containerBuilder.RegisterSharpRepository(configuration, repositoryName, lifetimeScopeTag);
        }
    }
}
