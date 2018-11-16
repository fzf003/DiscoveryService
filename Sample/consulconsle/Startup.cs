using System;
using System.IO;
using System.Net.Http;
using DiscoveryService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
namespace consulconsle {
    public class Startup {
        public Startup (IHostingEnvironment env) {
            var builder = new ConfigurationBuilder ()
                .SetBasePath (env.ContentRootPath)
                .AddJsonFile ("appsettings.json")
                .AddEnvironmentVariables ();

            Configuration = builder.Build ();

        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services) {

            services.AddOptions ();

            services.AddSingleton<ISerializationService, DefaultSerializationService> ();

            services.AddSingleton<IConnectionMultiplexer> ((prov) => {

                return ConnectionMultiplexer.Connect ("127.0.0.1:6379");
            });

            services.AddSingleton<IServiceBus, RedisBus> ();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();

            services.AddSwaggerGen (c => {
                 c.SwaggerDoc ("v1", new Info { Title = "apiservice", Version = "v1" });
             });

            services.AddMvcCore ().AddApiExplorer ();

            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);

        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }

            app.UseMvc ();

            app.UseMvcWithDefaultRoute ();

            app.UseDiscoveryService ();

            app.MapWhen (context => {
                return context.Request.Path.Value.Contains ("/hello");

            }, appbuild => {

                appbuild.Run (async context => {

                    await context.Response.WriteAsync (context.Connection.LocalIpAddress + "==" + context.Connection.LocalPort);

                    await context.Response.WriteAsync (context.Connection.RemoteIpAddress + "==" + context.Connection.RemotePort);

                });

            });

            app.UseSwagger (c => { });
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "apiservice");
            });

          

        }
    }
}