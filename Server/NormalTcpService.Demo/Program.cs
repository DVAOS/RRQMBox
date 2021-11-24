using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Text;

namespace TcpServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择服务类型");
            Console.WriteLine("1.普通TCP服务器");
            Console.WriteLine("2.简单TCP服务器");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateNormalTcpService();
                        break;
                    }
                case "2":
                    {
                        CreateSimpleTcpService();
                        break;
                    }
                default:
                    break;
            }
        }
        private static void CreateSimpleTcpService()
        {
            SimpleTcpService service = new SimpleTcpService();

            service.Connected += (client, e) =>
            {
                client.Send(Encoding.UTF8.GetBytes("来了，老弟"));
                //有客户端连接
            };

            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };

            service.Connecting += (client, e) =>
            {
                //为初始化配置
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
            };
            

            service.Received += (client, byteBlock ,obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                Console.WriteLine($"已从{client.Name}接收到信息：{mes}");//Name即IP+Port

                client.Send(Encoding.UTF8.GetBytes($"已收到信息：{mes}"));
            };


            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.ServerName = "RRQMService";//服务名称
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.ThreadCount = 5;//多线程数量，当SeparateThreadReceive为false时，该值只决定BytePool的数量。
            config.Backlog = 30;
            config.ClearInterval = 60 * 1000;//60秒无数据交互会清理客户端
            config.ClearType = ClearType.Receive | ClearType.Send;//清理统计
            config.MaxCount = 10000;//最大连接数

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine("简单服务器启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }


        private static void CreateNormalTcpService()
        {
            MyTcpService service = new MyTcpService();

            service.Connected += (client, e) =>
            {
                //有客户端连接
            };

            service.Connecting += (client, e) =>
            {
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
                //client.Send(Encoding.UTF8.GetBytes("滚"));
                //e.IsPermitOperation = false;//是否允许连接
            };

            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.ServerName = "RRQMService";//服务名称
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.ThreadCount = 5;//多线程数量，当SeparateThreadReceive为false时，该值只决定BytePool的数量。
            config.Backlog = 30;
            config.ClearInterval = 60 * 1000;//60秒无数据交互会清理客户端
            config.ClearType = ClearType.Receive | ClearType.Send;//清理统计
            config.MaxCount = 10000;//最大连接数

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine("普通服务器启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

    }

    public class MyTcpService : TcpService<MySocketClient>
    {
        protected override void OnConnecting(MySocketClient socketClient, ClientOperationEventArgs e)
        {
            //socketClient.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//普通TCP报文处理器
            //或
            e.DataHandlingAdapter = new NormalDataHandlingAdapter();

            base.OnConnecting(socketClient, e);
        }
    }

    public class MySocketClient : SocketClient
    {
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
            Console.WriteLine($"已从{this.Name}接收到信息：{mes}");//Name即IP+Port

            this.Send(Encoding.UTF8.GetBytes($"已收到信息：{mes}"));
        }
    }

}
