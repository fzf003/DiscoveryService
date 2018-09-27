using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscoveryService;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway
{
 
    [ApiController]
    public class QueryServiceController : ControllerBase
    {

        readonly IClusterClinet clusterClinet;

        public QueryServiceController(IClusterClinet clusterClinet)
        {
            this.clusterClinet = clusterClinet;

        }
  
        [HttpGet("/GetServices/{servicename}")]
        public Task<ServiceInformation[]> GetServices(string servicename)
        {
            return this.clusterClinet.FindServiceInstancesAsync(servicename);
        }
 
        [HttpGet("/GetServiceInfo/{servicename}")]
        public async Task<ServiceInformation> GetServiceName(string servicename)
        {
            return await this.clusterClinet.FindServiceInstanceAsync(servicename);
        }

    }
}
