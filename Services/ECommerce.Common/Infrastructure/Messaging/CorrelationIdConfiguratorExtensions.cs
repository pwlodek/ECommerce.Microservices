using GreenPipes;

namespace ECommerce.Common.Infrastructure.Messaging
{
    public static class CorrelationIdConfiguratorExtensions
    {
        public static void UseCorrelationId<T>(this IPipeConfigurator<T> configurator, IMessageCorrelationContextAccessor messageCorrelationContextAccessor)
            where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new CorrelationIdSpecification<T>(messageCorrelationContextAccessor));
        }
    }
}
