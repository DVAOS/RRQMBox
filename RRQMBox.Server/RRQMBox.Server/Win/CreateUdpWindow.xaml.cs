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
            config.SetValue(UdpSessionConfig.DefaultRemotePointProperty, new IPHost(this.Tb_TargetIPHost.Text).EndPoint);//设置默认终结点
            if ((bool)this.Cb_UseBind.IsChecked)
            {
                config.SetValue(UdpSessionConfig.UseBindProperty, true)//是否执行绑定
                    .SetValue(UdpSessionConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(this.Tb_iPHost.Text) })//绑定的IP，udp只能绑定一个地址。
                    .SetValue(UdpSessionConfig.SeparateThreadReceiveProperty,false)
                    .SetValue(UdpSessionConfig.ThreadCountProperty,5);
            }

            this.udpSession.Setup(config);//加载配置
            this.udpSession.Start();//启动
            this.udpSession.Received += this.UdpSession_Received;
            ShowMsg("服务已启动");
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
                ShowMsg($"收到信息：{Encoding.UTF8.GetString(e.Buffer, 0, (int)e.Length)}");
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
                    this.udpSession.Send(Encoding.UTF8.GetBytes("RRQM"));
                }
                else
                {
                    this.udpSession.SendAsync(Encoding.UTF8.GetBytes("RRQM"));
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