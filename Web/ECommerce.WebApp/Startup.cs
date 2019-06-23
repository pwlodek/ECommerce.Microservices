using ECommerce.WebApp.HealthChecks;
using ECommerce.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace ECommerce.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var redisUrl = Configuration["Cache:Redis"];
            services.AddHealthChecks()
                .AddCheck("redis", new RedisHealthCheck(redisUrl), tags: new[] { "redis" })
                .AddUrlGroup(new Uri($"http://{Configuration["Services:Catalog"]}/health/ready"), name: "catalog", tags: new[] { "url" })
                .AddUrlGroup(new Uri($"http://{Configuration["Services:Sales"]}/health/ready"), name: "sales", tags: new[] { "url"})
                .AddUrlGroup(new Uri($"http://{Configuration["Services:Reporting"]}/health/ready"), name: "reporting", tags: new[] { "url" });

            services.AddMvc();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddHttpContextAccessor();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = redisUrl;
                options.InstanceName = "master";
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            });
            services.AddHttpClient();
            
            // Required in farm scenario
            services
                .AddDataProtection(opt => opt.ApplicationDiscriminator = "ecommerce-webapp")
                .PersistKeysToRedis(ConnectionMultiplexer.Connect(redisUrl));

            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IBasketService, BasketService>();
            services.AddSingleton<IOrderService, OrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseCookiePolicy();
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
