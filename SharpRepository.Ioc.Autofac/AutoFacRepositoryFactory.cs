using System;
using System.Collections.Generic;
using System.Text;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ioc.Autofac
{
    class AutofacRepositoryFactory<T> : IAutofacRepositoryFactory<T> where T : class, new()
    {
        private ISharpRepositoryConfiguration Configuration;

        private string RepositoryName;

        public AutofacRepositoryFactory(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            Configuration = configuration;
            RepositoryName = repositoryName;
        }

        public IRepository<T, int> CreateRepository()
        {
            return RepositoryFactory.GetInstance<T>(Configuration, RepositoryName);
        }
    }
}
