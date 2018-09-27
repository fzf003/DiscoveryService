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

namespace Discovery.Consul
{
    class Program
    {
        static void Main(string[] args)
        {

            ServiceRun().Wait();

            Console.ReadKey();
        }

        static async Task ServiceRun()
        {
            var provider = GetProvider(services =>
          {
            /*  services.AddDiscoveryServiceClient(cfg =>
              {
                  cfg.Address = new Uri("http://10.6.24.13:8500");
              });*/

               services.AddSingleton<QueryServiceOption>(new QueryServiceOption{
                   GatewayUrl="http://localhost:5000"
               });

 

              services.AddSingleton<IGateWayQueryService, APiServiceQuery>();

          });


            var queryserviceenpoint = provider.GetService<IGateWayQueryService>();


            string servicename = "apiservice";


            for (; ; )
            {

                Console.WriteLine("===================================================");

                var queryServiceclient = RestService.For<IQueryService>("http://localhost:5000");
                
                //await queryserviceenpoint.GetEndpoint(servicename);

                 var queryresult = await queryServiceclient.QuerySerivce(servicename);


                Console.WriteLine(queryresult.ToString());

                Console.ReadKey();
            }

        }




        static IServiceProvider GetProvider(Action<IServiceCollection> serviceaction)
        {
            IServiceCollection servies = new ServiceCollection();
            if (serviceaction != null)
            {
                serviceaction(servies);
            }

            return servies.BuildServiceProvider();

        }
    }

    
}