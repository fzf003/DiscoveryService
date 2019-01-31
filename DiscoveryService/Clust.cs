namespace DiscoveryService
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    /* public static class Cluster
    {
        private static IClusterProvider _clusterProvider;

        private static IServiceUrlProvider _serviceUrlProvider;

        public static async Task<ServiceInformation[]> FindServiceInstances(string name, params string[] versions)
        {
            if (versions.Any())
            {
                return await _clusterProvider.FindServiceInstancesAsync(name).Where(p => versions.Contains(p.Version)).ToArray();
            }
            return _clusterProvider.FindServiceInstancesAsync(name);
        }

        public static ServiceInformation FindServiceInstance(string name,LoadBalancer loadbalancer =LoadBalancer.RoundRobin, params string[] versions)
        {
            return _clusterProvider.FindServiceInstance(name, loadbalancer,versions);
        }

        public static void BootstrapClient(IClusterProvider clusterProvider)
        {
            _clusterProvider = clusterProvider;
            _clusterProvider.BootstrapClient();
        }

        public static void KvPutAsync(string key, object value)
        {
              _clusterProvider.KvPut(key, value);
        }

        public static T KvGet<T>(string key)
        {
              return _clusterProvider.KvGet<T>(key);
        }

        public static string KvGetString(string key)
        {
            return _clusterProvider.KvGetString(key);
        }

        public static Uri Bootstrap(IServiceUrlProvider serviceUrlProvider, IClusterProvider clusterProvider, string serviceName, string version)
        {
            try
            {
                _serviceUrlProvider = serviceUrlProvider;
                var uri =_serviceUrlProvider.GetUri(serviceName, version);
                var serviceId = string.Format("{0}-{1}", serviceName, Guid.NewGuid().ToString("N"));
                _clusterProvider = clusterProvider;
                _clusterProvider.RegisterService(serviceName, serviceId, version, uri);
                return uri;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }


        public static Uri BootstrapWebServer(string webserverurl, IClusterProvider clusterProvider, string serviceName, string version)
        {
            try
            {
                var uri = new Uri(webserverurl); 
                var serviceId = string.Format("{0}-{1}", serviceName, Guid.NewGuid().ToString("N"));
                _clusterProvider = clusterProvider;
                _clusterProvider.RegisterService(serviceName, serviceId, version, uri);
                return uri;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    } */
}