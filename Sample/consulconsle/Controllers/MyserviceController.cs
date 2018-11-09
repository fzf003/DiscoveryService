using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
namespace consulconsle.Controllers {

    [Route ("api/[controller]")]
    [ApiController]
    public class CalcController : ControllerBase {

          

        [HttpGet ("add/{x}/{y}")]
        public string Add (int x, int y) 
        {
            return $"{x+y}==={ HttpContext.Request.Host.Port}";
        }

        [HttpGet ("sub/{x}/{y}")]
        public int Sub (int x, int y) => x - y;

        [HttpPost ("stddev")]
        public double StdDev ([FromBody] float[] numbers) => Math.Sqrt (numbers.Select (x => Math.Pow (x - numbers.Average (), 2)).Sum () / (numbers.Length - 1));
    }
}