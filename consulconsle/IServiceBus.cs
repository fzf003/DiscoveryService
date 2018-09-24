

using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace consulconsle
{
     public interface IServiceBus
    {
        Task SendAsync<TMessage>(string ChannelName, TMessage message) where TMessage : class;
        Guid Subscribe<TMessage>(string ChannelName, Action<TMessage> handlerAction) where TMessage : class;
        Guid Subscribe<TMessage>(string ChannelName, Func<TMessage, System.Threading.Tasks.Task> handlerAction) where TMessage : class;
        void Unsubscribe(string ChannelName);
    }

    public class RedisBus : IServiceBus
    {

      readonly IConnectionMultiplexer   _connectionMultiplexer;

        readonly ISerializationService _serializationService;

        readonly StackExchange.Redis.ISubscriber _subscriber;
 

        public RedisBus(IConnectionMultiplexer connectionMultiplexer, ISerializationService serializationService)
        {
  
            this._serializationService = serializationService;
             

            _connectionMultiplexer =  connectionMultiplexer;


            this._subscriber = this._connectionMultiplexer.GetSubscriber();
        }

        public Task SendAsync<TMessage>(string ChannelName, TMessage message) where TMessage : class
        {
            ;
            return this._subscriber.PublishAsync(ChannelName, this._serializationService.ToSerialization(message), CommandFlags.FireAndForget);
        }

        public Guid Subscribe<TMessage>(string ChannelName, Action<TMessage> handlerAction) where TMessage : class
        {

            this._subscriber.Subscribe(ChannelName, (channel, msg) =>
            {
               

                handlerAction(this._serializationService.Deserialization<TMessage>(msg));

            }, CommandFlags.FireAndForget);

            return Guid.Empty;
        }

        public Guid Subscribe<TMessage>(string ChannelName, Func<TMessage, System.Threading.Tasks.Task> handlerAction) where TMessage : class
        {

            _subscriber.Subscribe(ChannelName, (channel, msg) => handlerAction(this._serializationService.Deserialization<TMessage>(msg)));

            return Guid.Empty;
        }

        public void Unsubscribe(string ChannelName)
        {
             this._subscriber.Unsubscribe(ChannelName); ;
        }
    }


    public interface ISerializationService
    {
        string ToSerialization(object obj);

        object Deserialization(string objstr);

        T Deserialization<T>(string objstr) where T:class;

    }

    public class DefaultSerializationService : ISerializationService
    {


        public string ToSerialization(object obj)
        {
           return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public object Deserialization(string objstr)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(objstr);
        }


        public T Deserialization<T>(string objstr) where T:class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(objstr);
        }
    }

}