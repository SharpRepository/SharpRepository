using System;
using Castle.Windsor;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Windsor
{
    public class WindsorDependencyResolver: BaseRepositoryDependencyResolver
    {
        private readonly IWindsorContainer _container;
        public WindsorDependencyResolver(IWindsorContainer container)
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
