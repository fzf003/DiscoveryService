using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using Consul;
using DiscoveryService;
using Microsoft.Extensions.DependencyInjection;

namespace Discovery.Consul {
    class Program {
        static void Main (string[] args) {

            var provider = GetProvider (services => {
                services.AddDiscoveryService();
            });

           
            

            var client = provider.GetService<IClusterProvider> ();

            var httpfactory = provider.GetService<IHttpClientFactory> ();

            string servicename = "Net40App";

            var kvclient = client.KvPutAsync ("f", new {
                name = "fzf003"
            });

            for (;;) {
                var result = DnsHelper.GetIpAddressAsync ().Result;
                Console.WriteLine (result);
                Console.ReadKey ();
            }

           // Console.ReadKey ();

            for (;;) {
                Console.WriteLine ("===================================================");

                var servinfo = client.FindServiceInstanceAsync (servicename).Result;

                var httpclient = httpfactory.CreateClient ();

                httpclient.GetStringAsync (servinfo.ToUri ().AbsoluteUri + "api/health")
                    .ContinueWith (tr => {
                        Console.WriteLine ($"{servinfo.ToUri()}--{tr.Result}");
                    });

                Console.ReadKey ();
            }

            Console.ReadKey ();
        }

        static IServiceProvider GetProvider (Action<IServiceCollection> serviceaction) {
            IServiceCollection servies = new ServiceCollection ();
            if (serviceaction != null) {
                serviceaction (servies);
            }

            return servies.BuildServiceProvider ();

        }
    }
}