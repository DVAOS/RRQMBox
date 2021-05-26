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
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// RPCServiceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RPCServiceWindow : RRQMWindow
    {
        public RPCServiceWindow()
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

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
