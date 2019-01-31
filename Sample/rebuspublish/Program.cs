using System; 
using System.Collections.Generic; 
using System.IO; 
using System.Linq; 
using System.Threading.Tasks; 
using Microsoft.AspNetCore; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.Logging; 
 using DiscoveryService; 

namespace rebuspublish {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run(); 
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
             

      /* */  var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(path:"appsettings.json", optional:true, reloadOnChange:true)
                                .Build(); 

        return
            WebHost.CreateDefaultBuilder(args)
                   .AddDiscoveryServiceOptions(configuration,8893)
                   .UseStartup <Startup> ();
              
        }


      
                
    }
}
