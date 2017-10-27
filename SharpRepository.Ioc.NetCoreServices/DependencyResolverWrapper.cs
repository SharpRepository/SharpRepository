using System;
using SharpRepository.Repository.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace SharpRepository.Ioc.NetCoreServices
{
    public class DependencyResolverWrapper : IRepositoryDependencyResolver
    {
        private readonly IServiceProvider _resolver;

        public DependencyResolverWrapper(IServiceProvider resolver)
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
