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
using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace TokenClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择类型");
            Console.WriteLine("1.多租户Token客户端");
            Console.WriteLine("2.简单Token客户端");
            Console.WriteLine("3.测试连接性能");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateNormalTokenClient();
                        break;
                    }
                case "2":
                    {
                        CreateSimpleTokenClient();
                        break;
                    }
                case "3":
                    {
                        TestConnectPerformance();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        private static void TestConnectPerformance()
        {
            int count = 1000;

            List<SimpleTokenClient> clients = new List<SimpleTokenClient>();
            Console.WriteLine($"即将进行{count}次连接");
            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
              {
                  for (int i = 0; i < count; i++)
                  {
                      SimpleTokenClient tokenClient = new SimpleTokenClient();
                      clients.Add(tokenClient);
                      //声明配置
                      var config = new TokenClientConfig();
                      config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
                      //载入配置
                      tokenClient.Setup(config);

                      tokenClient.Connect("Token");
                  }
              });

            Console.WriteLine($"测试完成，用时:{timeSpan}");
        }

        private static void CreateSimpleTokenClient()
        {
            SimpleTokenClient tokenClient = new SimpleTokenClient();

            tokenClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tokenClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };

            tokenClient.Received += (client, byteBlock, obj) =>
            {
                //客户端接收信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"接收：{mes}");
            };

            //声明配置
            var config = new TokenClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            //载入配置
            tokenClient.Setup(config);

            Console.WriteLine("请输入连接Token");
            tokenClient.Connect(Console.ReadLine());

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tokenClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }

        private static void CreateNormalTokenClient()
        {
            MyTokenClient tokenClient = new MyTokenClient();

            tokenClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tokenClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };


            //声明配置
            var config = new TokenClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            //载入配置
            tokenClient.Setup(config);

            Console.WriteLine("请输入连接Token");
            tokenClient.Connect(Console.ReadLine());

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tokenClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }
    }
    class MyTokenClient : TokenClient
    {
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            Console.WriteLine($"接收到信息：{mes}");
        }
    }
}
