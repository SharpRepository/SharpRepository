using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using SharpRepository.Ioc.Autofac;

namespace SharpRepository.Ioc.Microsoft.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IHostBuilder SetupSharpRepository(this ISupportsConfigureWebHost hostBuilder, IConfigurationSection configurationSection, string repositoryName = null)
        {
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            hostBuilder.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(configurationSection, repositoryName));
            return hostBuilder;
        }
    }
}
