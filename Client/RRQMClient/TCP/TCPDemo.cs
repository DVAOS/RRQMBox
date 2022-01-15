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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.TCP
{
    public static class TCPDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.IOCP简单TCP客户端");
            Console.WriteLine("2.BIO简单TCP客户端");
            Console.WriteLine("3.Select简单TCP客户端");//客户端Select模型和BIO一样
            Console.WriteLine("4.测试TCP客户端连接性能");
            Console.WriteLine("5.测试TCP客户端发送流量性能");
            Console.WriteLine("6.测试同步发送并等待返回TCP客户端");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TCPDemo.StartSimpleTcpClient(ReceiveType.IOCP);
                        break;
                    }
                case "2":
                case "3":
                    {
                        TCPDemo.StartSimpleTcpClient(ReceiveType.Select);
                        break;
                    }
                case "4":
                    {
                        TCPDemo.StartConnectPerformanceTcpClient();
                        break;
                    }
                case "5":
                    {
                        TCPDemo.StartFlowPerformanceTcpClient();
                        break;
                    }
                case "6":
                    {
                        TCPDemo.CreateSendThenReturnTcpClient();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void CreateSendThenReturnTcpClient()
        {
            SendThenReturnTcpClient tcpClient = new SendThenReturnTcpClient();

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                byte[] data = tcpClient.SendThenReturn(Encoding.UTF8.GetBytes(Console.ReadLine()));
                Console.WriteLine($"同步收到：{Encoding.UTF8.GetString(data)}");
            }
        }

        static void StartSimpleTcpClient(ReceiveType receiveType)
        {
            SimpleTcpClient tcpClient = new SimpleTcpClient();

            tcpClient.Connected += (client, e) =>
            {
                Console.WriteLine(e.Message);
                //成功连接到服务器
            };
            tcpClient.Received += (client, byteBlock, obj) =>
            {
                //从服务器收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"已从服务器接收到信息：{mes}");
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
                Console.WriteLine(e.Message);
            };

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.ReceiveType = receiveType;
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tcpClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }

        static void StartConnectPerformanceTcpClient()
        {
            Console.WriteLine("按Enter键连接1000个客户端，按其他键，退出测试。");
            List<IClient> clients = new List<IClient>();

            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    break;
                }
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        SimpleTcpClient tcpClient = new SimpleTcpClient();

                        //声明配置
                        var config = new TcpClientConfig();
                        config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
                        config.ReceiveType = ReceiveType.IOCP;
                        config.OnlySend = true;//测试连接性能，启用仅发送功能，此时客户端不投递接收申请。
                                               //载入配置
                        tcpClient.Setup(config);

                        tcpClient.Connect();
                        clients.Add(tcpClient);
                        Console.WriteLine("连接成功");
                    }
                });

                Console.WriteLine($"测试完成，用时:{timeSpan}");

            }


        }
        static void StartFlowPerformanceTcpClient()
        {
            SimpleTcpClient tcpClient = new SimpleTcpClient();

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.ReceiveType = ReceiveType.IOCP;
            config.OnlySend = true;
            tcpClient.Setup(config);

            tcpClient.Connect();
            Console.WriteLine("连接成功");

            byte[] buffer = new byte[1024 * 1024];
            new Random().NextBytes(buffer);

            while (true)
            {
                tcpClient.Send(buffer);
            }
        }
    }
}
