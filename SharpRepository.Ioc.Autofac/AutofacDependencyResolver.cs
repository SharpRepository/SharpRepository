using System;
using Autofac;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Autofac
{
    public class AutofacDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly IContainer _container;
        public AutofacDependencyResolver(IContainer container)
        {
            _container = container;
        }

        protected override T ResolveInstance<T>()
        {
            return _container.Resolve<T>();
        }

        protected override object ResolveInstance(Type type)
        {
            return _container.Resolve(type);
        }
    }
}
