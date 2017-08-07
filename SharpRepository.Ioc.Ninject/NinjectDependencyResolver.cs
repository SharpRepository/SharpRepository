using System;
using Ninject;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Ninject
{
    public class NinjectDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly IReadOnlyKernel _kernel;
        public NinjectDependencyResolver(IReadOnlyKernel kernel)
        {
            _kernel = kernel;
        }
        protected override T ResolveInstance<T>()
        {
            return _kernel.Get<T>();
        }

        protected override object ResolveInstance(Type type)
        {
            return _kernel.Get(type);
        }
    }
}
