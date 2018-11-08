using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proto;
namespace consulapp {

    public class Startup {
        public static string[] Args { get; set; } = new string[] { };

           public  IConfigurationRoot Configuration { get; set; }


        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
               // .AddJsonFile("appsettings.json", optional:true)
                .AddEnvironmentVariables()
                .AddCommandLine(Startup.Args);   

               // Configuration=builder.
        }

        public void ConfigureServices (IServiceCollection services) {
            services.AddProtoActor ();
            // services.AddTransient<IActorManager, ActorManager>();
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime) {
            loggerFactory.AddConsole (LogLevel.Debug);
            //attach logger to proto logging just in case
            Proto.Log.SetLoggerFactory (loggerFactory);

            appLifetime.ApplicationStarted.Register (() => {

                var factory = app.ApplicationServices.GetService<IActorFactory> ();
                var actor = factory.GetActor<DIActor> ();

                actor.Tell ("sss");
            });

            //This is only for demo purposes done in the service initialization
            /*   var actorManager = app.ApplicationServices.GetRequiredService<IActorManager>();
               actorManager.Activate();
               //never do this
               Thread.Sleep(TimeSpan.FromSeconds(2));
               //notice, there is no second creation!
               actorManager.Activate();*/
        }
    }
}