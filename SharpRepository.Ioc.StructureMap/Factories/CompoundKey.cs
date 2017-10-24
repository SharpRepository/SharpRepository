using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap.Building;
using StructureMap.Pipeline;
using System;
using System.Reflection;

namespace SharpRepository.Ioc.StructureMap.Factories
{
    public class RepositoryCompoundKeyInstanceFactory : Instance
    {
        protected string repositoryName;
        protected ISharpRepositoryConfiguration configuration;
        
        public RepositoryCompoundKeyInstanceFactory(ISharpRepositoryConfiguration configuration, string repositoryName = null)
        {
            this.configuration = configuration;
            this.repositoryName = repositoryName;
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

            if (configuration == null) {
                return null;
            }

            var ctor = instanceType.GetConstructor(new[] { typeof(ISharpRepositoryConfiguration), typeof(string) });
            return ctor.Invoke(new object[] { configuration, repositoryName }) as Instance;
            
        }
    }

    public class RepositoryCompoundKeyInstance<T> : LambdaInstance<ICompoundKeyRepository<T>> where T : class, new()
    {
        public RepositoryCompoundKeyInstance(ISharpRepositoryConfiguration configuration, string repositoryName = null)
         : base(() => RepositoryFactory.GetCompoundKeyInstance<T>(configuration, repositoryName))
        {
        }
    }
}
