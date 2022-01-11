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
using RRQMSocket.WebSocket;
using System;

namespace WSServicePerformanceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.SuperSocket测试");
            Console.WriteLine("2.WebSocketSharp测试");
            Console.WriteLine("3.RRQMWebSocket测试");
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
                default:
                    break;
            }
            Console.ReadKey();
        }
        static void TestRRQMWebSocket()
        {
            SimpleWSService simpleWSService = new SimpleWSService();
            simpleWSService.Received += (client, e) =>
            {
                //client.Send("123");
                client.Send(e.PayloadData);
            };
            WSServiceConfig config = new WSServiceConfig();
            config.ListenIPHosts = new RRQMSocket.IPHost[] { new RRQMSocket.IPHost(7789)};
            config.ReceiveType = RRQMSocket.ReceiveType.IOCP;
            simpleWSService.Setup(config).Start();
            Console.WriteLine("启动服务监听！");
        }
        static void TestWebSocketSharp()
        {
            WebSocketSharp.Server.WebSocketServer webSocketServer = new WebSocketSharp.Server.WebSocketServer(7789);
            webSocketServer.Start();
            Console.WriteLine("启动服务监听！");
        }

        static void TestSuperSocket()
        {
            SuperSocket.WebSocket.WebSocketServer webSocketServer = new SuperSocket.WebSocket.WebSocketServer();
            webSocketServer.NewMessageReceived += (session, value) =>
            {
                session.Send(value);
            };
            webSocketServer.NewDataReceived += (session, value) =>
            {
                session.Send(value, 0, value.Length);
            };
            if (!webSocketServer.Setup("127.0.0.1", 7789))
            {
                Console.WriteLine("设置服务监听失败！");
            }
            webSocketServer.Start();
            Console.WriteLine("启动服务监听！");
        }
    }
}
