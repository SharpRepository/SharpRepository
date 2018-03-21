using SharpRepository.Repository.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SharpRepository.Samples.MVC5
{
    public class CustomAutofacRepositoryDependencyResolver : BaseRepositoryDependencyResolver
    {
        private readonly IDependencyResolver _resolver;

        public CustomAutofacRepositoryDependencyResolver(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        protected override T ResolveInstance<T>()
        {
            return _resolver.GetService<T>();
        }

        protected override object ResolveInstance(Type type)
        {
            return _resolver.GetService(type);
        }
    }
}