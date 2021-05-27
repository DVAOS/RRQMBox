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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RRQMBox.Client.Win;
using RRQMSkin.Windows;

namespace RRQMBox.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : RRQMWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestFileTransferButton_Click(object sender, RoutedEventArgs e)
        {
            FileClientWindow window = new FileClientWindow();
            window.Show();
        }
        
        private void TestTcpClientButton_Click(object sender, RoutedEventArgs e)
        {
            TcpClientWindow window = new TcpClientWindow();
            window.Show();
        }

        private void TestRPCButton_Click(object sender, RoutedEventArgs e)
        {
            RPCWindow window = new RPCWindow();
            window.Show();
        }

        private void StressTestButton_Click(object sender, RoutedEventArgs e)
        {
            StressTestingWindow window = new StressTestingWindow();
            window.Show();
        }
    }
}
