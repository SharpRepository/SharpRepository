using SharpRepository.Repository.Ioc;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRepository.Ioc.SimpleInjector
{
    public class SimpleInjectorDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly Container _container;
        public SimpleInjectorDependencyResolver(Container container)
        {
            _container = container;
        }

        protected override T ResolveInstance<T>() 
        {
            return (T)_container.GetInstance(typeof(T));
        }

        protected override object ResolveInstance(Type type)
        {
            return _container.GetInstance(type);
        }

        // may not operate correctly if T is a primitive type
        protected T ResolveInstanceInternal<T>() where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}
