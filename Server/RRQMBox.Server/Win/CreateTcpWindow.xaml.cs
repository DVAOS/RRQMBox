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
using RRQMBox.Server.Common;
using RRQMBox.Server.Model;
using RRQMCore.ByteManager;
using RRQMSkin.MVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Text;
using System.Timers;
using System.Windows;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// CreatTcpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateTcpWindow : RRQMWindow
    {
        public CreateTcpWindow(CreateType createType)
        {
            InitializeComponent();

            this.Loaded += this.CreatTcpWindow_Loaded;
            this.createType = createType;
            if (createType == CreateType.TCP)
            {
                this.Tb_Token.Visibility = Visibility.Collapsed;
                this.Tb_iPHost.Text = "127.0.0.1:7790";
            }
            timer = new Timer(1000);
            timer.Elapsed += this.Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UIInvoke(() =>
            {
                this.Tb_TickShow.Text = $"已收到{count}条消息，共计{(size / (1024.0 * 1024)).ToString("0.000")}Mb";
                count = 0;
                size = 0;
            });
        }

        private Timer timer;

        private void CreatTcpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cb_AdapterType.ItemsSource = Enum.GetValues(typeof(AdapterType));
            this.onLineClient = new RRQMList<SocketClient>();
            this.Lb_OnlineClient.ItemsSource = this.onLineClient;
        }

        private CreateType createType;
        private RRQMList<SocketClient> onLineClient;
        private SimpleTcpService tcpService;
        private SimpleTokenService tokenService;

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            if (createType == CreateType.TCP)
            {
                CreateTcp();
            }
            else if (createType == CreateType.Token)
            {
                CreateTokenTcp();
            }
        }

        private void CreateTcp()
        {
            if (tcpService == null)
            {
                tcpService = new SimpleTcpService();
                //订阅事件
                tcpService.ClientConnected += Service_ClientConnected;//订阅连接事件
                tcpService.ClientDisconnected += Service_ClientDisconnected;//订阅断开连接事件
                tcpService.CreateSocketClient += Service_CreatSocketCliect;
                tcpService.Received += this.OnReceived;
            }

            this.adapterIndex = this.Cb_AdapterType.SelectedIndex;

            //注入配置
            var config = new TcpServiceConfig();
            config.MaxCount = 100000;
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(this.Tb_iPHost.Text) })
                .SetValue(TcpServiceConfig.ClearTypeProperty, ClearType.Receive|ClearType.Send)
                .SetValue(ServiceConfig.LoggerProperty, new MsgLog(this.ShowMsg))//设置内部日志记录器
                .SetValue(ServiceConfig.ThreadCountProperty, int.Parse(this.Tb_ThreadCount.Text))//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 1000*1000)//10秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(ServiceConfig.SeparateThreadReceiveProperty, false);

            //载入配置
            tcpService.Setup(config);

            //或通过实例注入配置，实例注入时须实例化对应配置，否则部分属性不可见
            //var config1 = new TcpServiceConfig();
            //config1.ListenIPHosts = new IPHost[] { new IPHost(this.Tb_iPHost.Text) };
            //config1.Logger = new MsgLog(this.ShowMsg);
            //config1.ThreadCount = int.Parse(this.Tb_ThreadCount.Text);
            //config1.ClearInterval = 300;
            //config1.BufferLength = 1024;

            //载入配置
            //tcpService.Setup(config1);

            //启动
            tcpService.Start();

            ShowMsg("绑定成功");
        }

        private void CreateTokenTcp()
        {
            if (tokenService == null)
            {
                tokenService = new SimpleTokenService();
                //订阅事件
                tokenService.ClientConnected += Service_ClientConnected;//订阅连接事件
                tokenService.ClientDisconnected += Service_ClientDisconnected;//订阅断开连接事件
                tokenService.CreateSocketClient += Service_CreatSocketCliect;
                tokenService.Received += this.OnReceived;
            }

            this.adapterIndex = this.Cb_AdapterType.SelectedIndex;

            //属性设置
            var config = new ServiceConfig();
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(this.Tb_iPHost.Text) })
                .SetValue(ServiceConfig.LoggerProperty, new MsgLog(this.ShowMsg))//设置内部日志记录器
                .SetValue(ServiceConfig.ThreadCountProperty, int.Parse(this.Tb_ThreadCount.Text))//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServiceConfig.VerifyTokenProperty, this.Tb_Token.Text)
                .SetValue(TcpServiceConfig.ClearTypeProperty, ClearType.Send | ClearType.Receive);


            //TcpServiceConfig config = new TcpServiceConfig();
            //config.ListenIPHosts = new IPHost[] { new IPHost(7789) };
            //config.ClearInterval = 30;
            //config.ClearType =  ClearType.Receive|ClearType.Send;
            //config.ServerName = "MyServer";

            //方法
            this.tokenService.Setup(config);
            this.tokenService.Start();
            this.ShowMsg("绑定成功");
            this.ShowMsg($"请使用Token为{tokenService.VerifyToken}进行连接");
        }

        private int adapterIndex;

        private void Service_CreatSocketCliect(SimpleSocketClient arg1, CreateOption arg2)
        {
            switch (this.adapterIndex)
            {
                default:
                case 0:
                    {
                        arg1.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
                        break;
                    }
                case 1:
                    {
                        arg1.SetDataHandlingAdapter(new FixedHeaderDataHandlingAdapter());
                        break;
                    }
                case 2:
                    {
                        arg1.SetDataHandlingAdapter(new FixedSizeDataHandlingAdapter(1024));
                        break;
                    }
                case 3:
                    {
                        arg1.SetDataHandlingAdapter(new TerminatorDataHandlingAdapter(1024, "\r\n"));
                        break;
                    }
                case 4:
                    {
                        arg1.SetDataHandlingAdapter(new JsonStringDataHandlingAdapter());
                        break;
                    }
            }
            if (!isPerformanceTest)
            {
                ShowMsg($"正在使用适配器=>{arg1.DataHandlingAdapter.GetType().Name}");
            }
        }

        private int count;
        private long size;

        private void OnReceived(SimpleSocketClient client, ByteBlock byteBlock, object obj)
        {
            //System.Threading.Thread.Sleep(10);
            if (isPerformanceTest)
            {
                count++;
                size += byteBlock.Length;
            }
            else
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                ShowMsg($"已接收到信息：{mes}");
            }
        }

        private void Service_ClientDisconnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Remove((SocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void Service_ClientConnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Add((SocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void ShowMsg(string msg)
        {
            this.UIInvoke(() =>
            {
                this.msgBox.AppendText($"{msg}\r\n");
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
            if (tcpService != null && tcpService.ServerState == ServerState.Running)
            {
                tcpService.Stop();
                ShowMsg("解除绑定");
                this.onLineClient.Clear();
            }
            else
            {
                ShowMsg("服务器未绑定");
            }
        }

        private void Bt_Dispose_Click(object sender, RoutedEventArgs e)
        {
            if (tcpService != null && tcpService.ServerState == ServerState.Running)
            {
                tcpService.Dispose();
                ShowMsg("释放绑定");
                this.onLineClient.Clear();
            }
            else
            {
                ShowMsg("服务器未绑定");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lb_OnlineClient.SelectedItem != null)
            {
                if (this.Cb_IsAsync.IsChecked == true)
                {
                    ((SocketClient)this.Lb_OnlineClient.SelectedItem).SendAsync(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
                else
                {
                    ((SocketClient)this.Lb_OnlineClient.SelectedItem).Send(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
            }
            else
            {
                ShowMsg("请先选择客户端列表");
            }
        }

        private bool isPerformanceTest=true;

        private void TestCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_PerformanceTest.IsChecked == true)
            {
                isPerformanceTest = true;
            }
            else
            {
                isPerformanceTest = false;
            }
        }

        private void CorrugatedButton_Click(object sender, RoutedEventArgs e)
        {
            this.msgBox.Clear();
        }

        private void Bt_DisposeConnected_Click(object sender, RoutedEventArgs e)
        {
            this.tcpService.Clear();

            //string[] ids = this.tcpService.SocketClients.GetIDs();
            //foreach (var item in ids)
            //{
            //    if (this.tcpService.TryGetSocketClient(item,out SimpleSocketClient socketClient))
            //    {
            //        socketClient.Dispose();
            //    }
            //}
        }
    }
}