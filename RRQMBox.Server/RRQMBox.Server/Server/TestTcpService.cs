using RRQMBox.Server.Win;
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

   
}
