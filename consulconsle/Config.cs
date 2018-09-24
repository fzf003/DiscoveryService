using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Consul;
using DiscoveryService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace consulconsle {
    public interface IServiceHostProvider {
        IWebHostBuilder Start (string serviceName, string version);
    }

    public static class Configuration {
        public static Uri GetUri (string protocol = "http", int port = 0) {
            port = port == 0 ? FreeTcpPort () : port;
            var uri = $"{protocol}://localhost:{ port}/";
            return new Uri (uri);
        }



        public static int GetPort (int port = 0) {
            return port = port == 0 ? FreeTcpPort () : port;
        }

        private static int FreeTcpPort () {
            var l = new TcpListener (IPAddress.Loopback, 0);
            l.Start ();
            var port = ((IPEndPoint) l.LocalEndpoint).Port;
            l.Stop ();
            return port;
        }

        public static string GetLocalIP () {
            try {
                string HostName = Dns.GetHostName (); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry (HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++) {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                        return IpEntry.AddressList[i].ToString ();
                    }
                }
                return "";
            } catch (Exception ex) {

                return "";
            }
        }
    }

    public static class AppBuilderExtensions {
        public static IApplicationBuilder RegisterConsul (this IApplicationBuilder app, IApplicationLifetime lifetime) {

            var consulClient = app.ApplicationServices.GetService<IConsulClient> ();
            var endpoint = app.ApplicationServices.GetService<StartEndPoint> ();

            var httpCheck = new AgentServiceCheck () {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds (5), //服务启动多久后注册
                Interval = TimeSpan.FromSeconds (10), //健康检查时间间隔，或者称为心跳间隔
                HTTP = $"{endpoint.Url.AbsoluteUri}status", //健康检查地址
                Timeout = TimeSpan.FromSeconds (5)
            };

            Console.WriteLine (httpCheck.HTTP);

            // Register service with consul
            var registration = new AgentServiceRegistration () {
                Checks = new [] { httpCheck },
                ID = endpoint.ServiceId,
                Name = endpoint.serviceName,
                Address = endpoint.Url.Host,
                Port = endpoint.Url.Port,
                Tags = new [] { $"urlprefix-/webapp" } //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister (registration).Wait (); //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register (() => {
                consulClient.Agent.ServiceDeregister (registration.ID).Wait (); //服务停止时取消注册
            });

            return app;
        }
    }
}