using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using CorrelationId;
using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Reporting.Api.Modules;
using ECommerce.Reporting.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerce.Reporting.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        public bool UseCloudServices => Configuration.GetValue<bool>("UseCloudServices");

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var healthCheckBuilder = services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionStrings:ReportingDb"], tags: new[] { "db", "sql" });

            if (UseCloudServices)
            {
                healthCheckBuilder
                    .AddAzureServiceBusQueue(Configuration["Brokers:ServiceBus:Url"], "reporting_fanout", name: "sales_fanout_queue", tags: new[] { "broker" });
            }
            else
            {
                healthCheckBuilder
                    .AddRabbitMQ(Configuration["Brokers:RabbitMQ:Url"], tags: new[] { "broker" });
            }

            services.AddTransient<CorrelationIdDelegatingHandler>();
            services.AddHttpClient("DefaultClient")
                .AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

            services.AddCorrelationId();
            services.AddMvc();
            services.AddHostedService<ReportingService>();

            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule(GetBusModule());
            builder.RegisterModule<ConsumerModule>();

            builder.RegisterType<DataService>().As<IDataService>().SingleInstance();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().SingleInstance();
            
            return new AutofacServiceProvider(builder.Build());
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

        private IModule GetBusModule()
        {
            return UseCloudServices ?
                (IModule)new AzureBusModule() : new BusModule();
        }
    }
}
