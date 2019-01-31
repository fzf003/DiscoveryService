

using System;
using System.Threading.Tasks;
using System.Collections;
using Consul;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace DiscoveryService
{
    public interface IClusterProvider
    {
        Task<ServiceInformation[]> FindServiceInstancesAsync(string name);

        Task<ServiceInformation> RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri = null, IEnumerable<string> tags = null);

        Task BootstrapClientAsync();

        Task KvPutAsync(string key, object value);

        Task<T> KvGetAsync<T>(string key);

        Task KvDeleteAsync(string key);

        Task<bool> DeregisterServiceAsync(string serviceId);


    }

   



}