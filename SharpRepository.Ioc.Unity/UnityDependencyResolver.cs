using System;
using Microsoft.Practices.Unity;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Unity
{
    public class UnityDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly IUnityContainer _container;
        public UnityDependencyResolver(IUnityContainer container)
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
