using System;
using ECommerce.Catalog.Api.Services;
using ECommerce.Services.Common.Configuration;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ECommerce.Catalog.Api
{
    public class Startup
    {
        private static ILog Logger = LogManager.GetLogger(typeof(Startup));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionString"];
            Logger.Info($"Using connectionString='{connectionString}'.");

            var waiter = new DependencyAwaiter();
            waiter.WaitForSql(connectionString);

            services.AddMvc();
            services.AddScoped<IProductRepository>(c => new ProductRepository(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net();
            app.UseMvc();
        }
    }
}
