using System;
using Ninject;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Ninject
{
    public class NinjectIocContainer : ISharpRepositoryIocContainer
    {
        private readonly IKernel _kernel;
        public NinjectIocContainer(IKernel kernel)
        {
            _kernel = kernel;
        }
        public T GetInstance<T>()
        {
            return _kernel.Get<T>();
        }

        public object GetInstance(Type type)
        {
            return _kernel.Get(type);
        }
    }
}
