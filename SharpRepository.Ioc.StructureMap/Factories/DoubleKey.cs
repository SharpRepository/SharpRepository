using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap.Building;
using StructureMap.Pipeline;
using System;
using System.Reflection;

namespace SharpRepository.Ioc.StructureMap.Factories
{
    public class RepositoryDoubleKeyInstanceFactory : Instance
    {
        protected string repositoryName;
        protected ISharpRepositoryConfiguration configuration;
        
        public RepositoryDoubleKeyInstanceFactory(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            this.configuration = configuration;
            this.repositoryName = repositoryName;
        }

        public override string Description {
            get {
                return "Repository factory for class with double compound key";
            }
        }

        public override Type ReturnedType
        {
            get
            {
                return typeof(ICompoundKeyRepository<,,>);
            }
        }

        public override IDependencySource ToDependencySource(Type pluginType)
        {
            throw new NotSupportedException();
        }

        public override Instance CloseType(Type[] types)
        {
            var instanceType = typeof(RepositoryInstance<,,>).MakeGenericType(types);

            if (configuration == null)
            {
                return null;
            }

            var ctor = instanceType.GetConstructor(new[] { typeof(ISharpRepositoryConfiguration), typeof(string)  });
            return ctor.Invoke(new object[] { configuration, repositoryName }) as Instance;
        }
    }

    public class RepositoryInstance<T, TKey, TKey2> : LambdaInstance<ICompoundKeyRepository<T, TKey, TKey2>> where T : class, new()
    {
        public RepositoryInstance(ISharpRepositoryConfiguration configuration, string repositoryName = null)
         : base(() => RepositoryFactory.GetInstance<T, TKey, TKey2>(configuration, repositoryName))
        {
        }
    }
}
