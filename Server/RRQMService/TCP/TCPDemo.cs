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
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RRQMCore.Helper;
using RRQMSocket.FileTransfer;
using RRQMCore.ByteManager;
using System.Threading;
using RRQMSocket.Helper;

namespace RRQMService.TCP
{
    public static class TCPDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.IOCP简单TCP服务器（可兼容ClientTest-6）");
            Console.WriteLine("2.BIO简单TCP服务器（可兼容ClientTest-6）");
            Console.WriteLine("3.Select简单TCP服务器（可兼容ClientTest-6）");
            Console.WriteLine("4.测试TCP连接性能服务器");
            Console.WriteLine("5.测试TCP接收流量性能服务器");
            Console.WriteLine("6.测试TCP断线重连服务器");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TCPDemo.StartSimpleTcpService(ReceiveType.IOCP);
                        break;
                    }
                case "2":
                    {
                        TCPDemo.StartSimpleTcpService(ReceiveType.BIO);
                        break;
                    }
                case "3":
                    {
                        TCPDemo.StartSimpleTcpService(ReceiveType.Select);
                        break;
                    }
                case "4":
                    {
                        TCPDemo.StartConnectPerformanceTcpService();
                        break;
                    }
                case "5":
                    {
                        TCPDemo.StartFlowPerformanceTcpService();
                        break;
                    }
                case "6":
                    {
                        TCPDemo.StartBreakoutResumeTcpService();
                        break;
                    }
                default:
                    break;
            }
        }
        static void StartBreakoutResumeTcpService()
        {
            TcpService service = new TcpService();

            service.Connected += (client, e) =>
            {
                client.Send(Encoding.UTF8.GetBytes("RRQM"));
            };
            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 2000, (loop) =>
            {
                service.Stop();
                Console.WriteLine("服务器已停止");
                service.Start();
                Console.WriteLine("服务器已重新启动");
            });

            loopAction.RunAsync();

            Console.WriteLine($"TCP断线重连服务器启动成功");
        }
        static void StartSimpleTcpService(ReceiveType receiveType)
        {
            TcpService service = new TcpService();

            int count=0;
            service.Connecting += (client, e) =>
            {
                e.ID = Interlocked.Increment(ref count).ToString();//此处重新对ID赋值
            };

            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.Name}连接，ID={client.ID}"); };

            service.Disconnected += (client, e) => { Console.WriteLine($"客户端{client.Name}断开连接，原因：{e.Message}"); };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"已从{client.Name}接收到信息：{mes}");//Name即IP+Port
                client.Send(byteBlock);
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = receiveType;
            config.ClearInterval = 1000 * 10;
            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            Console.WriteLine($"{receiveType}服务器启动成功");
            Console.WriteLine($"输入“ID+空格+消息”的形式，给指定ID发送消息");

            while (true)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                if (inputs.Length<2)
                {
                    continue;
                }
                try
                {
                    service.Send(inputs[0],inputs[1]);
                    Console.WriteLine("发送成功");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static void StartConnectPerformanceTcpService()
        {
            TcpService service = new TcpService();

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = ReceiveType.Select;
            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
              {
                  Console.WriteLine($"在线客户端数量：{service.SocketClients.Count}");
              });

            loopAction.RunAsync();

            Console.WriteLine($"连接测试服务器启动成功");
        }
        static void StartFlowPerformanceTcpService()
        {
            TcpService service = new TcpService();

            long flow = 0;

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                flow += byteBlock.Len;
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = ReceiveType.IOCP;
            config.BufferLength = 1024 * 1024;
            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
              {
                  Console.WriteLine($"接收：{flow}字节，计：{FileUtility.ToFileLengthString(flow)}");
                  flow = 0;
              });

            loopAction.RunAsync();

            Console.WriteLine($"流量测试服务器启动成功");
        }
    }

    public class MyService : TcpService<MySocketClient>
    {
        protected override void LoadConfig(ServiceConfig serviceConfig)
        {
            //此处加载配置，用户可以从配置中获取配置项。
            base.LoadConfig(serviceConfig);
        }

        protected override void OnConnecting(MySocketClient socketClient, ClientOperationEventArgs e)
        {
            //对即将连接的客户端做初始化配置
            base.OnConnecting(socketClient, e);
        }
    }

    public class MySocketClient : SocketClient
    {
        protected override void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //此处处理数据，功能相当于Received事件。
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            Console.WriteLine($"已接收到信息：{mes}");
        }
    }
}
