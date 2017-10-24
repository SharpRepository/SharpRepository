using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap.Building;
using StructureMap.Pipeline;
using System;
using System.Reflection;

namespace SharpRepository.Ioc.StructureMap.Factories
{
    public class RepositoryTripleKeyInstanceFactory : Instance
    {
        protected string repositoryName;
        protected ISharpRepositoryConfiguration configuration;
        
        public RepositoryTripleKeyInstanceFactory(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            this.configuration = configuration;
            this.repositoryName = repositoryName;
        }

        public override string Description => "Repository factory for class with triple compound key";

        public override Type ReturnedType
        {
            get
            {
                return typeof(ICompoundKeyRepository<,,,>);
            }
        }

        public override IDependencySource ToDependencySource(Type pluginType)
        {
            throw new NotSupportedException();
        }

        public override Instance CloseType(Type[] types)
        {
            var instanceType = typeof(RepositoryInstance<,,,>).MakeGenericType(types);

            if (configuration == null) {
                return null;
            }

             var ctor = instanceType.GetConstructor(new[] { typeof(ISharpRepositoryConfiguration), typeof(string) });
             return ctor.Invoke(new object[] { configuration, repositoryName }) as Instance;
        }
}

    public class RepositoryInstance<T, TKey, TKey2, TKey3> : LambdaInstance<ICompoundKeyRepository<T, TKey, TKey2, TKey3>> where T : class, new()
    {
        public RepositoryInstance(ISharpRepositoryConfiguration configuration, string repositoryName = null)
         : base(() => RepositoryFactory.GetInstance<T, TKey, TKey2, TKey3>(configuration, repositoryName))
        {
        }
    }
}
