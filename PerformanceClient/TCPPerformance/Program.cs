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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TCPPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.测试1000连接10次");
            Console.WriteLine("2.测试10000连接，分10组，每组独立线程发送，每个客户端每次发送10byte数据");
            Console.WriteLine("3.测试10连接，每组独立线程发送10w次64K数据");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Test01();
                        break;
                    }
                case "2":
                    {
                        Test02();
                        break;
                    }
                case "3":
                    {
                        Test03();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        static void Test01()
        {
            TimeSpan time = TimeSpan.Zero;
            for (int i = 0; i < 10; i++)
            {
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                {
                    List<Socket> sockets = new List<Socket>();
                    for (int j = 0; j < 1000; j++)
                    {
                        sockets.Add(GetSocket());
                    }

                    foreach (var item in sockets)
                    {
                        item.Dispose();
                    }
                });
                time += timeSpan;
                Console.WriteLine(timeSpan);
            }
            Console.WriteLine($"总用时:{time}");
        }

        static void Test02()
        {
            stopwatch = new Stopwatch();
            List<List<Socket>> socketsCollection = new List<List<Socket>>();
            for (int i = 0; i < 10; i++)
            {
                List<Socket> sockets = new List<Socket>();
                for (int j = 0; j < 1000; j++)
                {
                    sockets.Add(GetSocket());
                }
                socketsCollection.Add(sockets);
            }

            stopwatch.Start();
            foreach (var item in socketsCollection)
            {
                SocketSend(item);
            }
        }

        static void Test03()
        {
            stopwatch = new Stopwatch();
            List<Socket> sockets = new List<Socket>();
            for (int j = 0; j < 10; j++)
            {
                sockets.Add(GetSocket());
            }
            stopwatch.Start();
            foreach (var item in sockets)
            {
                SocketSend(item);
            }
        }

        static void SocketSend(Socket socket)
        {
            byte[] data = new byte[64*1024];
            new Random().NextBytes(data);

            Task.Run(() =>
            {
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        try
                        {
                            socket.Send(data);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"异常退出,{ex.Message}");
                            break;
                        }
                    }
                });
                ShowTime(timeSpan);
            });
        }

        static void SocketSend(List<Socket> sockets)
        {
            byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };


            Task.Run(() =>
            {
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                  {
                      for (int i = 0; i < 1000; i++)
                      {
                          try
                          {
                              foreach (var item in sockets)
                              {
                                  item.Send(data);
                              }
                          }
                          catch (Exception ex)
                          {
                              Console.WriteLine($"异常退出,{ex.Message}");
                              break;
                          }
                      }
                  });
                ShowTime(timeSpan);
            });
        }

        static Stopwatch  stopwatch;
        static void ShowTime(TimeSpan timeSpan)
        {
            Console.WriteLine($"当前用时:{timeSpan},当前总用时：{stopwatch.Elapsed}");
        }

        static IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7789);
        static Socket GetSocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
            return socket;
        }
    }
}
