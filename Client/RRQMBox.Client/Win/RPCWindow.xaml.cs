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
using CookComputing.XmlRpc;
using RRQMBox.Client.Common;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// RPCWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RPCWindow : RRQMWindow
    {
        public RPCWindow()
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

        private void RRQMBinaryButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void RRQMUDPBinaryButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void XmlRpcButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void JsonRpcButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void IDInvokenButton_Click(object sender, RoutedEventArgs e)
        {
            IDRPCWindow window = new IDRPCWindow();
            window.Show();
        }

        TcpRpcClient client;
      
        private void Client_RPCInitialized1(object sender, MesEventArgs e)
        {
            ShowMsg("初始化完成");
        }

        private void ReGetServerProxyButton_Click(object sender, RoutedEventArgs e)
        {
            this.client.DiscoveryService(true);
        }

        private void GetProxyFileButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        
    }
}