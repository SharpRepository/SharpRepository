using System;
using Autofac;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Autofac
{
    public class AutofacRepositoryDependencyResolver : IServiceProvider
    {
        private readonly IContainer _container;
        public AutofacRepositoryDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
    }
}