using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SharpRepository.Ioc.Autofac;
using SharpRepository.Repository;
using SharpRepository.Repository.Ioc;
using SharpRepository.Samples.Net6Mvc.CustomRepositories;
using SharpRepository.Samples.Net6Mvc.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b =>
{
    // reads configuratio from appsettings.json
    var sharpRepoConfig = builder.Configuration.GetSection("sharpRepository");
    
    b.RegisterSharpRepository(sharpRepoConfig, null, MatchingScopeLifetimeTags.RequestLifetimeScopeTag); //default InMemory
    // b.RegisterSharpRepository(sharpRepoConfig, "mongoDb"); // for Mongo Db
    // b.RegisterSharpRepository(sharpRepoConfig, "efCore");// for Ef Core
});

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ContactContext>(options => options.UseInMemoryDatabase(Assembly.GetExecutingAssembly().GetName().Name ?? "test"), ServiceLifetime.Transient);

// services.AddTransient<DbContext, ContactContext>(); // needed if you don't write dbContextClass on json configuration

builder.Services.AddTransient<EmailRepository>(r => new EmailRepository(RepositoryFactory.BuildSharpRepositoryConfiguation(builder.Configuration.GetSection("sharpRepository")), "efCore"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

RepositoryDependencyResolver.SetDependencyResolver(app.Services);

app.Run();
