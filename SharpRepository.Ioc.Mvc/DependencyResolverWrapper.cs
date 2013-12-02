using System;
using System.Web.Mvc;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Mvc
{
    public class DependencyResolverWrapper : IRepositoryDependencyResolver
    {
        private readonly IDependencyResolver _resolver;

        public DependencyResolverWrapper(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public T Resolve<T>()
        {
            return _resolver.GetService<T>();
        }

        public object Resolve(Type type)
        {
            return _resolver.GetService(type);
        }
    }
}
