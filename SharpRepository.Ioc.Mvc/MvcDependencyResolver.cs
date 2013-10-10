using System;
using System.Web.Mvc;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.Mvc
{
    public class MvcDependencyResolver : IRepositoryDependencyResolver
    {
        public T Resolve<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }

        public object Resolve(Type type)
        {
            return DependencyResolver.Current.GetService(type);
        }
    }
}
