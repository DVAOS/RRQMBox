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
using Microsoft.Win32;
using RRQMBox.Client.Common;
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.FileTransfer;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;

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
            Timer timer = new Timer(1000);
            timer.Elapsed += this.Timer_Tick;
            timer.Start();
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
            ConnectService();
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            this.BeginDownload();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            this.BeginUploadFile();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            this.DisconnectService();
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

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Pause();
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Resume();
        }

        private void StopThisButton_Click(object sender, RoutedEventArgs e)
        {
            this.StopThis();
        }

        private void StopAllButton_Click(object sender, RoutedEventArgs e)
        {
            this.StopAll();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lb_FileList.SelectedItem != null)
            {
                UrlFileInfo fileInfo = (UrlFileInfo)this.Lb_FileList.SelectedItem;
                this.Cancel(fileInfo);
            }
            else
            {
                ShowMsg("请先选中传输项");
            }
        }

        private void SelectUrlButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectPathFile();
        }

        #region 事件方法

        private void FileClient_FileTransferCollectionChanged(object sender, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.Lb_FileList.ItemsSource = new RRQMList<UrlFileInfo>(this.fileClient.FileTransferCollection);
            });
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

                UIInvoke(() =>
                {
                    this.SaveDialog.DialogResult = dialogResult;
                });

                dialogResult.WaitHandle.WaitOne();
                e.TargetPath = dialogResult.Path;
            }
        }

        private void FileClient_ReceiveSystemMes(object sender, MesEventArgs e)
        {
            ShowMsg(e.Message);
        }

        private void FileClient_TransferFileError(object sender, TransferFileMessageArgs e)
        {
            ShowMsg($"传输类型：{e.TransferType}，信息：{e.Message}");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UIInvoke(() =>
            {
                if (fileClient != null)
                {
                    this.Speed.Speed = fileClient.TransferSpeed / (1024 * 1024.0);
                    this.Progress.Value = fileClient.TransferProgress;
                }
                else
                {
                    this.Speed.Speed = 0;
                    this.Progress.Value = 0;
                }
            });
        }

        #endregion 事件方法

        #region 绑定方法

        private void BeginDownload()
        {
            if (fileClient != null && fileClient.Online)
            {
                if (string.IsNullOrEmpty(this.Tb_Url.Text))
                {
                    ShowMsg("Url不能为空");
                    return;
                }

                try
                {
                    UrlFileInfo fileInfo = UrlFileInfo.CreatDownload(this.Tb_Url.Text);
                    fileInfo.Restart = (bool)this.Cb_Restart.IsChecked;
                    fileClient.RequestTransfer(fileInfo);
                }
                catch (Exception e)
                {
                    ShowMsg(e.Message);
                }
            }
            else
            {
                ShowMsg("未连接");
            }
        }

        private void StopAll()
        {
            if (fileClient != null)
            {
                fileClient.StopAllTransfer();
                ShowMsg("已停止下载");
            }
        }

        private void Resume()
        {
            if (fileClient != null)
            {
                fileClient.ResumeTransfer();
            }
        }

        private void BeginUploadFile()
        {
            if (fileClient != null && fileClient.Online)
            {
                if (File.Exists(this.Tb_Url.Text))
                {
                    try
                    {
                        fileClient.RequestTransfer(UrlFileInfo.CreatUpload(this.Tb_Url.Text, fileClient.BreakpointResume, (bool)this.Cb_Restart.IsChecked));
                    }
                    catch (Exception e)
                    {
                        ShowMsg(e.Message);
                    }
                }
                else
                {
                    ShowMsg("文件不存在");
                }
            }
        }

        private void SelectPathFile()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null && fileDialog.FileName.Length > 0)
            {
                this.Tb_Url.Text = fileDialog.FileName;
            }
        }

        private void StopThis()
        {
            if (fileClient != null)
            {
                fileClient.StopThisTransfer();
                ShowMsg("停止当前成功");
            }
        }

        private void Cancel(UrlFileInfo urlFileInfo)
        {
            if (fileClient != null)
            {
                if (fileClient.CancelTransfer(urlFileInfo))
                {
                    ShowMsg($"成功取消文件：{urlFileInfo.FileName}");
                }
            }
        }

        private void Pause()
        {
            if (fileClient != null)
            {
                this.fileClient.PauseTransfer();
            }
        }

        private void ConnectService()
        {
            if (fileClient == null)
            {
                fileClient = new FileClient();
                fileClient.TransferFileError += this.FileClient_TransferFileError;
                fileClient.BeforeFileTransfer += this.FileClient_BeforeFileTransfer; ;
                fileClient.FinishedFileTransfer += this.FileClient_FinishedFileTransfer; ;
                fileClient.DisconnectedService += this.FileClient_DisConnectedService;
                fileClient.ConnectedService += this.FileClient_ConnectedService;
                fileClient.FileTransferCollectionChanged += this.FileClient_FileTransferCollectionChanged;
                fileClient.Received += this.FileClient_Received;
            }

            try
            {
                var config = new FileClientConfig();
                config.SetValue(FileClientConfig.LoggerProperty, new MsgLog(this.ShowMsg))
                    .SetValue(FileClientConfig.RemoteIPHostProperty, new IPHost(this.Tb_iPHost.Text))
                    .SetValue(FileClientConfig.VerifyTokenProperty, this.Tb_VerifyToken.Text)
                    .SetValue(FileClientConfig.ReceiveDirectoryProperty,"C://新建文件夹");
                fileClient.Setup(config);
                fileClient.Connect();
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

        private void FileClient_Received(object sender, short? procotol, RRQMCore.ByteManager.ByteBlock byteBlock)
        {
            ShowMsg($"收到：协议={procotol}，信息={Encoding.UTF8.GetString(byteBlock.Buffer, 2, (int)byteBlock.Length - 2)}");
        }

        private void DisconnectService()
        {
            if (fileClient != null && fileClient.Online)
            {
                fileClient.Dispose();
            }

            fileClient = null;
            this.Tb_Icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
        }

        #endregion 绑定方法

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.fileClient != null && this.fileClient.Online)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes("RRQM");
                    this.fileClient.Send(data, 0, data.Length);
                    this.fileClient.SendAsync(data, 0, data.Length);
                    ShowMsg($"发送成功");
                }
                catch (Exception ex)
                {
                    ShowMsg(ex.Message);
                }
            }
        }

        private void SendProtocolButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.fileClient != null && this.fileClient.Online)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes("RRQM");
                    this.fileClient.Send(10, data, 0, data.Length);
                    this.fileClient.SendAsync(10, data, 0, data.Length);
                    ShowMsg($"发送成功");
                }
                catch (Exception ex)
                {
                    ShowMsg(ex.Message);
                }
            }
        }

        private void TestInvokeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.fileClient != null)
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            ShowMsg(this.fileClient.Invoke<string>("SayHello", InvokeOption.WaitInvoke, i));
                        }
                        catch (Exception ex)
                        {
                            ShowMsg(ex.Message);
                        }
                    }
                });
            }
        }
    }

    public class FileArgs
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
    }
}