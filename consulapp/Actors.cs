using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Proto;

namespace consulapp {
  public class DIActor : IActor {

    readonly ILogger logger;
    public DIActor () {

      this.logger = Proto.Log.CreateLogger ("DIACTOR");
    }
    public Task ReceiveAsync (IContext context) {
      Console.WriteLine (context.Message);
      if(context.Message is Proto.Started)
      {
      //Console.WriteLine ("my blog kiss", context.Message);

      Start();
      }

      return Actor.Done;
    }

   public static void Start () {
      var process = new System.Diagnostics.Process {
        StartInfo = new ProcessStartInfo {
        FileName = @"J:\dotnetapp\netfullapp\consulapp\consulapp\console\TestConsole.exe",
        Arguments = null,
        UseShellExecute = false,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        CreateNoWindow=true
        },
        EnableRaisingEvents = true
      };

      process.Start();

      process.OutputDataReceived += (sender, e) => {
        Console.WriteLine("数据接收:"+e.Data);

      };
      process.ErrorDataReceived += (sender, e) => {

         Console.WriteLine("数据出错:"+e.Data);

      };
      process.Exited += (sender, e) => {
         Console.WriteLine("数据推出:"+e);

      };
    }
  }
}