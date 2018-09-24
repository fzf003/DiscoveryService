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

namespace consulconsle {
    public class Program {
        public static void Main (string[] args) {
            //CreateWebHostBuilder (args).Build ().Run ();

          Console.WriteLine(  new myHost ().Start ("apiservice", "v1.0"));

            Console.ReadKey ();

            // new Host1 ().Start ("apiservice", "v1.0").Build ().Run ();
        }

    }

    public class myHost : DiscoveryService.IFrameworkProvider {
        public Uri Start (string serviceName, string version) {
            var baseurl = Configuration.GetUri ();

            WebHost.CreateDefaultBuilder ()
                .ConfigureServices (services => {
                    services.AddSingleton<StartEndPoint> (new StartEndPoint () {
                        Url = baseurl,
                            ServiceId = Guid.NewGuid ().ToString ("N"),
                            serviceName = serviceName,
                            Version = version

                    });

                    services.AddSingleton<ServiceConfig> (new ServiceConfig {
                        serviceName = serviceName,
                            serviceId = Guid.NewGuid ().ToString ("N"),

                            serviceUri = baseurl,

                            version = version

                    });

                    services.AddSingleton<IClusterClinet,ClusterClient>();
                })

                .UseUrls (baseurl.AbsoluteUri.Replace ("localhost", "*"))

                .UseStartup<Startup> ()
                .Build ().RunAsync ();

            return baseurl;
        }
    }

    public class Host1 : IServiceHostProvider {
        public IWebHostBuilder Start (string serviceName, string version) {
            var baseurl = Configuration.GetUri ();

            return WebHost.CreateDefaultBuilder ()
                .ConfigureServices (services => {
                    services.AddSingleton<StartEndPoint> (new StartEndPoint () {
                        Url = baseurl,
                            ServiceId = Guid.NewGuid ().ToString ("N"),
                            serviceName = serviceName,
                            Version = version

                    });
                })

                .UseUrls (baseurl.AbsoluteUri)

                .UseStartup<Startup> ();
        }
    }

    public class StartEndPoint {
        public Uri Url { get; set; }

        public string ServiceId { get; set; }

        public string serviceName { get; set; }

        public string Version { get; set; }

    }

}