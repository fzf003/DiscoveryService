

using System.Threading.Tasks;
using DiscoveryService;
using Refit;

namespace Discovery.Consul
{
    public interface IQueryService
    {
        /// <summary>
        /// 获取所有节点该名称的服务
        /// </summary>
        /// <returns></returns>
        [Get("/GetServices/{servicename}")]
        Task<ServiceInformation[]> QuerySerivceList(string servicename);

        /// <summary>
        /// 获取该服务节点默认（轮询负载均衡）
        /// </summary>
        /// <returns></returns>
        [Get("/GetServiceInfo/{servicename}")]
        Task<ServiceInformation> QuerySerivce(string servicename);

    }

    public class QueryServiceOption
    {
        public string GatewayUrl{get;set;}
    }



    public interface IGateWayQueryService
    {
        Task<ServiceInformation> GetEndpoint(string servicename);
    }

    public class APiServiceQuery : IGateWayQueryService
    {

 

        readonly QueryServiceOption option;

        public APiServiceQuery(QueryServiceOption option)
        {

         

            this.option=option;

        }

        public async  Task<ServiceInformation> GetEndpoint(string servicename)
        {
             var queryclient = RestService.For<IQueryService>(option.GatewayUrl);

            return await queryclient.QuerySerivce(servicename);
        }

    }
}