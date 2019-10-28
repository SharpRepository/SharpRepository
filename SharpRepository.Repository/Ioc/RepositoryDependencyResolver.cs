using System;

namespace SharpRepository.Repository.Ioc
{
    public static class RepositoryDependencyResolver
    {
        static RepositoryDependencyResolver()
        {
            Current = null;
        }

        public static IServiceProvider Current { get; private set; }

        public static void SetDependencyResolver(IServiceProvider resolver)
        {
            Current = resolver;
        }
    }
}
