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
using Rebus.Bus; 
using Rebus.Routing.TypeBased; 
using Rebus.ServiceProvider; 
using Rebus.Transport.InMem; 
using Refit; 
using Rebus.Subscriptions;
using Rebus.Persistence.FileSystem;
namespace Discovery.Consul {
    class Program {
        static void Main (string[] args) {
  

            ServiceRun().GetAwaiter().GetResult(); 
 
            Console.ReadKey (); 
        }

        static async Task ServiceRun () {

            var provider = GetProvider (services =>  {

            services.AddHttpClient();
            services.AddDiscoveryServiceClient(cfg => cfg.Address = new Uri("http://localhost:8870")); 

            services.AutoRegisterHandlersFromAssemblyOf < Handler1 > (); 
 
            services.AddRebus(configure => configure
                .Logging(l => l.None())
                .Subscriptions(s=>s.UseJsonFile("99.json"))
                .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), "Messages"))
                .Routing(r => r.TypeBased().MapAssemblyOf < Message1 > ("Messages")));
            }); 



            provider.UseRebus(); 


           var clusterclient = provider.GetService < IClusterClinet > (); 
           var httpClientFactory = provider.GetRequiredService < IHttpClientFactory > (); 

            string servicename = "apigateway"; 
                   var producer = provider.GetRequiredService < IBus > (); 
            await producer.Subscribe<Message1>();

            for (; ; ) {

                var httpclient = httpClientFactory.CreateClient(); 

             var gatewayapp = await  clusterclient.FindServiceInstanceAsync(servicename); 

                Console.WriteLine ("==================================================="); 

               Console.WriteLine($"{gatewayapp.ToUri()}fzf/add/9/8"); 
        

        
                         


          var resp = await  httpclient.GetStringAsync($"{gatewayapp.ToUri()}fzf/add/9/8"); 

          Console.WriteLine(resp); 
           await producer.Publish(new Message1(resp)); 


           
                Console.ReadKey (); 
            }

        }

        static IServiceProvider GetProvider (Action < IServiceCollection > serviceaction) {
            IServiceCollection servies = new ServiceCollection (); 
            
                serviceaction (servies); 
            

            return servies.BuildServiceProvider (); 

        }
    }

}