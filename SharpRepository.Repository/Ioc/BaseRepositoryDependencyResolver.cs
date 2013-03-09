using System;

namespace SharpRepository.Repository.Ioc
{
    public abstract class BaseRepositoryDependencyResolver : IRepositoryDependencyResolver
    {
        public T Resolve<T>()
        {
            try
            {
                return ResolveInstance<T>();
            }
            catch (Exception ex)
            {
                throw new RepositoryDependencyResolverException(typeof(T), ex);
            }
        }

        public object Resolve(Type type)
        {
            try
            {
                return ResolveInstance(type);
            }
            catch (Exception ex)
            {
                throw new RepositoryDependencyResolverException(type, ex);
            }
        }

        protected abstract T ResolveInstance<T>();
        protected abstract object ResolveInstance(Type type);
    }
}
