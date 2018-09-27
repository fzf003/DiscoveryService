using System;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DiscoveryService {

    public static class ServiceCollectionExtensions {
        public static void AddDiscoveryService (this IServiceCollection services, Action<ServiceConfig> options) {

            if (options == null) {
                throw new ArgumentNullException (nameof (options));
            }

            services.Configure (options);

            services.AddSingleton<IConsulClient> (new ConsulClient (cfg => {
                cfg.Address = new Uri ("http://localhost:8500");
            }));

            services.AddHttpClient ();

            services.AddSingleton<HttpClient> ();

            services.AddSingleton<IClusterProvider, ConsulProvider> ();

            services.AddSingleton<IClusterClinet, ClusterServiceClient> ();

        }

        public static void AddDiscoveryServiceClient (this IServiceCollection services, Action<ConsulClientConfiguration> cfgaction) {

            services.AddSingleton<IConsulClient> (new ConsulClient (cfg => {
                cfgaction (cfg);
            }));

            services.AddHttpClient ();

            services.AddSingleton<HttpClient> ();

            services.AddSingleton<IClusterProvider, ConsulProvider> ();

            services.AddSingleton<IClusterClinet, ClusterServiceClient> ();

        }

        public static IApplicationBuilder UseDiscoveryService (this IApplicationBuilder appBuilder) {
            var appLifetime = appBuilder.ApplicationServices.GetService<IApplicationLifetime> ();

            appLifetime.ApplicationStarted.Register (() => {

                var serviceinfo = appBuilder.ApplicationServices.GetService<IOptions<ServiceConfig>> ();
                var clusterClinet = appBuilder.ApplicationServices.GetService<IClusterClinet> ();

                clusterClinet.RegisterServiceAsync (
                    serviceinfo.Value.serviceName,
                    serviceinfo.Value.serviceId,
                    serviceinfo.Value.version,
                    serviceinfo.Value.serviceUri
                );

            });

            appLifetime.ApplicationStopping.Register (() => {

                var serviceinfo = appBuilder.ApplicationServices.GetService<IOptions<ServiceConfig>> ();
                var cluster = appBuilder.ApplicationServices.GetService<IClusterClinet> ();
                cluster.DeregisterServiceAsync (serviceinfo.Value.serviceId);

            });

            appBuilder.MapWhen (context => {
                return context.Request.Path.Value.Contains ("/status");

            }, appbuild => {

                appbuild.Run (async context => {

                    await context.Response.WriteAsync ("ok!");

                });

            });

            return appBuilder;
        }

    }

}