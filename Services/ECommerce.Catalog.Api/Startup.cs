using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CorrelationId;
using ECommerce.Catalog.Api.Configuration;
using ECommerce.Catalog.Api.Services;
using ECommerce.Services.Common.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                .AddSqlServer(Configuration["ConnectionStrings:ProductsDb"], tags: new[] { "db", "sql" });

            services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
            services.AddCorrelationId();
            services.AddMvc();
            services.AddHostedService<CatalogService>();

            var builder = new ContainerBuilder();

            builder.Populate(services);
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

            app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                Predicate = p => p.Tags.Count == 0
            });
            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = p => p.Tags.Count > 0
            });

            app.UseCorrelationId(new CorrelationIdOptions
            {
                UpdateTraceIdentifier = false,
                UseGuidForCorrelationId = true
            });

            app.UseMvc();
        }
    }
}
