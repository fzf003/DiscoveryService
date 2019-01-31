using System; 
using System.Threading.Tasks; 
using Consul; 
using DiscoveryService; 
using Microsoft.AspNetCore; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration; 
using System.Linq; 
using System.IO; 

namespace ConsulGatewayApp {
    class Program {
        public static void Main (string[] args) {
 
            CreateWebHostBuilder (args).Build ().Run (); 
        }

        public static IWebHostBuilder CreateWebHostBuilder (string[] args) {
 
 var config=new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(path:"appsettings.json", optional:true, reloadOnChange:true)
                                .AddJsonFile("Ocelot.json", true, true)
                                .Build();

            return WebHost.CreateDefaultBuilder (args)
                  
                 .AddDiscoveryServiceOptions(config)
                 .UseStartup < Startup > (); 
        }
    }


}