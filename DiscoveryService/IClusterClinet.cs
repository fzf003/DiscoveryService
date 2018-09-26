using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscoveryService {

    public interface IFrameworkProvider {
        Uri Start (string serviceName, string version,Action<Uri> action=null);
    }

    public interface IClusterClinet {
        Task<ServiceInformation[]> FindServiceInstancesAsync (string servicename);

        Task<ServiceInformation> FindServiceInstanceAsync (string servicename);

        Task KvPutAsync (string key, object value);

        Task<T> KvGetAsync<T> (string key);

        Task DeregisterServiceAsync ();

        Task<ServiceInformation> RegisterServiceAsync ();

    }

    public class ClusterClient : IClusterClinet {
        string serviceId = string.Empty;
        private readonly IClusterProvider _clusterProvider;
        private readonly IFrameworkProvider _frameworkProvider;

        private readonly ServiceConfig _serviceConfig;

        public ClusterClient (IClusterProvider clusterProvider, IFrameworkProvider frameworkProvider, ServiceConfig serviceConfig) {

            _frameworkProvider = frameworkProvider;
            var uri = _frameworkProvider.Start (serviceConfig.serviceName, serviceConfig.version);
            serviceId = serviceConfig.serviceId;
            _clusterProvider = clusterProvider;

        }

        public Task<ServiceInformation> RegisterServiceAsync () {
            return _clusterProvider.RegisterServiceAsync (this._serviceConfig.serviceName, serviceId, this._serviceConfig.version, this._serviceConfig.serviceUri);
        }

        public Task DeregisterServiceAsync () {
            if (string.IsNullOrWhiteSpace (serviceId)) {
                return Task.CompletedTask;
            } else {
                return _clusterProvider.DeregisterServiceAsync (serviceId);
            }
        }

        public Task<ServiceInformation[]> FindServiceInstancesAsync (string serviceName) {
            return _clusterProvider.FindServiceInstancesAsync (serviceName);
        }

        public Task<ServiceInformation> FindServiceInstanceAsync (string serviceName) {
            return _clusterProvider.FindServiceInstanceAsync (serviceName);
        }

        /* public   void BootstrapClient(IClusterProvider clusterProvider)
        {
            _clusterProvider = clusterProvider;
            _clusterProvider.BootstrapClientAsync().Wait();
        }
*/
        public Task KvPutAsync (string key, object value) {
            return _clusterProvider.KvPutAsync (key, value);
        }

        public Task<T> KvGetAsync<T> (string key) {
            return _clusterProvider.KvGetAsync<T> (key);
        }

        
    }
}