using System;
using CorrelationId;
using ECommerce.Customers.Api.Configuration;
using ECommerce.Customers.Api.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Customers.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionStrings:CustomersDb"], tags: new[] { "db", "sql" });

            services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
            services.AddCorrelationId();
            services.AddMvc();
            services.AddScoped<ICustomerRepository>(c => new CustomerRepository(Configuration["ConnectionStrings:CustomersDb"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
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
