using Ninject;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System.Reflection;

namespace SharpRepository.Ioc.Ninject
{
    public static class NinjectRepositoryExtensions
    {
        public static void BindSharpRepository(this IKernel kernel, ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            kernel.Bind(typeof (IRepository<>)).ToMethod(context =>
                {
                    var genericArgs = context.Request.Service.GetTypeInfo().GenericTypeArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], configuration, repositoryName);
                });

            kernel.Bind(typeof(IRepository<,>)).ToMethod(context =>
                {
                    var genericArgs = context.Request.Service.GetTypeInfo().GenericTypeArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], configuration, repositoryName);
                });

            kernel.Bind(typeof(ICompoundKeyRepository<,,>)).ToMethod(context =>
            {
                var genericArgs = context.Request.Service.GetTypeInfo().GenericTypeArguments;

                return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], genericArgs[2], configuration, repositoryName);
            });

            kernel.Bind(typeof(ICompoundKeyRepository<,,,>)).ToMethod(context =>
            {
                var genericArgs = context.Request.Service.GetTypeInfo().GenericTypeArguments;

                return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], genericArgs[2], genericArgs[2], configuration, repositoryName);
            });

            kernel.Bind(typeof(ICompoundKeyRepository<>)).ToMethod(context =>
            {
                var genericArgs = context.Request.Service.GetTypeInfo().GenericTypeArguments;

                return RepositoryFactory.GetInstance(genericArgs[0], configuration, repositoryName);
            });
        }
    }
}
