using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Catalog.Api.Modules;
using ECommerce.Catalog.Api.Services;
using ECommerce.Services.Common.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace ECommerce.Catalog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer Container { get; set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionStrings:ProductsDb"], tags: new[] { "db", "sql" })
                .AddRabbitMQ(Configuration["Brokers:RabbitMQ:Url"], tags: new[] { "broker" });

            services.AddMvc();
            services.AddHostedService<CatalogService>();

            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule<BusModule>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<IdentityService>().AsImplementedInterfaces().SingleInstance();

            Container = builder.Build();

            return new AutofacServiceProvider(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net();

            app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                Predicate = p => p.Tags.Count == 0
            });
            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = p => p.Tags.Count > 0
            });

            app.UseMvc();
        }
    }
}
