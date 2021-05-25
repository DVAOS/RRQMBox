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
using RRQMBox.Client.Common;
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.FileTransfer;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// FileClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FileClientWindow : RRQMWindow
    {
        public FileClientWindow()
        {
            InitializeComponent();
            this.Loaded += this.FileClientWindow_Loaded;
        }

        private void FileClientWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.fileList = new RRQMList<UrlFileInfo>();
            this.Lb_FileList.ItemsSource = this.fileList;
        }

        private FileClient fileClient;
        private RRQMList<UrlFileInfo> fileList;
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (fileClient != null)
            {
                ShowMsg("请勿重复连接");
                return;
            }

            try
            {
                fileClient = new FileClient();
                fileClient.Logger = new MsgLog(this.ShowMsg);
                fileClient.TransferFileError += FileClient_TransferFileError;
                fileClient.BeforeFileTransfer += this.FileClient_BeforeFileTransfer; ;
                fileClient.FinishedFileTransfer += this.FileClient_FinishedFileTransfer; ;
                fileClient.DisconnectedService += FileClient_DisConnectedService;
                fileClient.ReceiveSystemMes += this.FileClient_ReceiveSystemMes;
                fileClient.ConnectedService += this.FileClient_ConnectedService;
                fileClient.FileTransferCollectionChanged += this.FileClient_FileTransferCollectionChanged;
                fileClient.Connect(new IPHost("127.0.0.1:7789"), this.Tb_VerifyToken.Text);
            }
            catch (Exception ex)
            {
                if (fileClient != null && fileClient.Online)
                {
                    fileClient.Dispose();
                }

                fileClient = null;
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

        private void FileClient_FileTransferCollectionChanged(object sender, MesEventArgs e)
        {
            this.fileList = new RRQMList<UrlFileInfo>(this.fileClient.FileTransferCollection);
        }

        private void FileClient_DisConnectedService(object sender, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
            });
        }

        private void FileClient_ConnectedService(object sender, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6CE26C"));
            });
            ShowMsg("连接成功");
        }

        private void FileClient_FinishedFileTransfer(object sender, TransferFileMessageArgs e)
        {
            FileClient fileClient = sender as FileClient;//客户端中事件的sender实例均为FileClient
            RRQMSocket.FileTransfer.FileInfo fileInfo = e.FileInfo;//通过事件参数值，可获得完成的文件信息
            if (e.TransferType == TransferType.Download)
            {
                ShowMsg(string.Format("文件：{0}下载完成", e.FileInfo.FileName));
            }
            else
            {
                ShowMsg(string.Format("文件：{0}上传完成", e.FileInfo.FileName));
            }
        }

        private void FileClient_BeforeFileTransfer(object sender, FileOperationEventArgs e)
        {
            if (e.TransferType == TransferType.Download)
            {
                if (!Directory.Exists("ClientReceiveDir"))
                {
                    Directory.CreateDirectory("ClientReceiveDir");
                }

                DialogResult dialogResult = new DialogResult();
                dialogResult.Path = @"ClientReceiveDir\" + e.FileInfo.FileName;
                dialogResult.Visibility = System.Windows.Visibility.Visible;
                dialogResult.WaitHandle = new System.Threading.AutoResetEvent(false);

                this.DialogResult = dialogResult;
                dialogResult.WaitHandle.WaitOne();
                e.TargetPath = this.DialogResult.Path;
            }
        }

        private void FileClient_ReceiveSystemMes(object sender, MesEventArgs e)
        {
            ShowMsg(e.Message);
        }

        private void FileClient_TransferFileError(object sender, TransferFileMessageArgs e)
        {
            ShowMsg(e.Message);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (fileClient != null)
            {
                this.Speed = fileClient.TransferSpeed / (1024 * 1024.0);
                this.Progress = fileClient.TransferProgress;
            }
            else
            {
                this.Speed = 0;
                this.Progress = 0;
            }
        }

        #endregion 事件方法
    }
}
