using System;

namespace SharpRepository.Repository.Ioc
{
    public interface IRepositoryDependencyResolver
    {
        T GetInstance<T>();
        object GetInstance(Type type);
    }
}