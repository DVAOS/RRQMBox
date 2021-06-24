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
using RRQMBox.Server.Common;
using RRQMCore.ByteManager;
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.FileTransfer;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// FileServiceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FileServiceWindow : RRQMWindow
    {
        public FileServiceWindow()
        {
            InitializeComponent();
            this.Loaded += this.FileServiceWindow_Loaded;
        }

        private void FileServiceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            clientItems = new RRQMList<FileSocketClient>();
            this.Lb_Client.ItemsSource = clientItems;
        }

        private FileService fileService;
        private RRQMList<FileSocketClient> clientItems;

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (fileService == null)
            {
                fileService = new FileService();

                fileService.ClientConnected += FileService_ClientConnected;
                fileService.ClientDisconnected += FileService_ClientDisconnected;
                fileService.BeforeFileTransfer += this.FileService_BeforeFileTransfer;
                fileService.FinishedFileTransfer += this.FileService_FinishedFileTransfer;
                fileService.Received += this.FileService_Received;

                RPCService rPCService = new RPCService();
                rPCService.RegistServer<MyOperation>();
                rPCService.AddRPCParser("fileService", fileService);
                rPCService.OpenServer();
            }

            try
            {
                var config = new FileServiceConfig();
                config.SetValue(ServerConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(this.Tb_iPHost.Text) })
                    .SetValue(ServerConfig.ThreadCountProperty, int.Parse(this.Tb_ThreadCount.Text))
                    .SetValue(RRQMConfig.LoggerProperty, new MsgLog(this.ShowMsg))
                    .SetValue(TokenServerConfig.VerifyTokenProperty, this.Tb_VerifyToken.Text)
                    .SetValue(FileServiceConfig.BreakpointResumeProperty, (bool)this.Cb_breakpointResume.IsChecked)
                    .SetValue(FileServiceConfig.MaxDownloadSpeedProperty, 1024 * 1024 * 10L)
                    .SetValue(FileServiceConfig.MaxUploadSpeedProperty, 1024 * 1024 * 10L);

                this.fileService.Setup(config);
                this.fileService.Start();
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6CE26C"));
                ShowMsg("启动成功");
            }
            catch (Exception ex)
            {
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
                ShowMsg(ex.Message);
            }
        }

        private void FileService_Received(object sender, short? procotol, ByteBlock byteBlock)
        {
            string msg = Encoding.UTF8.GetString(byteBlock.Buffer, 2, (int)byteBlock.Length - 2);
            ShowMsg($"收到：协议=“{procotol}”，信息：{msg}");

            byte[] data = Encoding.UTF8.GetBytes(msg);
            ((FileSocketClient)sender).Send(data);
            ((FileSocketClient)sender).SendAsync(data);
            ShowMsg($"已普通回复");

            ((FileSocketClient)sender).Send(10, data, 0, data.Length);
            ((FileSocketClient)sender).SendAsync(10, data, 0, data.Length);
            ShowMsg($"已通过协议回复");
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

        #region 事件方法

        private void FileService_ReceiveSystemMes(object sender, MesEventArgs e)
        {
            ShowMsg(string.Format("收到发来的消息：{0}", e.Message));
        }

        private void FileService_ClientConnected(object sender, MesEventArgs e)
        {
            FileSocketClient client = (FileSocketClient)sender;

            UIInvoke(() =>
            {
                this.clientItems.Add(client);
            });
        }

        private void FileService_ClientDisconnected(object sender, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.clientItems.Remove((FileSocketClient)sender);
            });
        }

        private void FileService_FinishedFileTransfer(object sender, TransferFileMessageArgs e)
        {
            if (e.TransferType == TransferType.Download)
            {
                ShowMsg(string.Format("请求的文件：{0}已成功发送\r\n", e.FileInfo.FileName));
            }
            else
            {
                ShowMsg(string.Format("已收到发来的文件：{0}\r\n", e.FileInfo.FileName));
            }
        }

        private void FileService_BeforeFileTransfer(object sender, FileOperationEventArgs e)
        {
            if (e.TransferType == TransferType.Upload)
            {
                if (!Directory.Exists("ServiceReceiveDir"))
                {
                    Directory.CreateDirectory("ServiceReceiveDir");
                }
                e.TargetPath = @"ServiceReceiveDir\" + e.FileInfo.FileName;

                e.IsPermitOperation = this.AllowUp;//是否允许接收
            }
            else
            {
                e.IsPermitOperation = this.AllowDown;//是否允许下载
            }
        }

        #endregion 事件方法

        private void CloseServiceButton_Click(object sender, RoutedEventArgs e)
        {
            this.clientItems.Clear();
            if (fileService != null)
            {
                fileService.Dispose();
                fileService = null;
            }
            this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
            ShowMsg("服务已关闭");
        }

        private void SendMesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lb_Client.SelectedItem is FileSocketClient client)
            {
            }
        }

        public bool AllowDown { get; set; } = true;
        public bool AllowUp { get; set; } = true;

        private void Cb_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            switch (checkBox.Content.ToString())
            {
                case "允许下载":
                    {
                        AllowDown = (bool)checkBox.IsChecked;
                        break;
                    }
                case "允许上传":
                    {
                        AllowUp = (bool)checkBox.IsChecked;
                        break;
                    }
                default:
                    break;
            }
        }
    }

    public class MyOperation : ServerProvider
    {
        [RRQMRPC]
        public string SayHello(int a)
        {
            return a.ToString();
        }

        [RRQMRPC]
        public int Sum(int a,int b)
        {
            return a+b;
        }
    }
}