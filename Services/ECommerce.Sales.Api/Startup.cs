using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Sales.Api.Consumers;
using ECommerce.Sales.Api.Model;
using ECommerce.Sales.Api.Modules;
using ECommerce.Sales.Api.Services;
using log4net;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerce.Sales.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer Container { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionStrings:SalesDb"], tags: new[] { "db", "sql" })
                .AddRabbitMQ(Configuration["Brokers:RabbitMQ:Url"], tags: new[] { "broker" });
            
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<SalesContext>(options =>
                    {
                        options.UseSqlServer(Configuration["ConnectionStrings:SalesDb"],
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            });
                    },
                        ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                    );

            services.AddHttpClient();
            services.AddHostedService<SalesService>();

            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule<BusModule>();
            builder.RegisterModule<ConsumerModule>();
            builder.RegisterType<DataService>().As<IDataService>().SingleInstance();

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

            app.UseMvc();
            loggerFactory.AddLog4Net();
        }
    }
}
