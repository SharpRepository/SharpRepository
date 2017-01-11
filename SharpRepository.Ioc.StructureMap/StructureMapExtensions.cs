using SharpRepository.Ioc.StructureMap.Factories;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap;
using StructureMap.Pipeline;

namespace SharpRepository.Ioc.StructureMap
{
    public static class StructureMapExtensions
    {
        public static void ForRepositoriesUseSharpRepository(this ConfigurationExpression initialization, string repositoryName = null)
        {
            initialization.Scan(scan => scan.IncludeNamespaceContainingType<IAmInRepository>());

            initialization.For(typeof(IRepository<>)).Use(new RepositoryNoKeyInstanceFactory(repositoryName));

            initialization.For(typeof(IRepository<,>)).Use(new RepositorySingleKeyInstanceFactory(repositoryName));

            initialization.For(typeof(ICompoundKeyRepository<,,>)).Use(new RepositoryDoubleKeyInstanceFactory(repositoryName));

            initialization.For(typeof(ICompoundKeyRepository<,,,>)).Use(new RepositoryTripleKeyInstanceFactory(repositoryName));

            initialization.For(typeof(ICompoundKeyRepository<>)).Use(new RepositoryCompoundKeyInstanceFactory(repositoryName));
        }

        public static void ForRepositoriesUseSharpRepository(this ConfigurationExpression initialization, ISharpRepositoryConfiguration configuration)
        {
            initialization.Scan(scan => scan.IncludeNamespaceContainingType<IAmInRepository>());

            initialization.For(typeof(IRepository<>)).Use(new RepositoryNoKeyInstanceFactory(configuration));

            initialization.For(typeof(IRepository<,>)).Use(new RepositorySingleKeyInstanceFactory(configuration));

            initialization.For(typeof(ICompoundKeyRepository<,,>)).Use(new RepositoryDoubleKeyInstanceFactory(configuration));

            initialization.For(typeof(ICompoundKeyRepository<,,,>)).Use(new RepositoryTripleKeyInstanceFactory(configuration));

            initialization.For(typeof(ICompoundKeyRepository<>)).Use(new RepositoryCompoundKeyInstanceFactory(configuration));
        }
    }
}
