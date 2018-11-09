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
using Microsoft.Extensions.Options;

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
            var baseurl = UriConfiguration.GetUri();

            return WebHost.CreateDefaultBuilder()
                   .ConfigureServices(services =>
                   { 
                        services.AddDiscoveryService(cfg =>
                       {
                           cfg.serviceName = "apiservice";
                           cfg.serviceId = Guid.NewGuid().ToString("N");
                           cfg.serviceUri = baseurl;
                           cfg.version = "v1.0";
                        });
                    })
                 .UseUrls(baseurl.AbsoluteUri.Replace("localhost", "*"))
                .UseStartup<Startup>()
                .Build();
        }

    }



}