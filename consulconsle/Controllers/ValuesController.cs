using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DiscoveryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace consulconsle.Controllers
{

    
     [Route("apiservice/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        readonly IClusterProvider clusterProvider;

        readonly IServiceBus serviceBus;

        readonly ServiceConfig serviceConfig;

        readonly HttpClient httpClient;

         public IConfiguration Configuration { get; }



        public ValuesController(IClusterProvider clusterProvider, IServiceBus serviceBus, ServiceConfig serviceConfig, HttpClient httpClient,IConfiguration configuration)
        {
            this.clusterProvider = clusterProvider;

            this.serviceBus = serviceBus;

            this.serviceConfig = serviceConfig;

            this.httpClient = httpClient;

            this.Configuration=configuration;

        }
        // GET api/values
       [HttpGet]
        public async Task<ServiceInformation> Get()
        {

            var result = await this.clusterProvider.FindServiceInstanceAsync("apiservice");

            await this.serviceBus.SendAsync<dynamic>(this.serviceConfig.serviceName, result);

            return result;
        }

        
        [HttpGet("/ServiceInfo")]
        public string Heathle()
        {
             return HttpContext.Request.Host.Port + " " + Configuration["AppName"] + " " + DateTime.Now.ToString();

        }


      




        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ServiceInformation> Get(string id)
        {
            var result = await this.clusterProvider.FindServiceInstanceAsync(id);
            return result;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
