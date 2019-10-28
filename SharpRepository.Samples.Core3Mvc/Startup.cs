using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpRepository.Ioc.Autofac;
using SharpRepository.Repository;
using SharpRepository.Repository.Ioc;
using SharpRepository.Samples.Core3Mvc.CustomRepositories;

namespace SharpRepository.Samples.Core3Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ContactContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            // services.AddTransient<DbContext, ContactContext>(); // needed if you don't write dbContextClass on json configuration

            services.AddTransient<EmailRepository>(r => new EmailRepository(RepositoryFactory.BuildSharpRepositoryConfiguation(Configuration.GetSection("sharpRepository")), "efCore"));
            
        }

        // This is the default if you don't have an environment specific method.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // builder.RegisterSharpRepository(sharpRepoConfig); //default InMemory
            // builder.RegisterSharpRepository(sharpRepoConfig, "mongoDb"); // for Mongo Db
            // builder.RegisterSharpRepository(sharpRepoConfig, "efCore"); // for Ef Core

            builder.RegisterSharpRepository(Configuration.GetSection("sharpRepository"), "efCore"); // for Ef Core
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Passes service provide to SharpRepository
            RepositoryDependencyResolver.SetDependencyResolver(app.ApplicationServices);
        }
    }
}
