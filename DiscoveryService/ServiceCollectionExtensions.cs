using System;
using System.Net.Http;
using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace DiscoveryService
{

    public static class ServiceCollectionExtensions
    {
        public static void AddDiscoveryService(this IServiceCollection services, Action<ServiceConfig> options)
        {

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.Configure(options);


            services.AddSingleton<IConsulClient>(new ConsulClient(cfg =>
            {
                cfg.Address = new Uri("http://localhost:8500");
            }));

            services.AddHttpClient();

            services.AddSingleton<HttpClient>();

            services.AddSingleton<IClusterProvider, ConsulProvider>();

            services.AddSingleton<IClusterClinet, ClusterServiceClient>();

        }

        public static void AddDiscoveryServiceClient(this IServiceCollection services,Action<ConsulClientConfiguration> cfgaction)
        {
 
            services.AddSingleton<IConsulClient>(new ConsulClient(cfg =>
            {
                cfgaction(cfg);
             }));

            services.AddHttpClient();

            services.AddSingleton<HttpClient>();

            services.AddSingleton<IClusterProvider, ConsulProvider>();

            services.AddSingleton<IClusterClinet, ClusterServiceClient>();

        }










    }

}