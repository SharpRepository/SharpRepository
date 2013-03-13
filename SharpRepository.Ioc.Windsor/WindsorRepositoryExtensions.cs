using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ioc.Windsor
{
    public static class WindsorRepositoryExtensions
    {
        public static void RegisterSharpRepository(this IWindsorContainer container, string repositoryName = null)
        {
            container.Register(Component.For(typeof(IRepository<>)).UsingFactoryMethod((c, t) =>
                {
                    var genericArgs = t.GenericArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], repositoryName);
                }));

            container.Register(Component.For(typeof(IRepository<>)).UsingFactoryMethod((c, t) =>
                {
                    var genericArgs = t.GenericArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], repositoryName);
                }));
        }

        public static void RegisterSharpRepository(this IWindsorContainer container, ISharpRepositoryConfiguration configuration)
        {
            container.Register(Component.For(typeof(IRepository<>)).UsingFactoryMethod((c, t) =>
                {
                    var genericArgs = t.GenericArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], configuration);
                }));

            container.Register(Component.For(typeof(IRepository<>)).UsingFactoryMethod((c, t) =>
                {
                    var genericArgs = t.GenericArguments;

                    return RepositoryFactory.GetInstance(genericArgs[0], genericArgs[1], configuration);
                }));
        }
    }
}
