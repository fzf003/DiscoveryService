using System;
using Consul;
using Microsoft.Extensions.DependencyInjection;

namespace DiscoveryService {

    public static class ServiceCollectionExtensions {

         public static IServiceCollection AddDiscoveryServiceClient (this IServiceCollection services, Action < ConsulClientConfiguration > cfgaction) {

            services.AddSingleton < IConsulClient > (new ConsulClient (cfg =>  {
                cfgaction (cfg); 
            })); 

            services.AddHttpClient (); 
  
            services.AddSingleton < IClusterProvider, ConsulProvider > (); 

            services.AddSingleton < IClusterClinet, ClusterServiceClient > (); 

            return services;

        }


        public static IServiceCollection AddDiscoveryService(this IServiceCollection services)
        {
   
           services.AddSingleton < IConsulClient > (new ConsulClient (cfg =>  {
                cfg.Address = new Uri ("http://localhost:8500");
           })); 

            services.AddHttpClient (); 

            services.AddSingleton < IClusterProvider, ConsulProvider > (); 

            services.AddSingleton < IClusterClinet, ClusterServiceClient > (); 

            return services;

        }
 

    }
}