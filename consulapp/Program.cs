using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using consulapp.ProcessManager;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proto;
using Proto.Router;
using StackExchange.Redis;
using STAN.Client;
using Process = System.Diagnostics.Process;

namespace consulapp {
    class Program {
        static void Main (string[] args) {
            Console.WriteLine ("Hello World!");
            CreateWebHostBuilder(args).Run();
     

        

 
  

           /* IServiceCollection services = new ServiceCollection ();

            services.AddLogging ();

            services.AddSingleton<Consul.IConsulClient> (p => new Consul.ConsulClient (config => {

                config.Address = new Uri ($"http://localhost:8500");

            }));

            services.AddProtoActor (registerAction => {
                registerAction.RegisterProps<DIActor> (props => {

                    return props;
                    //Router.NewRoundRobinPool(props,1);

                  
                    //Actor.FromProducer(()=>new DIActor());
                });
            });*/

          //  IServiceProvider provider = services.BuildServiceProvider ();

            /*  

              var client = provider.GetService<Consul.IConsulClient> ();

              string serciename = "apiservice";
              for (;;) {

                  client.Health.Service (serciename).Result
                          .Response.Where (p => p.Service.Service == serciename)
                          .ToList ()
                          .ForEach (k => {

                              Console.WriteLine ($"{k.Service.Address}== {k.Service.Port}=={k.Service.Service}");
                          });
                  Console.ReadKey ();

              }*/

         /*   var Loggerfactory = provider.GetService<ILoggerFactory> ();

            var facotry = provider.GetService<IActorFactory> ();

            Proto.Log.SetLoggerFactory (Loggerfactory);

            var diactor = facotry.GetActor<DIActor> ();

            diactor.Tell ("aaaa");*/

            /* Proto.ActorClient client = new Proto.ActorClient (new MessageHeader (), next => async (context, target, envelope) => {
                Console.WriteLine("Sender:{0}", context.Message.ToString());
                    await  next.Invoke(context,target,envelope);
                   Console.WriteLine("PID:{0}", target.ToShortString());
                     Console.WriteLine("Sender:{0}", context.Message.ToString());
             });

             var actor = facotry.GetActor<DIActor> ();

             client.Tell(actor,new MessageEnvelope ("cccc",actor,new MessageHeader()));*/

            // actor.Tell ("aaaaaa");

            

            Console.ReadKey ();
        }

        public static IWebHost CreateWebHostBuilder (string[] args) {

            IConfiguration config = new ConfigurationBuilder()
            
							.AddCommandLine(args)
							.Build();

			Startup.Args = args;

            return new WebHostBuilder ()
            
            .UseStartup<Startup> ()
            
            .Build ();


        }
            
        static void Publisher () {
            var cf = new StanConnectionFactory ();
            var option = StanOptions.GetDefaultOptions ();
            option.NatsURL = "nats://localhost:4222";
            Console.WriteLine (option.MaxPubAcksInFlight);

            Console.WriteLine (option.NatsURL);

            using (var c = cf.CreateConnection ("fzf003", "appname", option)) {

                using (c.Subscribe ("foo", (obj, e) => {
                    Console.WriteLine (e.Message.TimeStamp.ToLocalTime ());
                    Console.WriteLine (
                        System.Text.Encoding.UTF8.GetString (e.Message.Data));
                    e.Message.Ack ();
                })) {
                    for (;;) {
                        c.Publish ("foo", System.Text.Encoding.UTF8.GetBytes ("hello"));
                        Console.ReadKey ();
                    }

                }
            }

        }

    }
}