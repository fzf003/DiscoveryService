using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DiscoveryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace consulconsle.Controllers {

    [Route ("apiservice/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {

        readonly IClusterClinet clusterProvider;

        readonly IServiceBus serviceBus;

        readonly IOptions<ServiceConfig> serviceConfig;

        readonly HttpClient httpClient;

        public IConfiguration Configuration { get; }

        public ValuesController (IClusterClinet clusterProvider, HttpClient httpClient, IConfiguration configuration, IOptions<ServiceConfig> serviceConfig) {
            this.httpClient = httpClient;

            this.Configuration = configuration;

            this.serviceConfig = serviceConfig;

            this.clusterProvider = clusterProvider;

        }

        /*  public ValuesController(IClusterClinet clusterProvider, IServiceBus serviceBus, IOptions<ServiceConfig> serviceConfig, HttpClient httpClient, IConfiguration configuration)
          {
              this.clusterProvider = clusterProvider;

              this.serviceBus = serviceBus;

              this.serviceConfig = serviceConfig;

              this.httpClient = httpClient;

              this.Configuration = configuration;

          } */

        [HttpGet ("str/{str}")]
        public async Task<string> GetStr (string str) => $"http://39.96.8.95:8700/{str}";

        // GET api/values
        [HttpGet]
        public async Task<ServiceInformation[]> Get () {

            var result = await this.clusterProvider.FindServiceInstancesAsync ("apiservice");

            return result.Select (p => {
                p.Version = this.serviceConfig.Value.version;
                return p;
            }).ToArray ();
        }

        [HttpGet ("/ServiceInfo")]
        public string Heathle () {
            return HttpContext.Request.Host.Port + " " + Configuration["AppName"] + " " + DateTime.Now.ToString ();

        }

        // GET api/values/5
        [HttpGet ("{id}")]
        public async Task<ServiceInformation> Get (string id) {
            var result = await this.clusterProvider.FindServiceInstanceAsync (id);
            if (result != null) {
                result.Version = this.serviceConfig.Value.version;
            }

            return result;
        }

        // POST api/values
        [HttpPost]
        public void Post ([FromBody] name value) {
            Console.WriteLine ("获取:" + value.l);
        }

        // PUT api/values/5
        [HttpPut ("{id}")]
        public void Put (int id, [FromBody] string value) { }

        // DELETE api/values/5
        [HttpDelete ("{id}")]
        public void Delete (int id) { }
    }

    public class name {
        public string l { get; set; }
    }
}