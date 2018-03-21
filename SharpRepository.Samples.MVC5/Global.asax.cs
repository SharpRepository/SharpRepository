using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.Configuration;
using SharpRepository.Ioc.Autofac;
using SharpRepository.Ioc.Mvc;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
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
            MvcDependencyResolver.ForRepositoriesUseSharpRepository("repository.json", "sharpRepository", lifecycle: Lifecycles.Unique); // no connection string on repository.json
            
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("repository.json")
              .Build();

            var section = config.GetSection("sharpRepository");
            ISharpRepositoryConfiguration configuration = RepositoryFactory.BuildSharpRepositoryConfiguation(section);

            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterSharpRepository(configuration, null, MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            builder.RegisterType<ContactsDbContext>().As<DbContext>().InstancePerRequest();
            var container = builder.Build();
            
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }

    /// <summary>
    /// Registers ContactsDbContext, that knows the connection string, as DbContext
    /// </summary>
    public class DbContexRegistry : Registry
    {
        public DbContexRegistry()
        {
            For<DbContext>().Use(() => new ContactsDbContext("name=ContactsDbContext"));
        }
    }
}
