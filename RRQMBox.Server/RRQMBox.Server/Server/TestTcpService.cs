using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMBox.Server
{
    public static class TestTcpService
    {
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
            service.BufferLength = 1024;//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
            service.IDFormat = "Tcp-{0}";//设置分配ID的格式， 格式必须符合字符串格式，至少包含一个补位， 初始值为“{0}-TCP”
            service.Logger = new Log();//设置内部日志记录器，默认日志是控制台输出。
            service.MaxCount = 10000;//设置最大连接数，可动态设置，当已连接数超过设置数值时，将主动断开客户端。

            //属性读取
            string ipAndPort = service.Name;//获取Ip及端口号
            string ip = service.IP;//获取IP
            int port = service.Port;//获取端口号
           
            //方法
            service.Bind(7789, 2);//绑定监听，可绑定Ipv6，可监听所有地址。
           
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

        private static void Service_OnReceived(RRQMSocketClient arg1, ByteBlock arg2, object arg3)
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

    public class MyTcpSocketClient : TcpSocketClient
    {
        /// <summary>
        /// 初次创建对象，效应相当于构造函数，但是调用时机在构造函数之后，可覆盖父类方法。
        /// 适配器只能在此处赋值，在构造函数中赋值会被此处覆盖。
        /// </summary>
        public override void Create()
        {
            this.DataHandlingAdapter = new NormalDataHandlingAdapter();//普通TCP报文处理器
            //this.DataHandlingAdapter = new FixedHeaderDataHandlingAdapter();//固定包头TCP报文处理器
            //this.DataHandlingAdapter = new FixedSizeDataHandlingAdapter(1024);//固定长度TCP报文处理器
            //this.DataHandlingAdapter = new TerminatorDataHandlingAdapter(1024, "\r\n");//终止字符TCP报文处理器
            //this.DataHandlingAdapter = new MyTestDataHandingAdopter();//自定义处理器
        }

        private int count;

        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            count++;
            if (count % 1000 == 0)
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                Console.WriteLine($"已接收到信息：{mes},第{count}条");
            }
            if (this.Online)
            {
                this.Send(byteBlock);//回传消息
            }
        }
    }
}
