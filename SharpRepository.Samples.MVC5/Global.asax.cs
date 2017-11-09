using SharpRepository.Ioc.Mvc;
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
