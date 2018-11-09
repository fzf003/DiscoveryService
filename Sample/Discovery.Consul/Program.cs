using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Consul;
using DiscoveryService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Discovery.Consul {
    class Program {
        static void Main (string[] args) {

             ServiceRun().Wait();

             /* 

            using (var host = new MyServiceHost (services => {
                 services.AddSingleton<IStart, MyStart> ();
             })) {

                host.Run();

                var process = host.ApplicationServices.GetService<IStart> ();

                process.Start ();

                Console.ReadKey ();
            }*/

            Console.ReadKey ();
        }

        static async Task ServiceRun () {
            var provider = GetProvider (services => {

                services.AddSingleton<QueryServiceOption> (new QueryServiceOption {
                    GatewayUrl = "http://10.0.84.33:60753"
                });

                services.AddSingleton<IGateWayQueryService, APiServiceQuery> ();

            });

            var queryserviceenpoint = provider.GetService<IGateWayQueryService> ();

            string servicename = "apiservice";

            for (;;) {

                Console.WriteLine ("===================================================");

            //    var queryServiceclient = RestService.For<IQueryService> ("http://10.0.84.33:60753");

             var GetEndpoint=   await queryserviceenpoint.GetEndpoint(servicename);

             //   var queryresult = await queryServiceclient.QuerySerivce (servicename);

                Console.WriteLine ($"{GetEndpoint.ToString()}");

                Console.ReadKey ();
            }

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