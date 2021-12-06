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
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择服务类型");
            Console.WriteLine("1.普通TCP客户端");
            Console.WriteLine("2.TCP性能连接");
            Console.WriteLine("3.BIO单线程拥塞TCP客户端");
            Console.WriteLine("4.发送流量测试");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateNormalTcpClient();
                        break;
                    }
                case "2":
                    {
                        TestConnectPerformance();
                        break;
                    } 
                case "3":
                    {
                        CreateBIOTcpClient();
                        break;
                    }
                case "4":
                    {
                        CreateFlowPerformance();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        private static void CreateFlowPerformance()
        {
            Console.WriteLine("测试之前，请确认服务器不处理接收数据，不然无法测试真实水平");
            Console.ReadKey();

            SimpleTcpClient tcpClient = new SimpleTcpClient();

            tcpClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.DataHandlingAdapter = new NormalDataHandlingAdapter();//设置数据处理适配器
            config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送
            config.ReceiveType = ReceiveType.BIO;//拥塞接收
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            byte[] data = new byte[1024*1000];
            new Random().NextBytes(data);

            long flow = 0;

            LoopAction loopAction = LoopAction.CreateLoopAction(-1,1000,(loop)=> 
            {
                Console.WriteLine($"已发送：{(flow/(1024*1024.0)).ToString("0.00")}Mb");
                flow = 0;
            });

            loopAction.RunAsync();

            Task.Run(()=> 
            {
                while (true)
                {
                    tcpClient.Send(data);
                    flow += data.Length;
                }
            });
        }

        private static void CreateBIOTcpClient()
        {
            SimpleTcpClient tcpClient = new SimpleTcpClient();

            tcpClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };

            tcpClient.Received += (client, byteBlock, obj) =>
            {
                //客户端接收信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"接收：{mes}");
            };

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.DataHandlingAdapter = new NormalDataHandlingAdapter();//设置数据处理适配器
            config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送
            config.ReceiveType = ReceiveType.BIO;//拥塞接收
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            tcpClient.Send(Encoding.UTF8.GetBytes("RRQM"));

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tcpClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }

        private static void TestConnectPerformance()
        {
            int count = 1000;
            Console.WriteLine($"即将进行{count}次连接");
            List<SimpleTcpClient> clients = new List<SimpleTcpClient>();
            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    SimpleTcpClient tcpClient = new SimpleTcpClient();

                    clients.Add(tcpClient);

                    //声明配置
                    var config = new TcpClientConfig();
                    config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
                    config.BufferLength = 1024 * 64;//缓存池容量
                    config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
                    config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
                    config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
                    config.DataHandlingAdapter = new NormalDataHandlingAdapter();//设置数据处理适配器
                    config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
                    config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送

                    //载入配置
                    tcpClient.Setup(config);

                    tcpClient.Connect();
                }
            });

            Console.WriteLine($"测试完成，用时:{timeSpan}");
        }

        private static void CreateNormalTcpClient()
        {
            MyTcpClient tcpClient = new MyTcpClient();

            tcpClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };


            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.DataHandlingAdapter = new NormalDataHandlingAdapter();//设置数据处理适配器
            config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送

            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tcpClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }
    }
    class MyTcpClient : RRQMSocket.TcpClient
    {
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            Console.WriteLine($"接收到信息：{mes}");
        }
    }
}
