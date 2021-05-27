using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RRQMBox.Client.Model;
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;

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

        TcpClient tcpClient;
        TokenTcpClient tokenTcpClient;
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
            tcpClient = new TcpClient();
            tcpClient.DataHandlingAdapter = GetAdapter(this.Cb_AdapterType);
            tcpClient.ConnectedService += this.TcpClient_ConnectedService;
            tcpClient.DisconnectedService += this.TcpClient_DisconnectedService;
            tcpClient.OnReceived += this.TcpClient_OnReceived;
            try
            {
                tcpClient.Connect(new IPHost(this.Tb_iPHost.Text));
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void CreateTokenTcpClient()
        {
            if (tokenTcpClient != null && tokenTcpClient.Online)
            {
                ShowMsg("重复连接");
                return;
            }
            tokenTcpClient = new TokenTcpClient();
            tokenTcpClient.VerifyToken = this.Tb_Token.Text;
            tokenTcpClient.DataHandlingAdapter = GetAdapter(this.Cb_AdapterType);
            tokenTcpClient.ConnectedService += this.TcpClient_ConnectedService;
            tokenTcpClient.DisconnectedService += this.TcpClient_DisconnectedService;
            tokenTcpClient.OnReceived += this.TcpClient_OnReceived;
            try
            {
                tokenTcpClient.Connect(new IPHost(this.Tb_iPHost.Text));
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void TcpClient_OnReceived(TcpClient arg1, RRQMCore.ByteManager.ByteBlock arg2, object arg3)
        {
            string msg = Encoding.UTF8.GetString(arg2.Buffer,0,(int)arg2.Length);
            ShowMsg($"{arg1.GetType().Name}收到消息：{msg}");
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
                if (tokenTcpClient != null)
                {
                    tokenTcpClient.Dispose();
                    tokenTcpClient = null;
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
                    tokenTcpClient.SendAsync(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
                else
                {
                    tokenTcpClient.Send(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
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
    }

   
}

