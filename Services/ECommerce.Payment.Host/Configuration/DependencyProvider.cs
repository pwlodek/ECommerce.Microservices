using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Payment.Host.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Payment.Host.Configuration
{
    class DependencyProvider : IServiceProviderFactory<ContainerBuilder>
    {
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<BusModule>();
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
