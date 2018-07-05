using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpRepository.Ioc.Microsoft.DependencyInjection;
using SharpRepository.Repository;
using SharpRepository.Samples.CoreMvc.CustomRepositories;
using System;
using MongoDB.Bson.Serialization;
using SharpRepository.CoreMvc.Models;

namespace SharpRepository.CoreMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<ContactContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            // services.AddTransient<DbContext, ContactContext>(); // needed if you don't write dbContextClass on json configuration

            services.AddTransient<EmailRepository>(r => new EmailRepository(RepositoryFactory.BuildSharpRepositoryConfiguation(Configuration.GetSection("sharpRepository")), "efCore"));

            // return services.UseSharpRepository(Configuration.GetSection("sharpRepository")); //default InMemory
            // return services.UseSharpRepository(Configuration.GetSection("sharpRepository"), "mongoDb"); // for Mongo Db
            return services.UseSharpRepository(Configuration.GetSection("sharpRepository"), "efCore"); // for Ef Core
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
