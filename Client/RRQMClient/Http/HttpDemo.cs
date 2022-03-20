using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.Http
{
    public class HttpDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.简单Http测试");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestHttp();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void TestHttp()
        {
            HttpClient client = new HttpClient();

            client.AddPlugin<MyClass>();

            client.Setup(new RRQMConfig()
                .UsePlugin()
                .SetRemoteIPHost(new IPHost("https://localhost:7219"))
                .SetClientSslOption(new ClientSslOption() { TargetHost = "localhost", SslProtocols = SslProtocols.Tls12 }))
                .Connect();
           
            HttpRequest request = new HttpRequest();
            request
                .InitHeaders()
                .SetUrl("/WeatherForecast")
                .SetHost(client.RemoteIPHost.Host)
                .AsGet();

            var respose = client.Request(request,timeout:1000000);
            Console.WriteLine(respose.GetBody());
        }
    }

    class MyClass:TcpPluginBase
    {
        protected override void OnReceivedData(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            Console.WriteLine(e.RequestInfo);
            base.OnReceivedData(client, e);
        }
    }
}
