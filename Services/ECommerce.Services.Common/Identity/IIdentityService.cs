using System;
namespace ECommerce.Services.Common.Identity
{
    public interface IIdentityService
    {
        string InstanceId { get; }

        string HostName { get; }
    }
}
