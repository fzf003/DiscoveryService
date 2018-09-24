

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DiscoveryService
{

    public interface ILoadBalancer
    {
        Task<ServiceInformation> GetServiceEndpoint(CancellationToken ct = default(CancellationToken));
    }

    
}