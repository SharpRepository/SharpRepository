using System;

namespace SharpRepository.Repository.Ioc
{
    public interface ISharpRepositoryIocContainer
    {
        T GetInstance<T>();
        object GetInstance(Type type);
    }

    public static class SharpRepositoryIocContainer
    {
        static SharpRepositoryIocContainer()
        {
            Current = null;
        }

        public static ISharpRepositoryIocContainer Current { get; private set; }

        public static void SetIocContainer(ISharpRepositoryIocContainer container)
        {
            Current = container;
        }
    }
}
