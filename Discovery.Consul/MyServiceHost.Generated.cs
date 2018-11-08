using System.Threading;

namespace Discovery.Consul {

    public partial class MyServiceHost : IHost {

     public CancellationToken HostClosed=>this._cancellationTokenSource.Token;

     

     }

}