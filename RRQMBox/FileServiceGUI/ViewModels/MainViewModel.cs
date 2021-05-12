//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows.Media;
using RRQMMVVM;
using RRQMSocket;
using RRQMSocket.FileTransfer;

namespace Demo.ServiceGUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            this.ClientItems = new RRQMList<FileSocketClient>();
            this.CreatServiceCommand = new ExecuteCommand(CreatService);
            this.CloseServiceCommand = new ExecuteCommand(CloseService);
            this.SendMesCommand = new ExecuteCommand<string>(this.SendMes);
        }

        #region 变量

        private FileService fileService;

        #endregion 变量

        #region Command

        public ExecuteCommand CreatServiceCommand { get; set; }
        public ExecuteCommand CloseServiceCommand { get; set; }
        public ExecuteCommand<string> SendMesCommand { get; set; }

        #endregion Command

        #region 属性

        private Brush serviceIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));

        public Brush ServiceIconForeground
        {
            get { return serviceIconForeground; }
            set { serviceIconForeground = value; OnPropertyChanged(); }
        }

        private Brush clientIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));

        public Brush ClientIconForeground
        {
            get { return clientIconForeground; }
            set { clientIconForeground = value; OnPropertyChanged(); }
        }

        private RRQMList<FileSocketClient> clientItems;

        public RRQMList<FileSocketClient> ClientItems
        {
            get { return clientItems; }
            set { clientItems = value; OnPropertyChanged(); }
        }

        private bool allowDownload = true;

        public bool AllowDownload
        {
            get { return allowDownload; }
            set { allowDownload = value; OnPropertyChanged(); }
        }

        private bool allowDelete = false;

        public bool AllowDelete
        {
            get { return allowDelete; }
            set { allowDelete = value; OnPropertyChanged(); }
        }

        private bool getFileInfo = false;

        public bool GetFileInfo
        {
            get { return getFileInfo; }
            set { getFileInfo = value; OnPropertyChanged(); }
        }

        private bool allowUpload = true;

        public bool AllowUpload
        {
            get { return allowUpload; }
            set { allowUpload = value; OnPropertyChanged(); }
        }

        private FileSocketClient selectedClient;

        public FileSocketClient SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged();
            }
        }

        private bool breakpointResume;

        public bool BreakpointResume
        {
            get { return breakpointResume; }
            set
            {
                breakpointResume = value;
            }
        }

        private string connectionToken;

        public string ConnectionToken
        {
            get { return connectionToken; }
            set { connectionToken = value; OnPropertyChanged(); }
        }

        #endregion 属性

        #region 绑定方法

        private void CreatService()
        {
            if (fileService != null && fileService.IsBind)
            {
                Log.Show("请勿重复绑定");
                return;
            }

            fileService = new FileService();
            try
            {
                fileService.Bind(7789, 1);
                fileService.VerifyToken = this.connectionToken;
                fileService.BreakpointResume = this.breakpointResume;
                fileService.ClientConnected += FileService_ClientConnected;
                fileService.ClientDisconnected += FileService_ClientDisconnected;

                fileService.BeforeFileTransfer += this.FileService_BeforeFileTransfer; ;
                fileService.FinishedFileTransfer += this.FileService_FinishedFileTransfer;
                fileService.ReceiveSystemMes += FileService_ReceiveSystemMes;
                fileService.RequestDeleteFile += this.FileService_RequestDeleteFile; ;
                fileService.RequestFileInfo += this.FileService_RequestFileInfo; ;
                this.ServiceIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6CE26C"));
                Log.Show("启动成功");
            }
            catch (Exception ex)
            {
                this.ServiceIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
                Log.Show(ex.Message);
            }
        }

        private void FileService_RequestFileInfo(object sender, FileOperationEventArgs e)
        {
            e.IsPermitOperation = this.getFileInfo;
        }

        private void FileService_RequestDeleteFile(object sender, FileOperationEventArgs e)
        {
            e.IsPermitOperation = this.allowDelete;
        }

        private void CloseService()
        {
            this.ClientItems.Clear();
            if (fileService != null)
            {
                fileService.Dispose();
                fileService = null;
            }
            this.ServiceIconForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8E8EC"));
        }

        private void SendMes(string mes)
        {
            if (this.selectedClient != null)
            {
                this.selectedClient.SendSystemMes(mes);
            }
        }

        #endregion 绑定方法

        #region 事件方法

        private void FileService_ReceiveSystemMes(object sender, MesEventArgs e)
        {
            Log.Show(string.Format("收到{0}发来的消息：{1}\r\n", ((FileSocketClient)sender).Name, e.Message));
        }

        private void FileService_ClientConnected(object sender, MesEventArgs e)
        {
            FileSocketClient client = (FileSocketClient)sender;

            UIInvoke(() =>
            {
                this.ClientItems.Add(client);
            });
        }

        private void FileService_ClientDisconnected(object sender, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.ClientItems.Remove((FileSocketClient)sender);
            });
        }

        private void FileService_FinishedFileTransfer(object sender, TransferFileMessageArgs e)
        {
            if (e.TransferType == TransferType.Download)
            {
                Log.Show(string.Format("{0}请求的文件：{1}已成功发送\r\n", ((FileSocketClient)sender).Name, e.FileInfo.FileName));
            }
            else
            {
                Log.Show(string.Format("已收到{0}发来的文件：{1}\r\n", ((FileSocketClient)sender).Name, e.FileInfo.FileName));
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
                e.IsPermitOperation = this.AllowUpload;//是否允许接收
            }
            else
            {
                e.IsPermitOperation = this.AllowDownload;//是否允许下载
            }
        }

        #endregion 事件方法
    }
}