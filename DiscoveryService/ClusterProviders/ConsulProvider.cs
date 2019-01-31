
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Newtonsoft.Json;

namespace DiscoveryService
{
 public class ConsulProvider : IClusterProvider
    {

        readonly IConsulClient consulClient;

        readonly  HashSet<string> lookup ;

        public ConsulProvider(IConsulClient consulClient)
        {
            this.consulClient = consulClient;

            this.lookup=new HashSet<string>();
        }


        public Task BootstrapClientAsync()
        {
            StartReaper();
            return Task.CompletedTask;
        }

        public async Task<ServiceInformation[]> FindServiceInstancesAsync(string name)
        {
            var services = await this.consulClient.Health.Service(name);

            var serviceEntrys = services.Response.Where(serviceEntry => QueryFilterService(serviceEntry, name));

            return serviceEntrys.Select(p => new ServiceInformation
            {
                Id = p.Service.ID,
                Name = p.Service.Service,
                Address = p.Service.Address,
                Port = p.Service.Port,
                Tags = p.Service.Tags

            }).ToArray();

        }


        bool QueryFilterService(ServiceEntry serviceEntry, string servicename)
        {
            var queryservicestatus = serviceEntry?.Checks.Any(pr => pr.Status == HealthStatus.Passing && pr.ServiceName == servicename);

            return queryservicestatus.Value;
        }


        public async Task<T> KvGetAsync<T>(string key)
        {

            var response = await this.consulClient.KV.Get(key);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not get value");
            }

            var strValue = Encoding.UTF8.GetString(response.Response.Value);

            return JsonConvert.DeserializeObject<T>(strValue);
        }

        public async Task KvPutAsync(string key, object value)
        {

            var pair = new KVPair(key)
            {
                Value = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))
            };

            var response = await this.consulClient.KV.Put(pair);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Could not get value");
            }

        }

        public async Task<ServiceInformation> RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri = null, IEnumerable<string> tags = null)
        {
            var tagList = (tags ?? Enumerable.Empty<string>()).ToList();
            tagList.Add($"urlprefix-/{serviceName}");
 
            AgentServiceRegistration asr = new AgentServiceRegistration()
            {
                Address = Dns.GetHostName(),
                Port = uri.Port,
                ID = serviceId,
                Name = serviceName,
                Tags = tagList.ToArray(),
                 
            };

            asr.Check = new AgentServiceCheck()
            {

                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//

                HTTP = $"http://{uri.Host}:{uri.Port}/status",

                Interval = TimeSpan.FromMilliseconds(2000),

                Timeout = TimeSpan.FromSeconds(5),
               
            };

            var response = await this.consulClient.Agent.ServiceRegister(asr);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("注册失败");
            }
 
            return new ServiceInformation
            {
                Id = asr.ID,
                Name = asr.Name,
                Address = asr.Address,
                Port = asr.Port,
                Version = version,
                Tags = asr.Tags
            };
        }

        public async Task<bool> DeregisterServiceAsync(string serviceId)
        {
            var response = await this.consulClient.Agent.ServiceDeregister(serviceId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("删除失败");
            }

            return true;
        }

        public async Task KvDeleteAsync(string key)
        {
            var response = await this.consulClient.KV.Delete(key);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("删除失败");
            }

            
        }

            private void StartReaper()
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1000).ConfigureAwait(false);
              //  Logger.Information("服务已启动");

                var c = this.consulClient;

                var lookup = new HashSet<string>();
                while (true)
                {
                    try
                    {
                     var res=await  c.Health.State(HealthStatus.Critical);
                     var queryrequest=res.Response.Select(p=>p.ServiceID).ToList();

                     foreach(var criticalServiceId in queryrequest)
                     {

                           if (lookup.Contains(criticalServiceId))
                            {
                               await DeregisterServiceAsync(criticalServiceId).ConfigureAwait(false);
                              Console.WriteLine("下线： {0}", criticalServiceId);
                               // await c.DeregisterServiceAsync(criticalServiceId).ConfigureAwait(false);
                               // Logger.Information("下线： {ServiceId}", criticalServiceId);
                            }

                     }

                     lookup.RemoveWhere(i => !queryrequest.Contains(i));
                        
                      
                    }
                    catch (Exception x)
                    {
                         Console.WriteLine($"{x} Crashed");
                    }

                    await Task.Delay(5000).ConfigureAwait(false);
                }
            });
        }

    
    }
}