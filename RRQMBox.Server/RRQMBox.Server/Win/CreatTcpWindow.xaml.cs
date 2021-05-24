using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// CreatTcpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreatTcpWindow : Window
    {
        public CreatTcpWindow()
        {
            InitializeComponent();
        }

       
        private void Bt_Start_Click(object sender, RoutedEventArgs e)
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

        private void ShowMsg(string msg)
        {
            this.msgBox.AppendText(msg+"\r\n");
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
