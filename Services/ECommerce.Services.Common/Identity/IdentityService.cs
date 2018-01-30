using System;
namespace ECommerce.Services.Common.Identity
{
    public class IdentityService : IIdentityService
    {
        private static readonly Guid _instanceId = Guid.NewGuid();

        public IdentityService()
        {
        }

        public string InstanceId => _instanceId.ToString();

        public string HostName => Environment.MachineName;
    }
}
