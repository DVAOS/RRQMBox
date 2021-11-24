using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Text;

namespace TcpClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择服务类型");
            Console.WriteLine("1.普通TCP客户端");
            Console.WriteLine("2.简单TCP客户端");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateNormalTcpClient();
                        break;
                    }
                case "2":
                    {
                        CreateSimpleTcpClient();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void CreateSimpleTcpClient()
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
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.DataHandlingAdapter = new NormalDataHandlingAdapter();//设置数据处理适配器
            config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送

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
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
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
