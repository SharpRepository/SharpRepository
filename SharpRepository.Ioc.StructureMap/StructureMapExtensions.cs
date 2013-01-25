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

            initialization.For(typeof(ICompoundKeyRepository<,,>))
                                 .Use(context =>
                                 {
                                     var entityType = context.BuildStack.Current.RequestedType.GetGenericArguments()[0];
                                     var keyType = context.BuildStack.Current.RequestedType.GetGenericArguments()[1];
                                     var key2Type = context.BuildStack.Current.RequestedType.GetGenericArguments()[2];

                                     return RepositoryFactory.GetInstance(entityType, keyType, key2Type, repositoryName);
                                 }
                );

            return initialization.For(typeof(IRepository<,>))
                                 .Use(context =>
                                 {
                                     var entityType = context.BuildStack.Current.RequestedType.GetGenericArguments()[0];
                                     var keyType = context.BuildStack.Current.RequestedType.GetGenericArguments()[1];

                                     return RepositoryFactory.GetInstance(entityType, keyType, repositoryName);
                                 }
                );
        }

        public static LambdaInstance<object> ForRepositoriesUseSharpRepository(this IInitializationExpression initialization, ISharpRepositoryConfiguration configuration)
        {
            initialization.Scan(scan => scan.IncludeNamespaceContainingType<IAmInRepository>());

            initialization.For(typeof(ICompoundKeyRepository<,,>))
                                 .Use(context =>
                                 {
                                     var entityType = context.BuildStack.Current.RequestedType.GetGenericArguments()[0];
                                     var keyType = context.BuildStack.Current.RequestedType.GetGenericArguments()[1];
                                     var key2Type = context.BuildStack.Current.RequestedType.GetGenericArguments()[2];

                                     return RepositoryFactory.GetInstance(entityType, keyType, key2Type, configuration);
                                 }
                );

            return initialization.For(typeof(IRepository<,>))
                                 .Use(context =>
                                 {
                                     var entityType = context.BuildStack.Current.RequestedType.GetGenericArguments()[0];
                                     var keyType = context.BuildStack.Current.RequestedType.GetGenericArguments()[1];

                                     return RepositoryFactory.GetInstance(entityType, keyType, configuration);
                                 }
                );
        }
    }
}
