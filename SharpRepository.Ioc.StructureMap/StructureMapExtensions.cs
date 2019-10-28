using Microsoft.Extensions.Configuration;
using SharpRepository.Ioc.StructureMap.Factories;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap;
using StructureMap.Pipeline;

namespace SharpRepository.Ioc.StructureMap
{
    public static class StructureMapExtensions
    {

        /// <summary>
        /// Configures StructureMap container telling to resolve IRepository and ICompoundKeyRepository with the repository from configuration
        /// </summary>
        /// <param name="init"></param>
        /// <param name="configuration"></param>
        /// <param name="repositoryName">name of repository implementation in configuration, null tell to use default in configuration</param>
        /// <param name="lifecycle">StructureMap coping of variables default is Lifecycle.Transient</param>
        public static void ForRepositoriesUseSharpRepository(this ConfigurationExpression init, IConfigurationSection configurationSection, string repositoryName = null, ILifecycle lifeCycle = null)
        {
            if (configurationSection == null)
                throw new ConfigurationErrorsException("Configuration section not found.");

            var configuration = RepositoryFactory.BuildSharpRepositoryConfiguation(configurationSection);

            init.ForRepositoriesUseSharpRepository(configuration, repositoryName, lifeCycle);
        }


        /// <summary>
        /// Configures StructureMap container telling to resolve IRepository and ICompoundKeyRepository with the repository from configuration
        /// </summary>
        /// <param name="init"></param>
        /// <param name="configuration"></param>
        /// <param name="repositoryName">name of repository implementation in configuration, null tell to use default in configuration</param>
        /// <param name="lifecycle">StructureMap coping of variables default is Lifecycle.Transient</param>
        public static void ForRepositoriesUseSharpRepository(this ConfigurationExpression init, ISharpRepositoryConfiguration configuration, string repositoryName = null, ILifecycle lifeCycle = null)
        {
            if (lifeCycle == null)
            {
                lifeCycle = Lifecycles.Transient;
            }

            init.For(typeof(IRepository<>))
                .LifecycleIs(lifeCycle)
                .Use(new RepositoryNoKeyInstanceFactory(configuration, repositoryName));

            init.For(typeof(IRepository<,>))
                .LifecycleIs(lifeCycle)
                .Use(new RepositorySingleKeyInstanceFactory(configuration, repositoryName));

            init.For(typeof(ICompoundKeyRepository<,,>))
                .LifecycleIs(lifeCycle)
                .Use(new RepositoryDoubleKeyInstanceFactory(configuration, repositoryName));

            init.For(typeof(ICompoundKeyRepository<,,,>))
                .LifecycleIs(lifeCycle)
                .Use(new RepositoryTripleKeyInstanceFactory(configuration, repositoryName));

            init.For(typeof(ICompoundKeyRepository<>))
                .LifecycleIs(lifeCycle)
                .Use(new RepositoryCompoundKeyInstanceFactory(configuration, repositoryName));
        }
    }
}
