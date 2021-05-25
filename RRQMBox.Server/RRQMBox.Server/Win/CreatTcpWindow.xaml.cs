using RRQMBox.Server.Common;
using RRQMBox.Server.Model;
using RRQMCore.ByteManager;
using RRQMMVVM;
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
            this.Loaded += this.CreatTcpWindow_Loaded;
        }

        private void CreatTcpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cb_AdapterType.ItemsSource = Enum.GetValues(typeof(AdapterType));
            this.onLineClient = new RRQMList<TcpSocketClient>();
            this.Lb_OnlineClient.ItemsSource = this.onLineClient;
        }
        RRQMList<TcpSocketClient> onLineClient;
        TcpService<MyTcpSocketClient> service;
        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            if (service != null && service.IsBind)
            {
                ShowMsg("重复绑定");
                return;
            }
            service = new TcpService<MyTcpSocketClient>();

            this.adapterIndex = this.Cb_AdapterType.SelectedIndex;

            //订阅事件
            service.ClientConnected += Service_ClientConnected;//订阅连接事件
            service.ClientDisconnected += Service_ClientDisconnected;//订阅断开连接事件
            service.CreatSocketCliect += Service_CreatSocketCliect;//订阅创建辅助类事件，可直接设置其他属性。

            //属性设置
            service.IsCheckClientAlive = (bool)this.Cb_AutoCheck.IsChecked;//使用空包检验活性，不会对数据有任何影响。
            service.BufferLength = 1024;//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
            service.IDFormat = "Tcp-{0}";//设置分配ID的格式， 格式必须符合字符串格式，至少包含一个补位， 初始值为“{0}-TCP”
            service.Logger = new MsgLog(this.ShowMsg);//设置内部日志记录器
            service.MaxCount = int.Parse(this.Tb_maxCount.Text);//设置最大连接数，可动态设置，当已连接数超过设置数值时，将主动断开客户端。

            //方法
            service.Bind(new IPHost(this.Tb_iPHost.Text), 2);//绑定监听，可绑定Ipv6，可监听所有地址。

            ShowMsg("绑定成功");
        }

        private int adapterIndex;
        private void Service_CreatSocketCliect(MyTcpSocketClient arg1, CreatOption arg2)
        {
            //此处可进行初始化设置
            if (arg2.NewCreate)
            {
                switch (this.adapterIndex)
                {
                    default:
                    case 0:
                        {
                            arg1.DataHandlingAdapter = new NormalDataHandlingAdapter();
                            break;
                        }
                    case 1:
                        {
                            arg1.DataHandlingAdapter = new FixedHeaderDataHandlingAdapter();
                            break;
                        }
                    case 2:
                        {
                            arg1.DataHandlingAdapter = new FixedSizeDataHandlingAdapter(1024);
                            break;
                        }
                    case 3:
                        {
                            arg1.DataHandlingAdapter = new TerminatorDataHandlingAdapter(1024, "\r\n");
                            break;
                        }
                    case 4:
                        {
                            arg1.DataHandlingAdapter = new JsonStringDataHandlingAdapter();
                            break;
                        }
                }

                ShowMsg($"NewCreate适配器=>{arg1.DataHandlingAdapter.GetType().Name}");

                arg1.OnReceived = this.OnReceived;//赋值委托，触发接收
            }

        }

        private int count;
        private void OnReceived(MyTcpSocketClient client, ByteBlock byteBlock, object obj)
        {
            count++;
            if (count % 1 == 0)//用于频率输出
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                ShowMsg($"已接收到信息：{mes},第{count}条");
            }
        }

        private void Service_ClientDisconnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Remove((TcpSocketClient)sender);
            });
        }

        private void Service_ClientConnected(object sender, MesEventArgs e)
        {

            this.UIInvoke(() =>
            {
                this.onLineClient.Add((TcpSocketClient)sender);
            });
        }

        private void ShowMsg(string msg)
        {
            this.UIInvoke(() =>
            {
                this.msgBox.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]:{msg}\r\n");
            });
        }

        private void UIInvoke(Action action)
        {
            this.Dispatcher.Invoke(() =>
            {
                action.Invoke();
            });
        }

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (service != null && service.IsBind)
            {
                service.Dispose();
                ShowMsg("解除绑定");
                service = null;
                this.onLineClient.Clear();
            }
            else
            {
                ShowMsg("服务器未绑定");
            }
        }
    }

    public class MyTcpSocketClient : TcpSocketClient
    {
        public Action<MyTcpSocketClient, ByteBlock, object> OnReceived;
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            OnReceived.Invoke(this, byteBlock, obj);//此处通过委托将收到的数据抛出
        }
    }
}
