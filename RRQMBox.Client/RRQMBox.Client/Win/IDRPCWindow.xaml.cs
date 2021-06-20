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
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// IDRPCWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IDRPCWindow : RRQMWindow
    {
        public IDRPCWindow()
        {
            InitializeComponent();
            Closing += this.IDRPCWindow_Closing;
        }

        private void IDRPCWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Client != null)
            {
                Client.Dispose();
            }
        }

        private void ShowMsg(string msg)
        {
            this.UIInvoke(() =>
            {
                this.msgBox.AppendText($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}]:{msg}\r\n");
            });
        }
        private void UIInvoke(Action action)
        {
            this.Dispatcher.Invoke(() =>
            {
                action.Invoke();
            });
        }

        TcpRPCClient Client;
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Client == null)
            {
                this.Client = new TcpRPCClient();
            }

            var config = new TcpRPCClientConfig();
            config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost(this.Tb_iPHost.Text))
                  .SetValue(TokenClientConfig.VerifyTokenProperty, "123RPC");
            try
            {
                this.Client.Setup(config);
                this.Client.Connect();
                ShowMsg("连接成功");
                this.Client.RegistServer(new IDCallBackServer(ShowMsg));
                this.Client.OpenCallBackServer();
                this.TitleContent = Client.ID;
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }


        private void DicconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Client != null)
            {
                this.Client.Disconnect();
            }
        }

        private void DisposeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Client != null)
            {
                this.Client.Dispose();
                this.Client = null;
            }
        }

        private void IDInvokenButton_Click(object sender, RoutedEventArgs e)
        {
            string id = this.Tb_ID.Text;
            int methodToken = int.Parse(this.Tb_MethodToken.Text);
            Task.Run(() =>
            {
                if (this.Client != null)
                {
                    try
                    {
                        string s = this.Client.Invoke<string>(id, methodToken, InvokeOption.CanFeedback, 10);
                        ShowMsg(s);
                    }
                    catch (Exception ex)
                    {
                        ShowMsg(ex.Message);
                    }
                }
            });

        }

        private void TestStringButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                if (this.Client != null)
                {
                    try
                    {
                        string s = this.Client.Invoke<string>("TestStringReturnNullParameter", InvokeOption.CanFeedback, 10);
                        ShowMsg(s);
                    }
                    catch (Exception ex)
                    {
                        ShowMsg(ex.Message);
                    }
                }
            });
        }
    }

    /// <summary>
    /// 用于反向RPC
    /// </summary>
    public class IDCallBackServer : ServerProvider
    {
        public IDCallBackServer(Action<string> msgBox)
        {
            this.msgBox = msgBox;
        }
        private Action<string> msgBox;

        [RRQMRPCCallBackMethod(1000)]
        public string SayHello(int age)
        {
            string mes = $"Hello,我今年{age}岁了";
            msgBox.Invoke($"被调用了，输出：{mes}");
            return mes;
        }
    }
}
