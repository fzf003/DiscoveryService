

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
 
namespace DiscoveryService
{

    public class RandomLoadBalancer : ILoadBalancer
    {
        private readonly Random _random;

        readonly List<ServiceInformation> servicestore;

        public RandomLoadBalancer(List<ServiceInformation> servicestore, int? seed = null)
        {
            this.servicestore = servicestore;
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }



        public async Task<ServiceInformation> GetServiceEndpoint(CancellationToken ct = default(CancellationToken))
        {
            var endpoints = servicestore;
            return await Task.FromResult(endpoints.Count == 0 ? null : endpoints[_random.Next(endpoints.Count)]);
        }
    }


    public class RoundRobinLoadBalancer : ILoadBalancer
    {

        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private int _index;

        readonly List<ServiceInformation> servicestore;

        public RoundRobinLoadBalancer(List<ServiceInformation> servicestore)
        {
            this.servicestore = servicestore;
        }

        public async Task<ServiceInformation> GetServiceEndpoint(CancellationToken ct = default(CancellationToken))
        {
            var endpoints = this.servicestore;
            if (endpoints.Count == 0)
            {
                return null;
            }

            await _lock.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                if (_index >= endpoints.Count)
                {
                    _index = 0;
                }
                var uri = endpoints[_index];
                _index++;

                return uri;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}