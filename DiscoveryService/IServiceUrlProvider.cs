using System;

namespace DiscoveryService
{
   public interface IServiceUrlProvider
    {
        Uri GetUri(string serviceName, string version);
    }


    public class ServiceUrlProvider : IServiceUrlProvider
    {
        public Uri GetUri(string serviceName, string version)
        {
            return UriConfiguration.GetUri();
        }
    }
}