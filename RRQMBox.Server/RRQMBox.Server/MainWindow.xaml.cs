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
using RRQMBox.Server.Win;
using RRQMSkin.Windows;
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
            CreatTcpWindow window = new CreatTcpWindow(false);
            window.Show();
            
        }

        private void CreatFileService_Click(object sender, RoutedEventArgs e)
        {
            FileServiceWindow window = new FileServiceWindow();
            window.Show();
            
        }

        private void CreatTokenTcpService_Click(object sender, RoutedEventArgs e)
        {
            CreatTcpWindow window = new CreatTcpWindow(true);
            window.Show();
           
        }

        private void CreatRPCService_Click(object sender, RoutedEventArgs e)
        {
            RPCServiceWindow window = new RPCServiceWindow();
            window.Show();
           
        }
    }
}
