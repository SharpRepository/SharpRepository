using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpRepository.Ioc.Autofac
{
    class RepositoryRegistrationSource : IRegistrationSource
    {
        protected ISharpRepositoryConfiguration Configuration;
        protected object[] LifetimeScopeTag;
        protected string RepositoryName;

        public RepositoryRegistrationSource(ISharpRepositoryConfiguration configuration, string repositoryName = null, params object[] lifetimeScopeTag)
        {
            Configuration = configuration;
            RepositoryName = repositoryName;
            LifetimeScopeTag = lifetimeScopeTag;
        }

        public bool IsAdapterForIndividualComponents => true;

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var swt = service as IServiceWithType;
            IEnumerable<Type> repositoryInterfaces = new List<Type> { typeof(IRepository<>), typeof(IRepository<,>),
                typeof(ICompoundKeyRepository<,,>), typeof(ICompoundKeyRepository<,,,>), typeof(ICompoundKeyRepository<>)
            };

            bool isRepositoryFunc(Type repo) => repo.GetTypeInfo().IsGenericType ? repositoryInterfaces.Contains(repo.GetGenericTypeDefinition()) : false;
            bool isCompoundRepositoryFunc(Type repo) => repo.GetTypeInfo().IsGenericType ? typeof(ICompoundKeyRepository<>) == repo.GetGenericTypeDefinition() : repo.GetInterfaces().Where(i => i.GetTypeInfo().IsGenericType).Select(i => i.GetGenericTypeDefinition()).Any(i => typeof(ICompoundKeyRepository<>) == i);

            if (swt == null || !isRepositoryFunc(swt.ServiceType))
            {
                // It's not a request for the base handler type, so skip it.
                return Enumerable.Empty<IComponentRegistration>();
            }

            IComponentLifetime lifetime;
            var instanceSharing = InstanceSharing.None;
            if (LifetimeScopeTag != null && LifetimeScopeTag.Count() > 0)
            {
                lifetime = new MatchingScopeLifetime(LifetimeScopeTag);
                instanceSharing = InstanceSharing.Shared;
            }
            else
                lifetime = new CurrentScopeLifetime();
            
            var registration = new ComponentRegistration(
                  Guid.NewGuid(),
                  new DelegateActivator(swt.ServiceType, (c, p) =>
                    {
                        Type modelType = swt.ServiceType.GetGenericArguments().First();
                        if (isCompoundRepositoryFunc(swt.ServiceType))
                        {

                            return RepositoryFactory.GetCompoundKeyInstance(modelType, Configuration, RepositoryName);
                        }
                        else if (isRepositoryFunc(swt.ServiceType))
                            switch (swt.ServiceType.GetGenericArguments().Count())
                            {
                                case 1:
                                    return RepositoryFactory.GetInstance(modelType, Configuration, RepositoryName);
                                case 2:
                                    var key1Type = swt.ServiceType.GetGenericArguments()[1];
                                    return RepositoryFactory.GetInstance(modelType, key1Type, Configuration, RepositoryName);
                                case 3:
                                    var key1Type2 = swt.ServiceType.GetGenericArguments()[1];
                                    var key2Type2 = swt.ServiceType.GetGenericArguments()[2];
                                    return RepositoryFactory.GetInstance(modelType, key1Type2, key2Type2, Configuration, RepositoryName);
                                case 4:
                                    var key1Type3 = swt.ServiceType.GetGenericArguments()[1];
                                    var key2Type3 = swt.ServiceType.GetGenericArguments()[2];
                                    var key3Type3 = swt.ServiceType.GetGenericArguments()[3];
                                    return RepositoryFactory.GetInstance(modelType, key1Type3, key2Type3, key3Type3, Configuration, RepositoryName);
                                default:
                                    throw new NotImplementedException("Error resolving repository: " + swt.ServiceType.Name);

                            }
                        else
                            throw new NotImplementedException("Error resolving repository: " + swt.ServiceType.Name);
                    }),
                  lifetime,
                  instanceSharing,
                  InstanceOwnership.OwnedByLifetimeScope,
                  new[] { service },
                  new Dictionary<string, object>()
            );


            return new IComponentRegistration[] { registration };
        }

        public Action<IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>> RegistrationConfiguration { get; set; }

    }
}

