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
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using Demo.ClientGUI.Models;
using Microsoft.Win32;
using RRQMMVVM;
using RRQMSocket;
using RRQMSocket.FileTransfer;

namespace Demo.ClientGUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += this.Timer_Tick;
            timer.Start();

            this.ConnectServiceCommand = new ExecuteCommand(ConnectService);
            this.DisConnectServiceCommand = new ExecuteCommand(DisConnectService);
            this.BeginDownloadCommand = new ExecuteCommand(BeginDownload);
            this.PauseCommand = new ExecuteCommand(Pause);
            this.StopThisCommand = new ExecuteCommand(StopThis);
            this.SelectPathCommand = new ExecuteCommand(SelectPathFile);
            this.BeginUploadFileCommand = new ExecuteCommand(BeginUploadFile);
            this.ResumeCommand = new ExecuteCommand(Resume);
            this.StopAllCommand = new ExecuteCommand(StopAll);
            this.SendSystemMessageCommand = new ExecuteCommand<string>(SendSystemMessage);
            this.RequestDeleteCommand = new ExecuteCommand(RequestDelete);
            this.RequestFileInfoCommand = new ExecuteCommand(RequestFileInfo);
            this.CancelCommand = new ExecuteCommand<UrlFileInfo>(Cancel);
        }

        #region 变量

        private FileClient fileClient;

        #endregion 变量

        #region Command

        public ExecuteCommand ConnectServiceCommand { get; set; }
        public ExecuteCommand DisConnectServiceCommand { get; set; }
        public ExecuteCommand BeginDownloadCommand { get; set; }
        public ExecuteCommand BeginUploadFileCommand { get; set; }
        public ExecuteCommand PauseCommand { get; set; }
        public ExecuteCommand StopThisCommand { get; set; }
        public ExecuteCommand SelectPathCommand { get; set; }
        public ExecuteCommand<UrlFileInfo> CancelCommand { get; set; }

        public ExecuteCommand ResumeCommand { get; set; }
        public ExecuteCommand StopAllCommand { get; set; }
        public ExecuteCommand<string> SendSystemMessageCommand { get; set; }
        public ExecuteCommand RequestDeleteCommand { get; set; }
        public ExecuteCommand RequestFileInfoCommand { get; set; }

        #endregion Command

        #region 属性

        private Brush clientIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));

        public Brush ClientIconForeground
        {
            get { return clientIconForeground; }
            set { clientIconForeground = value; OnPropertyChanged(); }
        }

        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; OnPropertyChanged(); }
        }

        private double speed;

        public double Speed
        {
            get { return speed; }
            set { speed = value; OnPropertyChanged(); }
        }

        private double progress;

        public double Progress
        {
            get { return progress; }
            set { progress = value; OnPropertyChanged(); }
        }

        private bool restart;

        public bool Restart
        {
            get { return restart; }
            set { restart = value; OnPropertyChanged(); }
        }

        private RRQMList<UrlFileInfo> filelist;

        public RRQMList<UrlFileInfo> FileList
        {
            get { return filelist; }
            set { filelist = value; OnPropertyChanged(); }
        }

        private DialogResult dialogResult;

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; OnPropertyChanged(); }
        }

        private string verifyToken;

        public string VerifyToken
        {
            get { return verifyToken; }
            set { verifyToken = value; OnPropertyChanged(); }
        }

        #endregion 属性

        #region 绑定方法

        private void SendSystemMessage(string mes)
        {
            if (fileClient != null && fileClient.Online)
            {
                fileClient.SendSystemMessage(mes);
            }
        }

        private void StopAll()
        {
            if (fileClient != null)
            {
                fileClient.StopAllTransfer();
                Log.Show("已停止下载");
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
                if (File.Exists(this.url))
                {
                    try
                    {
                        Task.Run(() =>
                        {
                            fileClient.RequestTransfer(UrlFileInfo.CreatUpload(this.url, this.restart, this.fileClient.BreakpointResume));
                            Log.Show("请求成功");
                        });
                    }
                    catch (Exception e)
                    {
                        Log.Show(e.Message);
                    }
                }
                else
                {
                    Log.Show("文件不存在");
                }
            }
        }

        private void SelectPathFile()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();

            if (fileDialog.FileName != null && fileDialog.FileName.Length > 0)
            {
                this.Url = fileDialog.FileName;
            }
        }

        private void StopThis()
        {
            if (fileClient != null)
            {
                fileClient.StopThisTransfer();
                Log.Show("停止当前成功");
            }
        }

        private void Cancel(UrlFileInfo urlFileInfo)
        {
            if (fileClient != null)
            {
                if (fileClient.CancelTransfer(urlFileInfo))
                {
                    Log.Show("操作成功");
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
            if (fileClient != null)
            {
                Log.Show("请勿重复连接");
                return;
            }

            try
            {
                fileClient = new FileClient();
                fileClient.TransferFileError += FileClient_TransferFileError;
                fileClient.BeforeFileTransfer += this.FileClient_BeforeFileTransfer; ;
                fileClient.FinishedFileTransfer += this.FileClient_FinishedFileTransfer; ;
                fileClient.DisconnectedService += FileClient_DisConnectedService;
                fileClient.ReceiveSystemMes += this.FileClient_ReceiveSystemMes;
                fileClient.ConnectedService += this.FileClient_ConnectedService;
                fileClient.FileTransferCollectionChanged += this.FileClient_FileTransferCollectionChanged;
                fileClient.Connect(new IPHost("127.0.0.1:7789"), this.verifyToken);
            }
            catch (Exception ex)
            {
                if (fileClient != null && fileClient.Online)
                {
                    fileClient.Dispose();
                }

                fileClient = null;
                this.ClientIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
                Log.Show(ex.Message);
            }
        }

        private void DisConnectService()
        {
            if (fileClient != null && fileClient.Online)
            {
                fileClient.Dispose();
            }

            fileClient = null;
            this.ClientIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
        }

        private void BeginDownload()
        {
            if (fileClient != null && fileClient.Online)
            {
                if (this.url == null || this.url.Length == 0)
                {
                    Log.Show("Url不能为空");
                    return;
                }
                Task.Run(() =>
                {
                    try
                    {
                        fileClient.RequestTransfer(UrlFileInfo.CreatDownload(this.Url)); ;
                    }
                    catch (Exception e)
                    {
                        Log.Show(e.Message);
                    }
                });
            }
            else
            {
                Log.Show("未连接");
            }
        }

        private void RequestDelete()
        {
            try
            {
                this.fileClient.RequestDelete(new UrlFileInfo(this.url));
                Log.Show("请求成功");
            }
            catch (Exception ex)
            {
                Log.Show(ex.Message);
            }
        }

        private void RequestFileInfo()
        {
            try
            {
                this.fileClient.RequestFileInfo(new UrlFileInfo(this.url));
                Log.Show("请求成功，请调试查看");
            }
            catch (Exception ex)
            {
                Log.Show(ex.Message);
            }
        }

        #endregion 绑定方法

        #region 事件方法

        private void FileClient_FileTransferCollectionChanged(object sender, MesEventArgs e)
        {
            this.FileList = new RRQMList<UrlFileInfo>(this.fileClient.FileTransferCollection);
        }

        private void FileClient_DisConnectedService(object sender, MesEventArgs e)
        {
            MainWindow.Window.Dispatcher.Invoke(() =>
            {
                this.ClientIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
            });
        }

        private void FileClient_ConnectedService(object sender, MesEventArgs e)
        {
            MainWindow.Window.Dispatcher.Invoke(() =>
            {
                this.ClientIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6CE26C"));
            });
            Log.Show("连接成功");
        }

        private void FileClient_FinishedFileTransfer(object sender, TransferFileMessageArgs e)
        {
            FileClient fileClient = sender as FileClient;//客户端中事件的sender实例均为FileClient
            RRQMSocket.FileTransfer.FileInfo fileInfo = e.FileInfo;//通过事件参数值，可获得完成的文件信息
            if (e.TransferType == TransferType.Download)
            {
                Log.Show(string.Format("文件：{0}下载完成", e.FileInfo.FileName));
            }
            else
            {
                Log.Show(string.Format("文件：{0}上传完成", e.FileInfo.FileName));
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
            Log.Show(e.Message);
        }

        private void FileClient_TransferFileError(object sender, TransferFileMessageArgs e)
        {
            Log.Show(e.Message);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.IsInDesignMode)
            {
                return;
            }

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