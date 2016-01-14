using System;
using SharpRepository.Repository.Ioc;
using StructureMap;

namespace SharpRepository.Ioc.StructureMap
{
    public class StructureMapRepositoryDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly IContainer _container;
        public StructureMapRepositoryDependencyResolver(IContainer container)
        {
            _container = container;
        }

        protected override T ResolveInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        protected override object ResolveInstance(Type type)
        {
            return _container.GetInstance(type);
        }
    }
}
