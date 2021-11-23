using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Text;

namespace TokenClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择类型");
            Console.WriteLine("1.普通Token客户端");
            Console.WriteLine("2.简单Token客户端");

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
                default:
                    break;
            }
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
            };

            //声明配置
            var config = new TokenClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.DataHandlingAdapter = new NormalDataHandlingAdapter();//设置数据处理适配器
            config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送

            //继承TokenClient配置
            Console.WriteLine("请输入连接Token");
            config.VerifyToken = Console.ReadLine();//连接验证令箭
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //载入配置
            tokenClient.Setup(config);

            tokenClient.Connect();

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
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.DataHandlingAdapter = new NormalDataHandlingAdapter();//设置数据处理适配器
            config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送

            //继承TokenClient配置
            Console.WriteLine("请输入连接Token");
            config.VerifyToken = Console.ReadLine();//连接验证令箭
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //载入配置
            tokenClient.Setup(config);

            tokenClient.Connect();

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
