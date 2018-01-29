using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Catalog.Api.Modules;
using ECommerce.Catalog.Api.Services;
using ECommerce.Services.Common.Configuration;
using log4net;
using MassTransit;
using MassTransit.Util;
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

        public IContainer Container { get; set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var rabbitHost = Configuration["RabbitHost"];
            Logger.Info($"Using RabbitHost='{rabbitHost}'.");

            var connectionString = Configuration["ConnectionString"];
            Logger.Info($"Using connectionString='{connectionString}'.");

            var waiter = new DependencyAwaiter();
            waiter.WaitForRabbit(rabbitHost);
            waiter.WaitForSql(connectionString);

            services.AddMvc();

            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterModule<BusModule>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();

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
            app.UseMvc();

            var bus = Container.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());

            Logger.Info("Running Catalog microservice.");
        }
    }
}
