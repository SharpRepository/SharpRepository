using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap.Building;
using StructureMap.Pipeline;
using System;
using System.Reflection;

namespace SharpRepository.Ioc.StructureMap.Factories
{
    public class RepositorySingleKeyInstanceFactory : Instance
    {
        protected string repositoryName;
        protected ISharpRepositoryConfiguration configuration;

        public RepositorySingleKeyInstanceFactory(string repositoryName)
        {
            this.repositoryName = repositoryName;
        }

        public RepositorySingleKeyInstanceFactory(ISharpRepositoryConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override string Description
        {
            get
            {
                return "Repository factory for class with no key";
            }
        }

        public override Type ReturnedType
        {
            get
            {
                return typeof(IRepository<,>);
            }
        }

        public override IDependencySource ToDependencySource(Type pluginType)
        {
            throw new NotSupportedException();
        }

        public override Instance CloseType(Type[] types)
        {
            var instanceType = typeof(RepositoryInstance<,>).MakeGenericType(types);

            if (this.configuration != null)
            {
                var ctor = instanceType.GetConstructor(new[] { typeof(ISharpRepositoryConfiguration) });
                return ctor.Invoke(new object[] { this.configuration }) as Instance;
            }
            else {
                var ctor = instanceType.GetConstructor(new[] { typeof(string) });
                return ctor.Invoke(new object[] { this.repositoryName }) as Instance;
            }
        }
    }

    public class RepositoryInstance<T, TKey> : LambdaInstance<IRepository<T, TKey>> where T : class, new()
    {
        public RepositoryInstance(string repositoryName)
            : base(() => RepositoryFactory.GetInstance<T, TKey>(repositoryName))
        {
        }

        public RepositoryInstance(ISharpRepositoryConfiguration configuration)
         : base(() => RepositoryFactory.GetInstance<T, TKey>(configuration, null))
        {
        }
    }
}
