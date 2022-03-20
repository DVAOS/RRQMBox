//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMBox.Server.Model;
using RRQMCore;
using RRQMCore.ByteManager;
using RRQMCore.Log;
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
        public CreateTcpWindow()
        {
            this.InitializeComponent();

            this.Loaded += this.CreatTcpWindow_Loaded;
            this.timer = new Timer(1000);
            this.timer.Elapsed += this.Timer_Elapsed;
            this.timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.Tb_TickShow.Text = $"已收到{this.count}条消息，共计{(this.size / (1024.0 * 1024)).ToString("0.000")}Mb";
                this.count = 0;
                this.size = 0;
            });
        }

        private Timer timer;

        private void CreatTcpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cb_AdapterType.ItemsSource = Enum.GetValues(typeof(AdapterType));
            this.onLineClient = new RRQMList<SocketClient>();
            this.Lb_OnlineClient.ItemsSource = this.onLineClient;
        }

        private RRQMList<SocketClient> onLineClient;
        private TcpService tcpService;

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            this.CreateTcp();
        }

        private void CreateTcp()
        {
            if (this.tcpService == null)
            {
                this.tcpService = new TcpService();
                this.tcpService.Container.RegisterTransient<ILog, ConsoleLogger>();

                //订阅事件
                this.tcpService.Connected += this.Service_ClientConnected;//订阅连接事件
                this.tcpService.Disconnected += this.Service_ClientDisconnected;//订阅断开连接事件
                this.tcpService.Connecting += this.Service_Connecting;
                this.tcpService.Received += this.TcpService_Received;

                //string[] ids = tcpService.SocketClients.GetIDs();//获取目前在线的所有ID
                //foreach (var id in ids)//遍历ID
                //{
                //    if (tcpService.TryGetSocketClient(id, out SimpleSocketClient socketClient))
                //    {
                //        socketClient.Send(new byte[] { 0 });//回发
                //    }
                //}
            }

            this.adapterIndex = this.Cb_AdapterType.SelectedIndex;

            //注入配置
            var config = new RRQMConfig();
            config.SetMaxCount(10000)
                .SetListenIPHosts(new IPHost[] { new IPHost(this.Tb_iPHost.Text) })
                .SetClearInterval(1000 * 20)
                .SetThreadCount(int.Parse(this.Tb_ThreadCount.Text))
                .SetClearType(ClearType.Receive | ClearType.Send)
                .SetBufferLength(1024);

            //载入配置
            this.tcpService.Setup(config);

            //启动
            this.tcpService.Start();

            this.ShowMsg("绑定成功");
        }

        private void TcpService_Received(SocketClient client, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (this.isPerformanceTest)
            {
                this.count++;
                this.size += byteBlock.Length;
            }
            else
            {
                string mes = byteBlock.ToString();
                this.ShowMsg($"已接收到信息：{mes}");
            }
        }

        private int adapterIndex;

        private void Service_Connecting(SocketClient arg1, ClientOperationEventArgs arg2)
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
                        arg1.SetDataHandlingAdapter(new FixedHeaderPackageAdapter());
                        break;
                    }
                case 2:
                    {
                        arg1.SetDataHandlingAdapter(new FixedSizePackageAdapter(1024));
                        break;
                    }
                case 3:
                    {
                        arg1.SetDataHandlingAdapter(new TerminatorPackageAdapter(1024, "\r\n"));
                        break;
                    }
                case 4:
                    {
                        //arg1.SetDataHandlingAdapter(new JsonStringDataHandlingAdapter());
                        break;
                    }
            }
            if (!this.isPerformanceTest)
            {
                this.ShowMsg($"正在使用适配器=>{arg1.DataHandlingAdapter.GetType().Name}");
            }
        }

        private int count;
        private long size;


        private void Service_ClientDisconnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Remove((SocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void Service_ClientConnected(object sender, RRQMEventArgs e)
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
            if (this.tcpService != null && this.tcpService.ServerState == ServerState.Running)
            {
                this.tcpService.Stop();
                this.ShowMsg("解除绑定");
                this.onLineClient.Clear();
            }
            else
            {
                this.ShowMsg("服务器未绑定");
            }
        }

        private void Bt_Dispose_Click(object sender, RoutedEventArgs e)
        {
            if (this.tcpService != null && this.tcpService.ServerState == ServerState.Running)
            {
                this.tcpService.Dispose();
                this.ShowMsg("释放绑定");
                this.onLineClient.Clear();
            }
            else
            {
                this.ShowMsg("服务器未绑定");
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
                this.ShowMsg("请先选择客户端列表");
            }
        }

        private bool isPerformanceTest = true;

        private void TestCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_PerformanceTest.IsChecked == true)
            {
                this.isPerformanceTest = true;
            }
            else
            {
                this.isPerformanceTest = false;
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