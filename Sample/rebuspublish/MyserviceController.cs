using DiscoveryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
namespace rebuspublish.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class CalcController : ControllerBase {

        readonly IOptionsSnapshot<ServiceConfig> _serviceconfig;

          public CalcController(IOptionsSnapshot<ServiceConfig> ServiceConfig)
          {
              this._serviceconfig=ServiceConfig;

          }

        [HttpGet ("add/{x}/{y}")]
        public string Add (int x, int y) 
        {
            return this._serviceconfig.Value.version;
        }

        [HttpGet ("sub/{x}/{y}")]
        public int Sub (int x, int y) => x - y;

        [HttpPost ("stddev")]
        public double StdDev ([FromBody] float[] numbers) => Math.Sqrt (numbers.Select (x => Math.Pow (x - numbers.Average (), 2)).Sum () / (numbers.Length - 1));
    }
}