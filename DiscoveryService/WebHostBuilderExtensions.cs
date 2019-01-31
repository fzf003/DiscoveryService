using System; 
using System.IO; 
using System.Linq; 
using System.Net.Http; 
using System.Threading.Tasks; 
using Consul; 
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Options; 
using Microsoft.AspNetCore; 
 
using Microsoft.Extensions.Logging; 
using System.Collections.Generic; 

namespace DiscoveryService {

    public static class WebHostBuilderExtensions {

   

     public static IWebHostBuilder AddDiscoveryServiceOptions(this IWebHostBuilder webHostBuilder, IConfiguration configuration, int?runPort = null ) {

         if (webHostBuilder == null) {
                throw new ArgumentNullException(nameof(webHostBuilder)); 
            }

        List < string > urls = new List < string > (); 

        ServiceConfig serviceConfig = new ServiceConfig(); 

        configuration.Bind("AppConfig", serviceConfig); 
 
        if (runPort != null) {
            urls.Add($"http://*:{runPort}");
            serviceConfig.serviceUri = new Uri($"http://localhost:{runPort}");
        }else {
            var baseurl = UriConfiguration.GetUri(); 
            urls.Add($"http://*:{baseurl.Port}");
            serviceConfig.serviceUri = baseurl; 
         }

         webHostBuilder.UseUrls(urls.ToArray()); 
        
        return  webHostBuilder.ConfigureServices(services=>{
               services.Configure<ServiceConfig>(options=>{
                options.serviceName = serviceConfig.serviceName; 
                options.serviceUri = serviceConfig.serviceUri; 
                options.version = serviceConfig.version; 
                options.Tags = serviceConfig.Tags; 
               });
        });
      
      }



         public static   IApplicationBuilder UseDiscoveryService (this IApplicationBuilder appBuilder) {
            var appLifetime = appBuilder.ApplicationServices.GetService < IApplicationLifetime > (); 

            appLifetime.ApplicationStarted.Register (() =>  {

                var serviceinfo = appBuilder.ApplicationServices.GetService < IOptions < ServiceConfig >> (); 
                var clusterClinet = appBuilder.ApplicationServices.GetRequiredService < IClusterClinet > (); 

                Console.WriteLine("开始:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(serviceinfo.Value)); 

                clusterClinet.RegisterServiceAsync (
                    serviceinfo.Value.serviceName, 
                    serviceinfo.Value.serviceId, 
                    serviceinfo.Value.version, 
                    serviceinfo.Value.serviceUri, 
                    serviceinfo.Value.Tags); 
             }); 

            appLifetime.ApplicationStopping.Register (() =>  {

                var serviceinfo = appBuilder.ApplicationServices.GetService < IOptions < ServiceConfig >> (); 
                var cluster = appBuilder.ApplicationServices.GetService < IClusterClinet > (); 
                cluster.DeregisterServiceAsync (serviceinfo.Value.serviceId); 
             }); 

            appBuilder.MapWhen (context =>  {
                return context.Request.Path.Value.Contains ("/status"); 

            }, appbuild =>  {

                appbuild.Run (async context =>  {

                    await context.Response.WriteAsync ("ok!"); 

                }); 

            }); 

            return appBuilder; 
        }

    }

}