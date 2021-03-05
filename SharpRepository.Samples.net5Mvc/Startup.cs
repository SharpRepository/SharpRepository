using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using SharpRepository.Samples.net5Mvc.CustomRepositories;
using SharpRepository.Repository;
using Microsoft.EntityFrameworkCore;
using SharpRepository.Ioc.Autofac;
using SharpRepository.Repository.Ioc;

namespace SharpRepository.Samples.net5Mvc
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
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
