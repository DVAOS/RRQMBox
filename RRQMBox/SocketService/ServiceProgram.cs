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
using RRQMCore.ByteManager;
using RRQMSocket;

namespace Demo.TestTcpService
{
    class ServiceProgram
    {
        private static void Main(string[] args)
        {
            CreatTcpService();
            CreatTokenTcpService();
            Console.ReadKey();
        }

        /// <summary>
        /// 创建TcpService，
        /// 其辅助类为<see cref="MyTcpSocketClient"/>，
        /// 处理接收的数据在辅助类中实现。
        /// </summary>
        private static void CreatTcpService()
        {
            TcpService<MyTcpSocketClient> service = new TcpService<MyTcpSocketClient>();

            //订阅事件
            service.ClientConnected += Service_ClientConnected;//订阅连接事件
            service.ClientDisconnected += Service_ClientDisconnected;//订阅断开连接事件
            service.CreatSocketCliect += Service_CreatSocketCliect;//订阅创建辅助类事件，可直接设置其他属性。

            //属性设置
            service.IsCheckClientAlive = true;//使用空包检验活性，不会对数据有任何影响。
            service.BufferLength = 1024*64;//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
            service.IDFormat = "TcpSocketClient_{0}";//设置分配ID的格式， 格式必须符合字符串格式，至少包含一个补位， 初始值为“{0}-TCP”
            service.Logger = new Log();//设置内部日志记录器，默认日志是控制台输出。
            service.MaxCount = 10000;//设置最大连接数，可动态设置，当已连接数超过设置数值时，将主动断开客户端。

            //属性读取
            string ipAndPort = service.Name;//获取Ip及端口号
            string ip = service.IP;//获取IP
            int port = service.Port;//获取端口号
            MyTcpSocketClient socketClient = service.SocketClients["TcpSocketClient_1"];//通过ID获取辅助类，查找未知会抛出异常
            service.SocketClients.TryGetSocketClient("TcpSocketClient_1",out socketClient);//通过ID获取辅助类

            //方法
            service.Bind(7789, 2);//绑定监听，可绑定Ipv6，可监听所有地址。
            bool isExist = service.SocketClientExist("TcpSocketClient_1");//使用该方法判断ID对应的TcpSocketClient是否在线。

            Console.WriteLine("TcpService绑定成功");
        }

        /// <summary>
        /// 创建RRQMTcpService，
        /// 该类是对TcpService的简单继承封装，
        /// 辅助类为<see cref="RRQMSocketClient"/>，
        /// 可直接订阅OnReceived事件。
        /// </summary>
        private static void CreatRRQMTcpService()
        {
            RRQMTcpService service = new RRQMTcpService();
            service.CreatSocketCliect += Service_CreatSocketCliect1;//在初创辅助类时，可指定数据处理适配器。
            service.OnReceived += Service_OnReceived;//可直接订阅收到数据事件。

            //方法
            service.Bind(7790, 2);//绑定监听，可绑定Ipv6，可监听所有地址。

            Console.WriteLine("RRQMTcpService绑定成功");
        }

        /// <summary>
        /// 创建TokenTcpService
        /// </summary>
        private static void CreatTokenTcpService()
        {
            TokenTcpService<MyTcpSocketClient> service = new TokenTcpService<MyTcpSocketClient>();
            service.VerifyToken = "ABC";
            service.ClientConnected += Service_ClientConnected;//订阅连接事件
            service.ClientDisconnected += Service_ClientDisconnected;//订阅断开连接事件

            //方法
            service.Bind(7791, 2);//绑定监听，可绑定Ipv6，可监听所有地址。

            Console.WriteLine("TokenTcpService绑定成功");
        }

        private static void Service_CreatSocketCliect1(RRQMSocketClient arg1, CreatOption arg2)
        {
            if (arg2.NewCreate)
            {
                arg1.DataHandlingAdapter = new NormalDataHandlingAdapter();
            }
        }

        private static void Service_OnReceived(RRQMSocketClient arg1,ByteBlock arg2, object arg3)
        {
            Console.WriteLine($"收到来自于{arg1.Name}的数据");
        }

        private static void Service_CreatSocketCliect(MyTcpSocketClient arg1, CreatOption arg2)
        {
            //此处可进行初始化设置
            arg1.DataHandlingAdapter = new FixedHeaderDataHandlingAdapter();
        }

        private static void Service_ClientDisconnected(object sender, MesEventArgs e)
        {
            TcpSocketClient tcpSocketClient = (TcpSocketClient)sender;
            Console.WriteLine($"客户端断开连接,Name={tcpSocketClient.Name},ID={tcpSocketClient.ID}");
        }

        private static void Service_ClientConnected(object sender, MesEventArgs e)
        {
            TcpSocketClient tcpSocketClient = (TcpSocketClient)sender;
            Console.WriteLine($"客户端连接,Name={tcpSocketClient.Name},ID={tcpSocketClient.ID}");
        }

        
    }
}