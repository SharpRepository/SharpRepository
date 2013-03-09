using System;
using SharpRepository.Repository.Ioc;
using StructureMap;

namespace SharpRepository.Ioc.StructureMap
{
    // do we need a way to pass in the Container?  Would that be in some global configuration file run in a bootstrapper or global.asax?
    public class Factory : IIocFactory
    {
        private readonly IContainer _container;

        public Factory(IContainer container)
        {
            _container = container;
        }

        public T CreateInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public object CreateInstance(Type type)
        {
            return _container.GetInstance(type);
        }
    }
}
