using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscoveryService {

  
    public interface IClusterClinet {
        Task<ServiceInformation[]> FindServiceInstancesAsync (string servicename);

        Task<ServiceInformation> FindServiceInstanceAsync (string servicename);

        Task KvPutAsync (string key, object value);

        Task<T> KvGetAsync<T> (string key);

        Task DeregisterServiceAsync (string serviceId);
       

        Task<ServiceInformation> RegisterServiceAsync (string serviceName, string serviceId, string version, Uri uri = null, IEnumerable<string> tags = null);


    }

    public class ClusterServiceClient : IClusterClinet {
 
        private readonly IClusterProvider _clusterProvider;
  
        public ClusterServiceClient (IClusterProvider clusterProvider) {
          
            _clusterProvider = clusterProvider;
         }

        public Task<ServiceInformation> RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri = null, IEnumerable<string> tags = null)
         {
            return _clusterProvider.RegisterServiceAsync (serviceName, serviceId, version, uri,tags);
         }

        public Task DeregisterServiceAsync (string serviceId) {
            if (string.IsNullOrWhiteSpace (serviceId)) {
                return Task.CompletedTask;
            } else {
                return _clusterProvider.DeregisterServiceAsync(serviceId);
            }
        }

        public Task<ServiceInformation[]> FindServiceInstancesAsync (string serviceName) {
            return _clusterProvider.FindServiceInstancesAsync (serviceName);
        }

        public Task<ServiceInformation> FindServiceInstanceAsync (string serviceName) {
            return _clusterProvider.FindServiceInstanceAsync (serviceName);
        }
 
        public Task KvPutAsync (string key, object value) {
            return _clusterProvider.KvPutAsync (key, value);
        }

        public Task<T> KvGetAsync<T> (string key) {
            return _clusterProvider.KvGetAsync<T> (key);
        }

        
    }
}