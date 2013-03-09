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

                                     return RepositoryFactory.GetInstance(genericArgs[0], repositoryName);
                                 }
                );

            initialization.For(typeof(IRepository<,>))
                                 .Use(context =>
                                 {
                                     var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();

                                     return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], repositoryName);
                                 }
                );

            return initialization.For(typeof(ICompoundKeyRepository<,,>))
                                .Use(context =>
                                 {
                                     var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();

                                     return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], genericArgs[2], repositoryName);
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

                                     return RepositoryFactory.GetInstance(genericArgs[0], configuration);
                                 }
                );

            initialization.For(typeof(IRepository<,>))
                                 .Use(context =>
                                 {
                                     var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();

                                     return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], configuration);
                                 }
                );

            return initialization.For(typeof(ICompoundKeyRepository<,,>))
                                .Use(context =>
                                {
                                    var genericArgs = context.BuildStack.Current.RequestedType.GetGenericArguments();

                                    return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], genericArgs[2], configuration);
                                }
                );
        }
    }
}
