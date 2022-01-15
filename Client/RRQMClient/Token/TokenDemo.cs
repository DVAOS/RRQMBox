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

namespace RRQMClient.Token
{
    public static class TokenDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.简单Token客户端(兼容测试多租户)");
            Console.WriteLine("2.测试Token客户端连接性能");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        StartSimpleTokenClient();
                        break;
                    }
                case "2":
                    {
                        StartConnectPerformanceTokenClient();
                        break;
                    }
                default:
                    break;
            }
        }

        static void StartConnectPerformanceTokenClient()
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
                        SimpleTokenClient client = new SimpleTokenClient();

                        //声明配置
                        var config = new TcpClientConfig();
                        config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
                        client.Setup(config);

                        client.Connect("Token");
                        clients.Add(client);
                        Console.WriteLine("连接成功");
                    }
                });

                Console.WriteLine($"测试完成，用时:{timeSpan}");
                
            }


        }

        static void StartSimpleTokenClient()
        {
            SimpleTokenClient tcpClient = new SimpleTokenClient();

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
            //载入配置
            tcpClient.Setup(config);

            while (true)
            {
                try
                {
                    Console.WriteLine("请输入连接令箭。");
                    tcpClient.Connect(Console.ReadLine());
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tcpClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }
    }
}
