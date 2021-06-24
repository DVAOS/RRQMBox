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
using RRQMBox.Client.Win;
using RRQMSkin.Windows;
using System.Windows;

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
            this.Close();
        }

        private void TestTcpClientButton_Click(object sender, RoutedEventArgs e)
        {
            TcpClientWindow window = new TcpClientWindow();
            window.Show();
            this.Close();
        }

        private void TestRPCButton_Click(object sender, RoutedEventArgs e)
        {
            RPCWindow window = new RPCWindow();
            window.Show();
            this.Close();
        }

        private void StressTestButton_Click(object sender, RoutedEventArgs e)
        {
            StressTestingWindow window = new StressTestingWindow();
            window.Show();
            this.Close();
        }

        private void RPCStressTestButton_Click(object sender, RoutedEventArgs e)
        {
            RPCStressTestingWindow window = new RPCStressTestingWindow();
            window.Show();
            this.Close();
        }

        private void ProtocolButton_Click(object sender, RoutedEventArgs e)
        {
            ProtocolClientWindow window = new ProtocolClientWindow();
            window.Show();
            this.Close();
        }

        private void UDPButton_Click(object sender, RoutedEventArgs e)
        {
            UDPStressWindow window = new UDPStressWindow();
            window.Show();
            this.Close();
        }
    }
}