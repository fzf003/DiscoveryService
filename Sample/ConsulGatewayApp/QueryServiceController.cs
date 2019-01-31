using System.Threading.Tasks;
using DiscoveryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
namespace ConsulGatewayApp {
    
   /*  [ApiController]
    public class QueryServiceController  {

        readonly IClusterClinet clusterClinet;

        readonly ServiceConfig serviceConfig;

        public QueryServiceController (IClusterClinet clusterClinet, IOptions<ServiceConfig> serviceConfig) {
            this.clusterClinet = clusterClinet;
            this.serviceConfig = serviceConfig.Value;

        }

    

        [HttpGet ("/GetServices/{servicename}")]
        public async Task<ServiceInformation[]> GetServices (string servicename) {

            var result= await this.clusterClinet.FindServiceInstancesAsync (servicename);

              return result.Select(p=>
            {
                p.Version=this.serviceConfig.version;
                return p;
            }).ToArray();
        }

        [HttpGet ("/GetServiceInfo/{servicename}")]
        public async Task<ServiceInformation> GetServiceName (string servicename) {
            var result = await this.clusterClinet.FindServiceInstanceAsync (servicename);

            if (result != null) {
                result.Version = this.serviceConfig.version;
            }

            return result;

        }

    }*/
}