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
using System.Text;

namespace TokenServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择服务类型");
            Console.WriteLine("1.多租户Token服务器");
            Console.WriteLine("2.简单Token服务器");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateNormalTokenService();
                        break;
                    }
                case "2":
                    {
                        CreateSimpleTokenService();
                        break;
                    }
                default:
                    break;
            }
        }
        private static void CreateSimpleTokenService()
        {
            SimpleTokenService service = new SimpleTokenService();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1,1000,(loop)=> 
            {
                Console.WriteLine($"客户端数量：{service.SocketClients.Count}");
            });

            loopAction.RunAsync();


            service.Connected += (client, e) =>
            {
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

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                Console.WriteLine($"已从{client.Name}接收到信息,并回应：{mes}");//Name即IP+Port

                client.Send(Encoding.UTF8.GetBytes($"响应信息：{mes}"));
            };


            //声明配置
            var config = new TokenServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.ServerName = "RRQMService";//服务名称
            config.ThreadCount = 5;//多线程数量，当SeparateThreadReceive为false时，该值只决定BytePool的数量。
            config.Backlog = 30;
            config.ClearInterval = 60 * 1000;//60秒无数据交互会清理客户端
            config.ClearType = ClearType.Receive | ClearType.Send;//清理统计
            config.MaxCount = 10000;//最大连接数
            config.ReceiveType = ReceiveType.IOCP;

            //继承TokenService配置
            config.VerifyToken = "Token";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine($"简单服务器启动成功,请使用'{service.VerifyToken}'连接");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }


        private static void CreateNormalTokenService()
        {
            MyTokenService service = new MyTokenService();

            service.Connected += (client, e) =>
            {
                //有客户端连接
            };

            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };

            //声明配置
            var config = new TokenServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.ServerName = "RRQMService";//服务名称
            config.ThreadCount = 5;//多线程数量，当SeparateThreadReceive为false时，该值只决定BytePool的数量。
            config.Backlog = 30;
            config.ClearInterval = 60 * 1000;//60秒无数据交互会清理客户端
            config.ClearType = ClearType.Receive | ClearType.Send;//清理统计
            config.MaxCount = 10000;//最大连接数

            //继承TokenService配置
            config.VerifyToken = "Token";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine($"普通服务器启动成功,请使用'{service.VerifyToken}'连接");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

    }

    public class MyTokenService : TokenService<MyTokenSocketClient>
    {
        protected override void OnConnecting(MyTokenSocketClient socketClient, ClientOperationEventArgs e)
        {
            socketClient.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//普通TCP报文处理器
            base.OnConnecting(socketClient, e);
        }

        protected override void OnVerifyToken(MyTokenSocketClient client, VerifyOption verifyOption)
        {
            if (verifyOption.Token == this.VerifyToken)
            {
                verifyOption.Accept = true;//如果是配置中的Token，直接允许连接
            }
            else if (verifyOption.Token.StartsWith("T"))//以T为标识示例，标识为租户
            {
                verifyOption.Accept = true;
                verifyOption.Flag = "租户";
            }
            else
            {
                verifyOption.Accept = false;
                verifyOption.ErrorMessage = "啥也不是";
            }
        }
    }

    public class MyTokenSocketClient : TokenSocketClient
    {
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
            Console.WriteLine($"已从{this.Name}接收到信息：{mes}");//Name即IP+Port

            this.Send(Encoding.UTF8.GetBytes($"已收到信息：{mes}"));
        }
    }
}
