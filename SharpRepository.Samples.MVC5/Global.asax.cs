using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.Configuration;
using SharpRepository.Ioc.Autofac;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using SharpRepository.Repository.Ioc;
using SharpRepository.Samples.MVC5.Models;
using System;
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
            
            // get configuration from repository.json
            var config = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("repository.json")
              .Build();

            var section = config.GetSection("sharpRepository");
            ISharpRepositoryConfiguration configuration = RepositoryFactory.BuildSharpRepositoryConfiguation(section);

            // configure autofac
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterSharpRepository(configuration, null, MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            builder.RegisterType<ContactsDbContext>().As<DbContext>().WithParameter(new TypedParameter(typeof(string), "name=ContactsDbContext")).InstancePerRequest();
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            RepositoryDependencyResolver.SetDependencyResolver(new CustomAutofacRepositoryDependencyResolver(DependencyResolver.Current));
        }
    }
}
