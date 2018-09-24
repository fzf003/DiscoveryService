

using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using DiscoveryService;
namespace consulconsle {
  public interface IHost : IDisposable
    {
        void Run();
        void Stop();

    }

    public class MyServiceHost : IHost
    {
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        readonly ManualResetEvent _workerShutDown = new ManualResetEvent(false);

        readonly IServiceCollection services = new ServiceCollection();

        ServiceProvider serviceProvider;

        public MyServiceHost()
        {

        }

        public virtual void OptionsConfigurat(IServiceCollection services)
        {
services.AddDiscoveryService();
services.AddSingleton<IClusterClinet,ClusterClient>();


        }

        public void Run()
        {

            OptionsConfigurat(services);

            serviceProvider=services.BuildServiceProvider();


            var token = _cancellationTokenSource.Token;

            token.Register(() =>
            {

                Console.WriteLine("正在停止。。。。。");

            });

            Console.WriteLine("运行中.....");


            this._workerShutDown.Set();
        }


        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
            this.serviceProvider?.Dispose();
        }

        public void Dispose()
        {
            Stop();

            if (!_workerShutDown.WaitOne(1000))
            {
                Console.WriteLine("The {workerName} worker did not shut down within {shutdownTimeoutSeconds} seconds!", 1000);

            }
            else
            {
                Console.WriteLine("已关闭。。。。");

            }



        }


    }
}