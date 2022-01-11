using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.Ssl
{
    public static class SslTCP
    {
        public static void Start()
        {
            Console.WriteLine("1.BIO简单SslTCP服务器");
            Console.WriteLine("2.Select简单SslTCP服务器");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        StartSslTcpService(ReceiveType.BIO);
                        break;
                    }
                case "2":
                    {
                        StartSslTcpService(ReceiveType.Select);
                        break;
                    }
                default:
                    break;
            }
        }
        static void StartSslTcpService(ReceiveType receiveType)
        {
            SimpleTcpService service = new SimpleTcpService();

            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.Name}连接"); };

            service.Disconnected += (client, e) => { Console.WriteLine($"客户端{client.Name}断开连接，原因：{e.Message}"); };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"已从{client.Name}接收到信息：{mes}");//Name即IP+Port
                client.Send(byteBlock);
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = receiveType;
            config.SslOption = new ServiceSslOption() { Certificate = new X509Certificate2("RRQMSocket.pfx", "RRQMSocket"), SslProtocols = SslProtocols.Tls12 };
            //载入配置
            service.Setup(config);

            //启动
            service.Start();


            Console.WriteLine($"Ssl {receiveType}服务器启动成功");
        }
    }
}
