using RRQMProxy;
using RRQMSocket;
using RRQMSocket.RPC.WebApi;
using System;
using System.Security.Authentication;
using RRQMCore.Extensions;
using RRQMSocket.Http;

namespace DotnetCoreClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = CreateWebApiClient();
            WeatherForecastController weatherForecastController = new WeatherForecastController(client);

            var result = weatherForecastController.Get();
            Console.WriteLine(result.ToJsonString());
            Console.ReadKey();
        }


        private static WebApiClient CreateWebApiClient()
        {
            WebApiClient client = new WebApiClient();
            client.Setup(new RRQMConfig()
                .SetRemoteIPHost(new IPHost("https://localhost:5001"))
                .SetClientSslOption(new ClientSslOption() { TargetHost = "localhost", SslProtocols = SslProtocols.Tls12 }));
            client.Connect();
            Console.WriteLine("连接成功");
            return client;
        }
    }
}
