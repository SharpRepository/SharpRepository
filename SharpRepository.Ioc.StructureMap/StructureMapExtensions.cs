using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap;
using StructureMap.Pipeline;

namespace SharpRepository.Ioc.StructureMap
{
    public static class StructureMapExtensions
    {
        public static LambdaInstance<object> ForRepositoriesUseSharpRepository(this IInitializationExpression initialization, string repositoryName = null)
        {
            initialization.Scan(scan => scan.IncludeNamespaceContainingType<IAmInRepository>());

            initialization.For(typeof(IRepository<>))
                                 .Use(context =>
                                 {
                                     var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();
                                     var entityType = genericArgs[0];

                                     return RepositoryFactory.GetInstance(entityType, repositoryName);
                                 }
                );

            return initialization.For(typeof(IRepository<,>))
                                 .Use(context =>
                                 {
                                     var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();
                                     var entityType = genericArgs[0];
                                     var keyType = genericArgs[1];

                                     return RepositoryFactory.GetInstance(entityType, keyType, repositoryName);
                                 }
                );
        }

        public static LambdaInstance<object> ForRepositoriesUseSharpRepository(this IInitializationExpression initialization, ISharpRepositoryConfiguration configuration)
        {
            initialization.Scan(scan => scan.IncludeNamespaceContainingType<IAmInRepository>());

            initialization.For(typeof(IRepository<>))
                                 .Use(context =>
                                 {
                                     var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();
                                     var entityType = genericArgs[0];

                                     return RepositoryFactory.GetInstance(entityType, configuration);
                                 }
                );

            return initialization.For(typeof(IRepository<,>))
                                 .Use(context =>
                                 {
                                     var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();
                                     var entityType = genericArgs[0];
                                     var keyType = genericArgs[1];

                                     return RepositoryFactory.GetInstance(entityType, keyType, configuration);
                                 }
                );
        }
    }
}
