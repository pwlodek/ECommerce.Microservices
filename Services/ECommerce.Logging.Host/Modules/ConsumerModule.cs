using System;
using Autofac;
using ECommerce.Logging.Host.Consumers;

namespace ECommerce.Logging.Host.Modules
{
    public class ConsumerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LogCommandConsumer>();
        }
    }
}
