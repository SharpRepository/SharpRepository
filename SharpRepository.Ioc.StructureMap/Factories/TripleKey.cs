using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap.Building;
using StructureMap.Pipeline;
using System;

namespace SharpRepository.Ioc.StructureMap.Factories
{
    public class RepositoryTripleKeyInstanceFactory : Instance
    {
        protected string repositoryName;
        protected ISharpRepositoryConfiguration configuration;

        public RepositoryTripleKeyInstanceFactory(string repositoryName)
        {
            this.repositoryName = repositoryName;
        }

        public RepositoryTripleKeyInstanceFactory(ISharpRepositoryConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override string Description {
            get {
                return "Repository factory for class with triple compound key";
            }
        }

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
            
            if (this.configuration != null) { 
                return Activator.CreateInstance(instanceType,this.configuration, this.repositoryName) as Instance;
            } else {
                var ctor = instanceType.GetConstructor(new[] { typeof(string) });
                return ctor.Invoke(new object[] { this.repositoryName }) as Instance;
            }
        }
}

    public class RepositoryInstance<T, TKey, TKey2, TKey3> : LambdaInstance<ICompoundKeyRepository<T, TKey, TKey2, TKey3>> where T : class, new()
    {
        public RepositoryInstance(string repositoryName)
            : base(() => RepositoryFactory.GetInstance<T, TKey, TKey2, TKey3>( repositoryName))
        {
        }

        public RepositoryInstance(ISharpRepositoryConfiguration configuration)
         : base(() => RepositoryFactory.GetInstance<T, TKey, TKey2, TKey3>(configuration, null))
        {
        }
    }
}
