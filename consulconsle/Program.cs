using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DiscoveryService;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace consulconsle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder().Run();
        }



        public static IWebHost CreateWebHostBuilder()
        {
            var baseurl = Configuration.GetUri();

            return WebHost.CreateDefaultBuilder()
                   .ConfigureServices(services =>
                   {
                       services.AddSingleton<ServiceConfig>(new ServiceConfig
                       {

                           serviceName = "apiservice",

                           serviceId = Guid.NewGuid().ToString("N"),

                           serviceUri = baseurl,

                           version = "v1.0"
                       });
                   })
                 .UseUrls(baseurl.AbsoluteUri.Replace("localhost", "*"))
                .UseStartup<Startup>()
                .Build();
        }

    }



}