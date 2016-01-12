using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap.Building;
using StructureMap.Pipeline;
using System;

namespace SharpRepository.Ioc.StructureMap.Factories
{
    public class RepositoryCompoundKeyInstanceFactory : Instance
    {
        protected string repositoryName;
        protected ISharpRepositoryConfiguration configuration;

        public RepositoryCompoundKeyInstanceFactory(string repositoryName)
        {
            this.repositoryName = repositoryName;
        }

        public RepositoryCompoundKeyInstanceFactory(ISharpRepositoryConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override string Description {
            get {
                return "Repository factory for class with compound key";
            }
        }

        public override Type ReturnedType
        {
            get
            {
                return typeof(ICompoundKeyRepository<>);
            }
        }

        public override IDependencySource ToDependencySource(Type pluginType)
        {
            throw new NotSupportedException();
        }

        public override Instance CloseType(Type[] types)
        {
            var instanceType = typeof(RepositoryCompoundKeyInstance<>).MakeGenericType(types);
            
            if (this.configuration != null) { 
                return Activator.CreateInstance(instanceType,this.configuration, this.repositoryName) as Instance;
            } else {
                var ctor = instanceType.GetConstructor(new[] { typeof(string) });
                return ctor.Invoke(new object[] { instanceType, this.repositoryName }) as Instance;
            }
        }
    }

    public class RepositoryCompoundKeyInstance<T> : LambdaInstance<ICompoundKeyRepository<T>> where T : class, new()
    {
        public RepositoryCompoundKeyInstance(string repositoryName)
            : base(() => RepositoryFactory.GetCompoundKeyInstance<T>( repositoryName))
        {
        }

        public RepositoryCompoundKeyInstance(ISharpRepositoryConfiguration configuration)
         : base(() => RepositoryFactory.GetCompoundKeyInstance<T>(configuration, null))
        {
        }
    }
}
