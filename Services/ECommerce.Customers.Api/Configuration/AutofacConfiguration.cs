using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Customers.Api.Configuration
{
    public static class AutofacConfiguration
    {
        public static IContainer AddAutofac(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            return builder.Build();
        }
    }
}
