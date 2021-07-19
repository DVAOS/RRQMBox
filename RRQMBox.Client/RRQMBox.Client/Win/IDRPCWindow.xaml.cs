//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
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
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Threading.Tasks;
using System.Windows;

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

        private TcpRPCClient Client;

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
                this.Client.RegisterServer(new IDCallBackServer(ShowMsg));
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
                        string s = this.Client.Invoke<string>(id, methodToken, InvokeOption.WaitInvoke, 10);
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
                        string s = this.Client.Invoke<string>("TestStringReturnNullParameter", InvokeOption.WaitInvoke, 10);
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