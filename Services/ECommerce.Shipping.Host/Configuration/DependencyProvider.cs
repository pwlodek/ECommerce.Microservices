using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Shipping.Host.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Shipping.Host.Configuration
{
    class DependencyProvider : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();
            var useCloud = configuration.GetValue<bool>("UseCloudServices");
            builder.RegisterModule(useCloud ? (IModule)new AzureBusModule() : new BusModule());
            builder.RegisterModule<ConsumerModule>();

            builder.Populate(services);

            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}
