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
using RRQMSocket.WebSocket.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WSClientPerformanceDemo
{
    class Program
    {
        static int len = 1024*64;
        static void Main(string[] args)
        {
            Console.WriteLine("1.SuperSocket测试");
            Console.WriteLine("2.WebSocketSharp测试");
            Console.WriteLine("3.RRQMWebSocket测试");
            Console.WriteLine("4.WebSocketSharp wss测试");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestSuperSocket();
                        break;
                    }
                case "2":
                    {
                        TestWebSocketSharp();
                        break;
                    }
                case "3":
                    {
                        TestRRQMWebSocket();
                        break;
                    } 
                case "4":
                    {
                        TestWebSocketSharpWSS();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        static void TestRRQMWebSocket()
        {
            SimpleWSClient simpleWSClient = new SimpleWSClient();

            int count = 0;
            simpleWSClient.Received += (client, e) =>
            {
                if (e.PayloadData.Length != len)
                {
                    Console.WriteLine("数据错误。");
                }
                count++;
            };

            WSClientConfig config = new WSClientConfig();
            config.RemoteIPHost = new RRQMSocket.IPHost("127.0.0.1:7789");
            config.SeparateThreadSend = true;
            config.ReceiveType = RRQMSocket.ReceiveType.IOCP;

            simpleWSClient.Setup(config).Connect();
            Console.WriteLine("连接成功");


            Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine(count);
                    count = 0;
                    await Task.Delay(1000);
                }
            });

            byte[] data = new byte[len];
            new Random().NextBytes(data);
            while (true)
            {
                simpleWSClient.Send(data, 0, data.Length);

                //simpleWSClient.Send(Console.ReadLine());
            }
        }

        static void TestSuperSocket()
        {
            WebSocket4Net.WebSocket webSocket = new WebSocket4Net.WebSocket("ws://127.0.0.1:7789");
            webSocket.Open();

            int count = 0;

            webSocket.DataReceived += (sender, e) =>
            {
                if (e.Data.Length != len)
                {
                    Console.WriteLine("数据错误。");
                }
                count++;
            };

            Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine(count);
                    count = 0;
                    await Task.Delay(1000);
                }
            });

            byte[] data = new byte[len];
            new Random().NextBytes(data);
            while (true)
            {
                webSocket.Send(data,0,data.Length);//发送消息的函数
            }
        }

        static void TestWebSocketSharpWSS()
        {
            string webPath = "wss://mqtt.eclipseprojects.io/mqtt";
            WebSocketSharp.WebSocket webSocket = new WebSocketSharp.WebSocket(webPath);
            //webSocket.SslConfiguration = new WebSocketSharp.Net.ClientSslConfiguration() {  };
            webSocket.Connect();


            int count = 0;

            webSocket.OnMessage += (sender, e) =>
            {
                if (e.RawData.Length != len)
                {
                    Console.WriteLine("数据错误。");
                }
                count++;
            };

            Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine(count);
                    count = 0;
                    await Task.Delay(1000);
                }
            });

            byte[] data = new byte[len];
            new Random().NextBytes(data);
            while (true)
            {
                webSocket.Send(data);//发送消息的函数
            }
        }

        static void TestWebSocketSharp()
        {
            string webPath = "ws://127.0.0.1:7789";
            WebSocketSharp.WebSocket webSocket = new WebSocketSharp.WebSocket(webPath);
            webSocket.Connect();

            
            int count = 0;

            webSocket.OnMessage += (sender, e) =>
            {
                if (e.RawData.Length != len)
                {
                    Console.WriteLine("数据错误。");
                }
                count++;
            };

            Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine(count);
                    count = 0;
                    await Task.Delay(1000);
                }
            });

            byte[] data = new byte[len];
            new Random().NextBytes(data);
            while (true)
            {
                webSocket.Send(data);//发送消息的函数
            }
        }
    }
}
