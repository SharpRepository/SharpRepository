using SharpRepository.Ioc.StructureMap.Factories;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap;

namespace SharpRepository.Ioc.StructureMap
{
    public static class StructureMapExtensions
    {
        public static void ForRepositoriesUseSharpRepository(this ConfigurationExpression initialization, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            initialization.Scan(scan => { 
                scan.IncludeNamespaceContainingType<IAmInRepository>();
                scan.WithDefaultConventions();
            });
            
            initialization.For(typeof(IRepository<>)).Singleton();
            initialization.For(typeof(IRepository<,>)).Singleton();
            initialization.For(typeof(ICompoundKeyRepository<,,>)).Singleton();
            initialization.For(typeof(ICompoundKeyRepository<,,,>)).Singleton();
            initialization.For(typeof(ICompoundKeyRepository<>)).Singleton();

            initialization.For(typeof(IRepository<>)).Use(new RepositoryNoKeyInstanceFactory(configuration, repositoryName));
            initialization.For(typeof(IRepository<,>)).Use(new RepositorySingleKeyInstanceFactory(configuration, repositoryName));
            initialization.For(typeof(ICompoundKeyRepository<,,>)).Use(new RepositoryDoubleKeyInstanceFactory(configuration, repositoryName));
            initialization.For(typeof(ICompoundKeyRepository<,,,>)).Use(new RepositoryTripleKeyInstanceFactory(configuration, repositoryName));
            initialization.For(typeof(ICompoundKeyRepository<>)).Use(new RepositoryCompoundKeyInstanceFactory(configuration, repositoryName));
        }
    }
}
