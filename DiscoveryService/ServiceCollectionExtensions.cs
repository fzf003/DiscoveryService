using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace DiscoveryService {

    public static class ServiceCollectionExtensions {
        public static void AddDiscoveryService (this IServiceCollection services) {

            services.AddSingleton<IConsulClient> (new ConsulClient (cfg => {
                cfg.Address = new Uri ("http://localhost:8500");
            }));

            services.AddHttpClient ();

            services.AddSingleton<IClusterProvider, ConsulProvider> ();

    

        }
    }

}