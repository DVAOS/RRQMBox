using System;
using System.Collections.Generic;
using System.IO;
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
using RRQMBox.Server.Common;
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.FileTransfer;

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
            if (fileService != null && fileService.IsBind)
            {
                ShowMsg("请勿重复绑定");
                return;
            }

            fileService = new FileService();
            try
            {
                fileService.Bind(new IPHost(this.Tb_iPHost.Text), int.Parse(this.Tb_ThreadCount.Text));
                fileService.Logger = new MsgLog(this.ShowMsg);
                fileService.VerifyToken = this.Tb_VerifyToken.Text;
                fileService.BreakpointResume = (bool)this.Cb_breakpointResume.IsChecked;
                fileService.ClientConnected += FileService_ClientConnected;
                fileService.ClientDisconnected += FileService_ClientDisconnected;
                fileService.BeforeFileTransfer += this.FileService_BeforeFileTransfer; ;
                fileService.FinishedFileTransfer += this.FileService_FinishedFileTransfer;
                fileService.ReceiveSystemMes += FileService_ReceiveSystemMes;
                fileService.RequestDeleteFile += this.FileService_RequestDeleteFile; ;
                fileService.RequestFileInfo += this.FileService_RequestFileInfo; ;
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6CE26C"));
                ShowMsg("启动成功");
            }
            catch (Exception ex)
            {
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
                ShowMsg(ex.Message);
            }
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


        #region 事件方法

        private void FileService_ReceiveSystemMes(object sender, MesEventArgs e)
        {
            ShowMsg(string.Format("收到{0}发来的消息：{1}\r\n", ((FileSocketClient)sender).Name, e.Message));
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
                ShowMsg(string.Format("{0}请求的文件：{1}已成功发送\r\n", ((FileSocketClient)sender).Name, e.FileInfo.FileName));
            }
            else
            {
                ShowMsg(string.Format("已收到{0}发来的文件：{1}\r\n", ((FileSocketClient)sender).Name, e.FileInfo.FileName));
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

                UIInvoke(()=> 
                {
                    e.IsPermitOperation = (bool)this.Cb_AllowUpload.IsChecked;//是否允许接收
                });
                
            }
            else
            {
                UIInvoke(() =>
                {
                    e.IsPermitOperation = (bool)this.Cb_AllowDownload.IsChecked;//是否允许下载
                });
               
            }
        }

        private void FileService_RequestFileInfo(object sender, FileOperationEventArgs e)
        {
            e.IsPermitOperation = (bool)this.Cb_GetFileInfo.IsChecked;
        }

        private void FileService_RequestDeleteFile(object sender, FileOperationEventArgs e)
        {
            e.IsPermitOperation = (bool)this.Cb_AllowDelete.IsChecked;
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
    }
}
