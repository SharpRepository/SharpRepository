using System;

namespace SharpRepository.Repository.Ioc
{
    public interface IRepositoryIocContainer
    {
        T GetInstance<T>();
        object GetInstance(Type type);
    }
}