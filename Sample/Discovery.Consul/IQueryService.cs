

using System.Threading.Tasks;
using DiscoveryService;
using Refit;
using System;
using Rebus.Handlers;
using System.Linq;
using Rebus.Bus;

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


    public interface IStart
    {
        void Start();
    }

    public class MyStart : IStart
    {
        public void Start()
        {
           Console.WriteLine("开始");
        }
    }


    public class Message1
    {
        public Message1(string name)
        {
            Id = Guid.NewGuid();
            Name=name;
        }
        public Guid Id { get; }
         public string Name { get; }

        public override string ToString()
        {
            return $"Message1 : {Id}";
        }
    }


     public class Handler1 : IHandleMessages<Message1>
    {
        public Task Handle(Message1 message)
        {
            Console.WriteLine($"Handler1 received : {message.Name}");

            return Task.CompletedTask;
        }
    }




}