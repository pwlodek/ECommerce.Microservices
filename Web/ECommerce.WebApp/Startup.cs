using ECommerce.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace ECommerce.WebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddHttpContextAccessor();
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "redis:6379";
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
                .PersistKeysToRedis(ConnectionMultiplexer.Connect("redis:6379"));

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

            app.UseCookiePolicy();
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
