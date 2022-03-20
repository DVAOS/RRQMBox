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
using RRQMCore;
using RRQMCore.ByteManager;
using RRQMCore.Log;
using RRQMCore.Run;
using RRQMSocket;
using RRQMSocket.FileTransfer;
using RRQMSocket.Plugins;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMService.TCP
{
    public static class TCPDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.IOCP简单TCP服务器（可兼容ClientTest-6）");
            Console.WriteLine("2.测试TCP命令行插件");
            Console.WriteLine("3.测试TCP连接性能服务器");
            Console.WriteLine("4.测试TCP接收流量性能服务器");
            Console.WriteLine("5.测试TCP断线重连服务器");
            Console.WriteLine("6.测试TCP服务器插件");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TCPDemo.StartSimpleTcpService();
                        break;
                    }
                case "2":
                    {
                        TCPDemo.StartCommandLineTcpService();
                        break;
                    }
                case "3":
                    {
                        TCPDemo.StartConnectPerformanceTcpService();
                        break;
                    }
                case "4":
                    {
                        TCPDemo.StartFlowPerformanceTcpService();
                        break;
                    }
                case "5":
                    {
                        TCPDemo.StartBreakoutResumeTcpService();
                        break;
                    }
                case "6":
                    {
                        TCPDemo.StartPlugTcpService();
                        break;
                    }
                default:
                    break;
            }
        }

        static void StartCommandLineTcpService()
        {
            TcpService service = new TcpService();

            service.Connecting += (client, e) =>
            {
                e.DataHandlingAdapter = new TerminatorPackageAdapter(1024, "\r\n");//命令行中使用\r\n结尾
                //e.DataHandlingAdapter = new NormalDataHandlingAdapter();//亦或者省略\r\n，但此时调用方不能高速调用，会粘包
            };
            service.AddPlugin<MyCommandLinePlugin>();

            //声明配置
            var config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) }) //同时监听两个地址
                .UsePlugin();

            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            Console.WriteLine($"TCP命令行插件服务器启动成功");
        }

        static void StartPlugTcpService()
        {
            TcpService service = new TcpService();

            service.AddPlugin<MyPlug1>();
            service.AddPlugin<MyPlug2>();

            //声明配置
            var config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) }) //同时监听两个地址
                .UsePlugin();

            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            Console.WriteLine($"TCP插件服务器启动成功");
        }

        static void StartBreakoutResumeTcpService()
        {
            TcpService service = new TcpService();

            service.Connected += (client, e) =>
            {
                client.Send(Encoding.UTF8.GetBytes("RRQM"));
                Console.WriteLine($"{client.ID}连接");
            };

            service.Disconnected += (client, e) =>
            {
                Console.WriteLine($"{client.ID}断开连接");
            };
            //声明配置
            var config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) });//同时监听两个地址
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
        static void StartSimpleTcpService()
        {
            TcpService service = new TcpService();

            int count = 0;
            service.Connecting += (client, e) =>
            {
                e.ID = Interlocked.Increment(ref count).ToString();//此处重新对ID赋值
            };

            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.IP}:{client.Port}连接，ID={client.ID}"); };

            service.Disconnected += (client, e) => { Console.WriteLine($"客户端{client.IP}:{client.Port}断开连接，原因：{e.Message}"); };

            service.Received += (client, byteBlock, requestInfo) =>
            {
                //从客户端收到信息
                string mes = byteBlock.ToString();
                Console.WriteLine($"已从{client.IP}:{client.Port}接收到信息：{mes}");//Name即IP+Port
                client.Send(byteBlock);
            };

            //声明配置
            var config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) });//同时监听两个地址

            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            Console.WriteLine($"服务器启动成功");
            Console.WriteLine($"输入“ID+空格+消息”的形式，给指定ID发送消息");

            while (true)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                if (inputs.Length < 2)
                {
                    continue;
                }
                try
                {
                    service.Send(inputs[0], inputs[1]);
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
            var config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })//同时监听两个地址
                .SetMaxCount(10000);

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

            service.Received += (client, byteBlock, requestInfo) =>
            {
                //从客户端收到信息
                flow += byteBlock.Len;
            };

            //声明配置
            var config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })//同时监听两个地址
                .SetBufferLength(1024 * 1024)
                .SetThreadCount(10);

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

    /// <summary>
    /// 命令行插件。
    /// 声明的方法必须以"Command"结尾，支持json字符串，参数之间空格隔开。
    /// </summary>
    class MyCommandLinePlugin : TcpCommandLinePlugin
    {
        public int AddCommand(int a, int b)
        {
            return a + b;
        }

        public SumClass SumCommand(SumClass sumClass)
        {
            sumClass.Sum = sumClass.A + sumClass.B;
            return sumClass;
        }
    }

    class SumClass
    {
        public int A { get; set; }
        public int B { get; set; }
        public int Sum { get; set; }

    }


    /// <summary>
    /// 从接口实现
    /// </summary>
    class MyPlug1 : ITcpPlugin
    {
        public IService Service { get; set; }
        public IPluginsManager PluginsManager { get; set; }
        public byte Order { get; set; }
        public ILog Logger { get; set; }

        public void Dispose()
        {

        }

        public void OnConnected(ITcpClientBase client, RRQMEventArgs e)
        {

        }

        public void OnConnecting(ITcpClientBase client, ClientOperationEventArgs e)
        {

        }

        public void OnDisconnected(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"{nameof(MyPlug1)}检测到断线，原因：{e.Message}");
        }

        public void OnIDChanged(ITcpClientBase client, RRQMEventArgs e)
        {

        }

        public void OnReceivedData(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            string mes = e.ByteBlock.ToString();
            if (mes.StartsWith("1"))
            {
                e.AddOperation(Operation.Handled);
                Console.WriteLine($"从插件{nameof(MyPlug1)}收到数据:{e.ByteBlock}");
            }
        }

        public void OnSendingData(ITcpClientBase client, SendingEventArgs e)
        {

        }
    }

    /// <summary>
    /// 从泛型实现
    /// </summary>
    class MyPlug2 : TcpPluginBase
    {
        protected override void OnReceivedData(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            string mes = e.ByteBlock.ToString();
            if (mes.StartsWith("2"))
            {
                e.AddOperation(Operation.Handled);
                Console.WriteLine($"从插件{nameof(MyPlug2)}收到数据:{e.ByteBlock}");
            }
        }

        protected override void OnDisconnected(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"{nameof(MyPlug2)}检测到断线，原因：{e.Message}");
        }
    }

    public class MyService : TcpService<MySocketClient>
    {
        protected override void LoadConfig(RRQMConfig config)
        {
            //此处加载配置，用户可以从配置中获取配置项。
            base.LoadConfig(config);
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
