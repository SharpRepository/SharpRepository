using System;
using System.Web.Http;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Ioc.WebApi
{
    public class WebApiDependencyResolver : IRepositoryDependencyResolver
    {
        public T Resolve<T>()
        {
            return (T)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(T));
        }

        public object Resolve(Type type)
        {
            return GlobalConfiguration.Configuration.DependencyResolver.GetService(type);
        }
    }
}
