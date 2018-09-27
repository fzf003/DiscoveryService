


using System;
using Microsoft.Extensions.Options;

namespace DiscoveryService
{

    public class ServiceConfig : IOptions<ServiceConfig>
    {
        public string serviceName { get; set; }

        public string serviceId { get; set; }

        public Uri serviceUri { get; set; }

        public string version { get; set; }

        public ServiceConfig Value => this;
    }
}