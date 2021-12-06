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
using RRQMSocket.FileTransfer;
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

namespace FileClientGUI.Win
{
    /// <summary>
    /// PullWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PullWindow : Window
    {
        public PullWindow()
        {
            InitializeComponent();
        }
        public bool SelectRequest(out FileRequest fileRequest)
        {
            this.ShowDialog();
            fileRequest = this.fileRequest;
            return this.go;
        }

        private void tb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.tb2.Text = System.IO.Path.GetFileName(this.tb1.Text);
        }

        FileRequest fileRequest;
        bool go;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fileRequest = new FileRequest()
            {
                FileCheckerType = FileCheckerType.MD5,
                Path = this.tb1.Text,
                SavePath = this.tb2.Text
            };
            this.go = true;
            this.Close();
        }
    }
}
