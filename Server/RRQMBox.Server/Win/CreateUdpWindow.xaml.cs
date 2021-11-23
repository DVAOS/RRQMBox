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
using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Net;
using System.Text;
using System.Timers;
using System.Windows;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// CreateUdpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateUdpWindow : RRQMWindow
    {
        public CreateUdpWindow()
        {
            InitializeComponent();
            this.Loaded += CreateUdpWindow_Loaded;
        }

        private void CreateUdpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ShowMsg("请再启动一个窗口，并且设置监听和目标地址后进行测试。");
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

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            if (this.udpSession == null)
            {
                this.udpSession = new SimpleUdpSession();
            }
            var config = new UdpSessionConfig();//UDP配置
            
            config.ListenIPHosts = new IPHost[] { new IPHost(this.Tb_iPHost.Text) };//绑定的IP，udp只能绑定一个地址。
            
            //设置默认终结点,方便在调用Send时直接发送。一般作为服务器时不用设置。
            //config.DefaultRemotePoint = new IPHost(this.Tb_TargetIPHost.Text).EndPoint;
            
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.ServerName = "RRQMService";//服务名称
            config.UseBind = true;//启动监听
            config.ListenIPHosts = new IPHost[] { new IPHost(this.Tb_iPHost.Text) };
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.ThreadCount = 5;//多线程数量
           
            this.udpSession.Setup(config);//加载配置

            try
            {
                this.udpSession.Start();//启动
                this.udpSession.Received += this.UdpSession_Received;
                ShowMsg("服务已启动");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int count = 0;

        private void UdpSession_Received(EndPoint endpoint, RRQMCore.ByteManager.ByteBlock e)
        {
            if (isPerformanceTest)
            {
                count++;
            }
            else
            {
                ShowMsg($"从{endpoint.ToString()}收到信息：{Encoding.UTF8.GetString(e.Buffer, 0, (int)e.Length)}");
            }
        }

        private SimpleUdpSession udpSession;

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (udpSession != null)
            {
                udpSession.Stop();
                ShowMsg("解除绑定");
            }
        }

        private void Bt_Dispose_Click(object sender, RoutedEventArgs e)
        {
            if (udpSession != null)
            {
                udpSession.Dispose();
                udpSession = null;
                ShowMsg("释放绑定");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.udpSession != null)
            {
                if ((bool)this.Cb_IsAsync.IsChecked)
                {
                    this.udpSession.Send(new IPHost(this.Tb_TargetIPHost.Text).EndPoint,Encoding.UTF8.GetBytes("RRQM"));
                }
                else
                {
                    this.udpSession.SendAsync(new IPHost(this.Tb_TargetIPHost.Text).EndPoint,Encoding.UTF8.GetBytes("RRQM"));
                }
            }
        }

        private void CorrugatedButton_Click(object sender, RoutedEventArgs e)
        {
            this.msgBox.Clear();
        }

        private bool isPerformanceTest = true;
        private Timer timer;

        private void TestCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_PerformanceTest.IsChecked == true)
            {
                isPerformanceTest = true;
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
                timer = new Timer(1000);
                timer.Elapsed += this.Timer_Elapsed;
                timer.Start();
            }
            else
            {
                isPerformanceTest = false;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ShowMsg($"收到{count}条数据");
            count = 0;
        }
    }
}