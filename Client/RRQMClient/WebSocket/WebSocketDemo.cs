//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using RRQMSocket.WebSocket;
using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace RRQMClient.WebSocket
{
    public static class WebSocketDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.测试WS接收和发送");
            Console.WriteLine("2.测试WS分片发送");
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
                default:
                    break;
            }
        }
        static void TestSubpackageClient()
        {
            WSClient myWSClient = new WSClient();
            myWSClient.Setup("http://127.0.0.1:7789/ws");
            myWSClient.Connect();
            Console.WriteLine("连接成功");

            byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            while (true)
            {
                Console.ReadKey();
                myWSClient.SubSendWithWS(data, 1, 8, 4);
            }
        }

        static void TestWSsClient()
        {
            WSClient myWSClient = new WSClient();

            myWSClient.Setup(new RRQMConfig()
                .SetRemoteIPHost(new IPHost("wss://127.0.0.1:7789/ws"))
                .SetClientSslOption(
                new ClientSslOption()
                {
                    ClientCertificates = new X509CertificateCollection() { new X509Certificate2("RRQMSocket.pfx", "RRQMSocket") },
                    SslProtocols = SslProtocols.Tls12,
                    TargetHost = "127.0.0.1",
                    CertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; }
                }))
                .Connect();

            Console.WriteLine("连接成功");
            while (true)
            {
                myWSClient.SendWithWS(Console.ReadLine());
            }
        }

        static void TestClient()
        {
            WSClient myWSClient = new WSClient();

            myWSClient.Received += MyWSClient_Received;
            myWSClient.Setup("ws://127.0.0.1:7789/ws");
            myWSClient.Connect();
            Console.WriteLine("连接成功");
            while (true)
            {
                myWSClient.SendWithWS(Console.ReadLine());
            }
        }

        private static void MyWSClient_Received(WSClient client, WSDataFrame dataFrame)
        {
            switch (dataFrame.Opcode)
            {
                case WSDataType.Cont:
                    Console.WriteLine($"收到中间数据，长度为：{dataFrame.PayloadLength}");
                    break;
                case WSDataType.Text:
                    Console.WriteLine(dataFrame.ToText());
                    break;
                case WSDataType.Binary:
                    if (dataFrame.FIN)
                    {
                        Console.WriteLine($"收到二进制数据，长度为：{dataFrame.PayloadLength}");
                    }
                    else
                    {
                        Console.WriteLine($"收到未结束的二进制数据，长度为：{dataFrame.PayloadLength}");
                    }
                    break;
                case WSDataType.Close:
                    break;
                case WSDataType.Ping:
                    break;
                case WSDataType.Pong:
                    break;
                default:
                    break;
            }
        }
    }
}
