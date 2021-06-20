using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
            config.SetValue(UdpSessionConfig.DefaultRemotePointProperty,new IPHost(this.Tb_TargetIPHost.Text).EndPoint);//设置默认终结点
            if ((bool)this.Cb_UseBind.IsChecked)
            {
                config.SetValue(UdpSessionConfig.UseBindProperty, true)//是否执行绑定
                    .SetValue(UdpSessionConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(this.Tb_iPHost.Text) });//绑定的IP，udp只能绑定一个地址。
            }

            this.udpSession.Setup(config);//加载配置
            this.udpSession.Start();//启动
            this.udpSession.Received += this.UdpSession_Received;
            ShowMsg("服务已启动");
        }

        int count = 0;
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

        SimpleUdpSession udpSession;
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
            if (this.udpSession!=null)
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
        bool isPerformanceTest=true;
        Timer timer;
        private void TestCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_PerformanceTest.IsChecked == true)
            {
                isPerformanceTest = true;
                if (timer!=null)
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
