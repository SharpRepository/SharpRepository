using System;
using SharpRepository.Repository.Ioc;
using Unity;

namespace SharpRepository.Ioc.Unity
{
    public class UnityRepositoryDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly IUnityContainer _container;
        public UnityRepositoryDependencyResolver(IUnityContainer container)
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