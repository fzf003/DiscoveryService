using System;
using System.Threading;
using DiscoveryService;
using Microsoft.Extensions.DependencyInjection;
namespace Discovery.Consul {

    public interface IHost : IDisposable {
        void Run ();
        void Stop ();

    }

    public partial class MyServiceHost : IHost {
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource ();
        readonly ManualResetEvent _workerShutDown = new ManualResetEvent (false);
        readonly IServiceCollection services;

        ServiceProvider applicationServices;

        public ServiceProvider ApplicationServices { get => applicationServices; set => applicationServices = value; }

        public MyServiceHost (Action<IServiceCollection> servicesbuild=null) {

            this.services = new ServiceCollection ();
            servicesbuild?.Invoke(this.services);

        }

        protected virtual void OnStart()
        {

        }

        public void Run () {

            ApplicationServices = services.BuildServiceProvider ();

            this.HostClosed.Register (() => {
                Console.WriteLine ("正在停止。。。。。");
            });

            OnStart();

            this._workerShutDown.Set ();
        }

        public void Stop () {
            _cancellationTokenSource?.Cancel ();
            this.ApplicationServices?.Dispose ();
        }

        public void Dispose () {
            Stop ();

            if (!_workerShutDown.WaitOne (1000)) {
                Console.WriteLine ("The {workerName} worker did not shut down within {shutdownTimeoutSeconds} seconds!", 1000);

            } else {
                Console.WriteLine ("已关闭。。。。");

            }

        }

    }


    public class HelloHost:MyServiceHost
    {
        
    }
}