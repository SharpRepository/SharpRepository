using System;

namespace SharpRepository.Repository.Ioc
{
    public interface IRepositoryDependencyResolver
    {
        T Resolve<T>();
        object Resolve(Type type);
    }

    public static class RepositoryDependencyResolver
    {
        static RepositoryDependencyResolver()
        {
            Current = null;
        }

        public static IRepositoryDependencyResolver Current { get; private set; }

        public static void SetDependencyResolver(IRepositoryDependencyResolver container)
        {
            Current = container;
        }
    }
}
