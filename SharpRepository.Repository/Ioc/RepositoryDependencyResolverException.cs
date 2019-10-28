using System;

namespace SharpRepository.Repository.Ioc
{
    public class RepositoryDependencyResolverException : Exception
    {
        public Type DependencyType { get; internal set; }
        public IServiceProvider DependencyResolver { get; internal set; }

        public RepositoryDependencyResolverException(Type dependencyType, Exception innerException = null)
            : base(
                String.Format("Could not resolve type '{0}' using the '{1}'.  Make sure you have configured your Ioc container for this type.  View the InnerException for more details.",
                    dependencyType,
                    RepositoryDependencyResolver.Current == null ? "" : RepositoryDependencyResolver.Current.GetType().Name), 
            innerException)
        {
            DependencyType = dependencyType;
            DependencyResolver = RepositoryDependencyResolver.Current;
        }
    }
}
