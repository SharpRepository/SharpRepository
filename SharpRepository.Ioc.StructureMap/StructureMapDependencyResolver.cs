using System;
using SharpRepository.Repository.Ioc;
using StructureMap;

namespace SharpRepository.Ioc.StructureMap
{
    public class StructureMapDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly IContainer _container;
        public StructureMapDependencyResolver(IContainer container)
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

    public class TestUsage
    {
        public TestUsage()
        {
            RepositoryDependencyResolver.SetDependencyResolver(new StructureMapDependencyResolver(ObjectFactory.Container));
        }
    }
}
