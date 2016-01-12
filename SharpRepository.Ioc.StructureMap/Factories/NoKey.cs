using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap.Building;
using StructureMap.Pipeline;
using System;

namespace SharpRepository.Ioc.StructureMap.Factories
{
    public class RepositoryNoKeyInstanceFactory : Instance
    {
        protected string repositoryName;
        protected ISharpRepositoryConfiguration configuration;

        public RepositoryNoKeyInstanceFactory(string repositoryName)
        {
            this.repositoryName = repositoryName;
        }

        public RepositoryNoKeyInstanceFactory(ISharpRepositoryConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override string Description {
            get {
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
            
            if (this.configuration != null) { 
                return Activator.CreateInstance(instanceType,this.configuration, this.repositoryName) as Instance;
            } else {
                var ctor = instanceType.GetConstructor(new[] { typeof(string) });
                return ctor.Invoke(new object[] { instanceType, this.repositoryName }) as Instance;
            }
        }
    }
    public class RepositoryInstance<T> : LambdaInstance<IRepository<T>> where T : class
    {
        public RepositoryInstance(string repositoryName)
            : base(() => (IRepository<T>)RepositoryFactory.GetInstance(typeof(T), repositoryName))
        {
        }

        public RepositoryInstance(ISharpRepositoryConfiguration configuration)
         : base(() => (IRepository<T>)RepositoryFactory.GetInstance(typeof(T), configuration, null))
        {
        }
    }
}
