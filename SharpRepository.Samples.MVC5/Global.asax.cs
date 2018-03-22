using Microsoft.Extensions.Configuration;
using SharpRepository.Ioc.Unity;
using SharpRepository.Repository;
using SharpRepository.Repository.Ioc;
using SharpRepository.Samples.MVC5.Models;
using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using Unity.Lifetime;

namespace SharpRepository.Samples.MVC5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // MvcDependencyResolver.ForRepositoriesUseSharpRepository("repository.json", "sharpRepository", "efConnectionString"); // holds connection string on repository.json
            //MvcDependencyResolver.ForRepositoriesUseSharpRepository("repository.json", "sharpRepository", lifecycle: Lifecycles.Unique); // no connection string on repository.json
            var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("repository.json")
            .Build();

            var section = config.GetSection("sharpRepository");
            var sharpConfig = RepositoryFactory.BuildSharpRepositoryConfiguation(section);

            var container = new UnityContainer();
            container.RegisterSharpRepository(sharpConfig, null);

            container.RegisterType<DbContext>(new PerRequestLifetimeManager(), new InjectionFactory(c => new ContactsDbContext("name=ContactsDbContext")));
            
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            
            RepositoryDependencyResolver.SetDependencyResolver(new UnityRepositoryDependencyResolver(container));

        }
    }

    /// <summary>
    /// Registers ContactsDbContext, that knows the connection string, as DbContext
    /// </summary>
    //public class DbContexRegistry : Registry
    //{
    //    public DbContexRegistry()
    //    {
    //        For<DbContext>().Use(() => new ContactsDbContext("name=ContactsDbContext"));
    //    }
    //}
}
