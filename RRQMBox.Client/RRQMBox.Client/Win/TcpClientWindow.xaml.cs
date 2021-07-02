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
using RRQMBox.Client.Model;
using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// TcpClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TcpClientWindow : RRQMWindow
    {
        public TcpClientWindow()
        {
            InitializeComponent();
            this.Loaded += this.TcpClientWindow_Loaded;
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

        private void TcpClientWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cb_AdapterType.ItemsSource = Enum.GetValues(typeof(AdapterType));
        }

        private SimpleTcpClient tcpClient;
        private SimpleTokenClient tokenClient;

        private void TcpConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_IsToken.IsChecked == true)
            {
                CreateTokenTcpClient();
            }
            else
            {
                CreateTcpClient();
            }
        }

        private void CreateTcpClient()
        {
            if (tcpClient != null && tcpClient.Online)
            {
                ShowMsg("重复连接");
                return;
            }
            tcpClient = new SimpleTcpClient();
            tcpClient.ConnectedService += this.TcpClient_ConnectedService;//订阅连接成功事件
            tcpClient.DisconnectedService += this.TcpClient_DisconnectedService;//订阅断开连接事件
            tcpClient.Received += this.TcpClient_Received;
            try
            {
                var config = new TcpClientConfig();
                config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost(this.Tb_iPHost.Text))//远程IPHost
                    .SetValue(TcpClientConfig.SeparateThreadSendProperty, true)//独立线程发送
                    .SetValue(TcpClientConfig.DataHandlingAdapterProperty, GetAdapter(this.Cb_AdapterType));//数据处理适配器

                tcpClient.Setup(config);//载入配置
                tcpClient.Connect();//连接
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void CreateTokenTcpClient()
        {
            if (tokenClient != null && tokenClient.Online)
            {
                ShowMsg("重复连接");
                return;
            }
            tokenClient = new SimpleTokenClient();
            tokenClient.ConnectedService += this.TcpClient_ConnectedService;
            tokenClient.DisconnectedService += this.TcpClient_DisconnectedService;
            tokenClient.Received += this.TcpClient_Received;
            try
            {
                var config = new TcpClientConfig();
                config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost(this.Tb_iPHost.Text))
                    .SetValue(TokenClientConfig.VerifyTokenProperty, this.Tb_Token.Text)
                    .SetValue(TcpClientConfig.SeparateThreadSendProperty, true)
                    .SetValue(TcpClientConfig.DataHandlingAdapterProperty, GetAdapter(this.Cb_AdapterType));
                tokenClient.Setup(config);
                tokenClient.Connect();
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void TcpClient_Received(RRQMCore.ByteManager.ByteBlock arg2, object arg3)
        {
            string msg = Encoding.UTF8.GetString(arg2.Buffer, 0, (int)arg2.Length);
            ShowMsg($"{this.tcpClient.GetType().Name}收到消息：{msg}");
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
            if (this.Cb_IsToken.IsChecked == true)
            {
                if (tokenClient != null)
                {
                    tokenClient.Dispose();
                    tokenClient = null;
                }
            }
            else
            {
                if (tcpClient != null)
                {
                    tcpClient.Dispose();
                    tcpClient = null;
                }
            }
        }

        private void TcpSendButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_IsToken.IsChecked == true)
            {
                if (this.Cb_IsAsync.IsChecked == true)
                {
                    tokenClient.SendAsync(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
                else
                {
                    tokenClient.Send(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
            }
            else
            {
                if (this.Cb_IsAsync.IsChecked == true)
                {
                    tcpClient.SendAsync(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
                else
                {
                    tcpClient.Send(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
            }
        }

        private DataHandlingAdapter GetAdapter(ComboBox comboBox)
        {
            DataHandlingAdapter adapter;
            switch (comboBox.SelectedIndex)
            {
                default:
                case 0:
                    {
                        adapter = new NormalDataHandlingAdapter();
                        break;
                    }
                case 1:
                    {
                        adapter = new FixedHeaderDataHandlingAdapter();
                        break;
                    }
                case 2:
                    {
                        adapter = new FixedSizeDataHandlingAdapter(1024);
                        break;
                    }
                case 3:
                    {
                        adapter = new TerminatorDataHandlingAdapter(1024, "\r\n");
                        break;
                    }
                case 4:
                    {
                        adapter = new JsonStringDataHandlingAdapter();
                        break;
                    }
            }

            return adapter;
        }

        private void Cb_IsToken_Click(object sender, RoutedEventArgs e)
        {
            if (this.Cb_IsToken.IsChecked == true)
            {
                this.Tb_Token.Visibility = Visibility.Visible;
            }
            else
            {
                this.Tb_Token.Visibility = Visibility.Collapsed;
            }
        }

        private void SendBigButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.tcpClient != null)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            this.tcpClient.Send(new byte[1024]);
                        }
                        catch (Exception ex)
                        {
                            ShowMsg(ex.Message);
                        }
                    }
                });
            }
        }
    }
}