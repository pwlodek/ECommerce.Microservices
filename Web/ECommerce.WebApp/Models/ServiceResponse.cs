using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Models
{
    public class ServiceResponse<TPayload>
    {
        public string HostName { get; set; }

        public string InstanceId { get; set; }

        public TPayload Payload { get; set; }
    }
}
