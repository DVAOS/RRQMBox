//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RRQMCore.ByteManager;
using RRQMSocket;

namespace SocketStressTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.测试连接");
            Console.WriteLine("2.测试使用固定包头发送");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestConnect();
                        break;
                    }
                case "2":
                    {
                        TestFixedHeaderSend();
                        break;
                    }
                default:
                    break;
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 测试连接
        /// </summary>
        static void TestConnect()
        {
            Task.Run(() =>
            {
                int count = 1000;
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                {
                    for (int i = 0; i < count; i++)
                    {
                        Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7789));
                        tcpClient.Dispose();
                    }
                });
                Console.WriteLine($"Socket连接{count}次，用时{timeSpan}");
            });

            Task.Run(() =>
            {
                int count = 1000;
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                  {
                      for (int i = 0; i < count; i++)
                      {
                          RRQMSocket.TcpClient tcpClient = new RRQMSocket.TcpClient();
                          tcpClient.Connect(new IPHost("127.0.0.1:7789"));
                          tcpClient.Dispose();
                      }
                  });
                Console.WriteLine($"普通TCP连接{count}次，用时{timeSpan}");
            });

            Task.Run(() =>
            {
                int count = 1000;
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                  {
                      for (int i = 0; i < count; i++)
                      {
                          TokenTcpClient client = new TokenTcpClient();
                          client.VerifyToken = "ABC";
                          client.Connect(new IPHost("127.0.0.1:7791"));
                          client.Dispose();
                      }
                  });
                Console.WriteLine($"TokenTCP连接{count}次，用时{timeSpan}");
            });
        }

        /// <summary>
        /// 测试使用固定包头发送
        /// </summary>
        static void TestFixedHeaderSend()
        {
            RRQMSocket.TcpClient tcpClient = new RRQMSocket.TcpClient();
            tcpClient.OnReceived += TcpClient_OnReceived;
            tcpClient.DataHandlingAdapter = new FixedHeaderDataHandlingAdapter();
            tcpClient.Connect(new IPHost("127.0.0.1:7789"));

            byte[] data = Encoding.UTF8.GetBytes("RRQM");
            Task.Run(() =>
            {
                for (int j = 0; j < 10; j++)
                {
                    int count = 1000000;
                    TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                    {
                        for (int i = 0; i < count; i++)
                        {
                            tcpClient.Send(data);
                        }
                    });
                    Console.WriteLine($"TCP发送{count}次，用时{timeSpan}");
                }

            });
        }

        private static int count;
        private static void TcpClient_OnReceived(RRQMSocket.TcpClient arg1, ByteBlock byteBlock, object arg3)
        {
            //count++;
            //if (count % 1000 == 0)
            //{
            //    string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
            //    Console.WriteLine($"已接收到信息：{mes},第{count}条");
            //}
        }
    }
}
