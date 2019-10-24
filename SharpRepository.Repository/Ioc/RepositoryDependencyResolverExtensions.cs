using System;

namespace SharpRepository.Repository.Ioc
{
    public static class RepositoryDependencyResolverExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            try
            {
                return (T)serviceProvider.GetService(typeof(T));
            }
            catch (Exception ex)
            {
                throw new RepositoryDependencyResolverException(typeof(T), ex);
            }
        }
    }
}
