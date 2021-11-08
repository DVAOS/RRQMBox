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
using RRQMBox.Server.Model;
using RRQMBox.Server.Win;
using RRQMSkin.Windows;
using System.Windows;

namespace RRQMBox.Server
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

        private void CreatTcpService_Click(object sender, RoutedEventArgs e)
        {
            CreateTcpWindow window = new CreateTcpWindow(CreateType.TCP);
            window.Show();
            this.Close();
        }

        private void CreatFileService_Click(object sender, RoutedEventArgs e)
        {
            FileServiceWindow window = new FileServiceWindow();
            window.Show();
            this.Close();
        }

        private void CreatTokenTcpService_Click(object sender, RoutedEventArgs e)
        {
            CreateTcpWindow window = new CreateTcpWindow(CreateType.Token);
            window.Show();
            this.Close();
        }

        private void CreatRPCService_Click(object sender, RoutedEventArgs e)
        {
            RPCServiceWindow window = new RPCServiceWindow();
            window.Show();
            this.Close();
        }

        private void CreatProtocolService_Click(object sender, RoutedEventArgs e)
        {
            CreateProcotolWindow window = new CreateProcotolWindow();
            window.Show();
            this.Close();
        }

        private void CreatUdpService_Click(object sender, RoutedEventArgs e)
        {
            CreateUdpWindow window = new CreateUdpWindow();
            window.Show();
        }

        private void CreatXunitTestService_Click(object sender, RoutedEventArgs e)
        {
            XUnitWindow window = new XUnitWindow();
            window.Show();
            this.Close();
        }
    }
}