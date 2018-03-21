using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpRepository.Ioc.Autofac
{
    interface IAutofacRepositoryFactory<T> where T : class
    {
        IRepository<T, int> CreateRepository();
    }
}
