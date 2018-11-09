using System;
using DiscoveryService;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ConsulGatewayApp
{
    class Program
    {
          public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) 
        {
              var baseurl = UriConfiguration.GetUri();
           return  WebHost.CreateDefaultBuilder(args)
                   .UseKestrel()
                   .ConfigureAppConfiguration((context,cfg)=>{
                        cfg.AddJsonFile("Ocelot.json",true,true);
                    })
                     .ConfigureServices(services =>
                   { 
                        services.AddDiscoveryService(cfg =>
                       {
                           cfg.serviceName = "gatewayapp";
                           cfg.serviceId = Guid.NewGuid().ToString("N");
                           cfg.serviceUri = baseurl;
                           cfg.version = "v1.0";
                        });
                    })
                    .PreferHostingUrls(true)
                    .SetUrl(baseurl)
                   .UseStartup<Startup>();
        }
    }


    public static class WebHostBuilderExtensions
    {

       public static IWebHostBuilder SetUrl(this IWebHostBuilder hostBuilder, Uri baseurl)
       {
 
            return hostBuilder.UseUrls(baseurl.AbsoluteUri.Replace("localhost", "*"));
       }

    }
    
    
}
