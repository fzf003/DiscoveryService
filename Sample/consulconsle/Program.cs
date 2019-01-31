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

namespace consulconsle {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Run(); 
         }

        public static IWebHost CreateWebHostBuilder(string[] args) {
             
              var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(path:"appsettings.json", optional:true, reloadOnChange:true)
                                .Build();

             return WebHost.CreateDefaultBuilder(args)
                            .AddDiscoveryServiceOptions(configuration)
                            .UseStartup < Startup > ()
                            .Build(); 
        }

    }



}