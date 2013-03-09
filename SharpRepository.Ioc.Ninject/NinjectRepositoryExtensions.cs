using Ninject;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ioc.Ninject
{
    public static class NinjectRepositoryExtensions
    {
        public static void BindSharpRepository(this IKernel kernel, string repositoryName = null)
        {
            kernel.Bind(typeof (IRepository<>)).ToMethod(context =>
                {
                    var genericArgs = context.Request.ParentRequest.Service.GetGenericArguments();

                    return RepositoryFactory.GetInstance(genericArgs[0], repositoryName);
                });

            kernel.Bind(typeof(IRepository<,>)).ToMethod(context =>
                {
                    var genericArgs = context.Request.ParentRequest.Service.GetGenericArguments();

                    return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], repositoryName);
                });
        }

        public static void BindSharpRepository(this IKernel kernel, ISharpRepositoryConfiguration configuration)
        {
            kernel.Bind(typeof (IRepository<>)).ToMethod(context =>
                {
                    var genericArgs = context.Request.ParentRequest.Service.GetGenericArguments();

                    return RepositoryFactory.GetInstance(genericArgs[0], configuration);
                });

            kernel.Bind(typeof(IRepository<,>)).ToMethod(context =>
                {
                    var genericArgs = context.Request.ParentRequest.Service.GetGenericArguments();

                    return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], configuration);
                });
        }
    }
}
