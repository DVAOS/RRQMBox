//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using RRQMSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.WebSocket
{
    public static class WebSocketDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.测试WS接收和发送");
            Console.WriteLine("2.测试WS分片发送");
            Console.WriteLine("3.测试WSs接收和发送");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestClient();
                        break;
                    }
                case "2":
                    {
                        TestSubpackageClient();
                        break;
                    }
                case "3":
                    {
                        TestWSsClient();
                        break;
                    }
                default:
                    break;
            }
        }
        static void TestSubpackageClient()
        {
            MyWSClient myWSClient = new MyWSClient();
            WSClientConfig config = new WSClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");
            myWSClient.Setup(config);
            myWSClient.Connect();
            Console.WriteLine("连接成功");

            byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            while (true)
            {
                Console.ReadKey();
                myWSClient.SubpackageSend(data, 1, 8, 4);
            }
        }

        static void TestWSsClient()
        {
            MyWSClient myWSClient = new MyWSClient();
            WSClientConfig config = new WSClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");
            config.SslOption = new ClientSslOption()
            {
                ClientCertificates = new X509CertificateCollection() { new X509Certificate2("RRQMSocket.pfx", "RRQMSocket") },
                SslProtocols = SslProtocols.Tls12,
                TargetHost = "127.0.0.1",
                CertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; }
            };
            config.ReceiveType = ReceiveType.BIO;
            myWSClient.Setup(config);
            myWSClient.Connect();
            Console.WriteLine("连接成功");
            while (true)
            {
                myWSClient.Send(Console.ReadLine());
            }
        }

        static void TestClient()
        {
            MyWSClient myWSClient = new MyWSClient();
            WSClientConfig config = new WSClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");
            myWSClient.Setup(config);
            myWSClient.Connect();
            Console.WriteLine("连接成功");
            while (true)
            {
                myWSClient.Send(Console.ReadLine());
            }
        }
    }
}
