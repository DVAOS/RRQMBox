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
using RRQMCore.ByteManager;
using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Text;
using System.Windows;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// TcpClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProtocolClientWindow : RRQMWindow
    {
        public ProtocolClientWindow()
        {
            InitializeComponent();
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

        private SimpleProtocolClient client;

        private void TcpConnectButton_Click(object sender, RoutedEventArgs e)
        {
            CreateClient();
        }

        private void CreateClient()
        {
            if (client != null && client.Online)
            {
                ShowMsg("重复连接");
                return;
            }
            client = new SimpleProtocolClient();
            client.ConnectedService += this.TcpClient_ConnectedService;
            client.DisconnectedService += this.TcpClient_DisconnectedService;
            client.Received += this.Client_Received;
            try
            {
                var config = new TcpClientConfig();
                config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost(this.Tb_iPHost.Text))
                    .SetValue(TokenClientConfig.VerifyTokenProperty, this.Tb_Token.Text)
                    .SetValue(TcpClientConfig.SeparateThreadSendProperty, true);
                client.Setup(config);
                client.Connect();
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void Client_Received(short? arg1, ByteBlock byteBlock)
        {
            if (arg1 == null)
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                ShowMsg($"接收到无协议信息：ID={this.client.ID},信息：{mes}");
            }
            else
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 2, (int)byteBlock.Length - 2);
                ShowMsg($"接收到协议信息：ID={this.client.ID},协议={arg1},信息：{mes}");
            }
        }

        private void TcpClient_DisconnectedService(object sender, MesEventArgs e)
        {
            ShowMsg($"{sender.GetType().Name}已断开连接");
        }

        private void TcpClient_ConnectedService(object sender, MesEventArgs e)
        {
            ShowMsg($"{sender.GetType().Name}成功连接");
            ShowMsg($"正在使用{((TcpClient)sender).DataHandlingAdapter.GetType().Name}适配器");
        }

        private void TcpDicConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        private void TcpSendButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_IsAsync.IsChecked == true)
            {
                client.SendAsync(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
            }
            else
            {
                client.Send(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
            }
        }

        private void ResetIDButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.client != null)
            {
                this.client.ResetID("MyClientID");
                ShowMsg($"成功重置ID，当年ID={this.client.ID}");
            }
        }
    }
}