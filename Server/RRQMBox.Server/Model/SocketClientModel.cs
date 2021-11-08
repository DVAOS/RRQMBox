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
using RRQMSkin.MVVM;
using RRQMSocket.FileTransfer;
using System;
using System.IO;
using System.Timers;

namespace RRQMBox.Server.Model
{
    public class SocketClientModel : ObservableObject, IDisposable
    {
        private FileSocketClient fileSocketClient;

        private string progress;

        private string speed;

        private Timer timer;

        public SocketClientModel(FileSocketClient socketClient)
        {
            this.fileSocketClient = socketClient;
        }

        public string ID { get => this.fileSocketClient.ID; }

        public string FileName
        {
            get
            {
                if (this.fileSocketClient.TransferFileInfo == null)
                {
                    return null;
                }
                return Path.GetFileName(this.fileSocketClient.TransferFileInfo.FilePath);
            }
        }

        public string Name
        {
            get { return fileSocketClient.Name; }
        }
        
        public string Progress
        {
            get { return progress; }
            set { progress = value; OnPropertyChanged(); }
        }

        public string Speed
        {
            get { return speed; }
            set { speed = value; OnPropertyChanged(); }
        }

        public TransferStatus TransferStatus
        {
            get { return this.fileSocketClient.TransferStatus; }
        }

        public int MaxUploadSpeed
        {
            get { return (int)(this.fileSocketClient.MaxUploadSpeed/ (1024 * 1024.0)); }
            set { this.fileSocketClient.MaxUploadSpeed = value*1024*1024L; OnPropertyChanged(); }
        }

        public int MaxDownloadSpeed
        {
            get { return (int)(this.fileSocketClient.MaxDownloadSpeed / (1024 * 1024.0)); }
            set 
            { 
                this.fileSocketClient.MaxDownloadSpeed = value * 1024 * 1024L;
                OnPropertyChanged(); 
            }
        }


        public void Begin()
        {
            timer = new Timer(1000);
            timer.Elapsed += this.Timer_Elapsed;
            timer.Start();
        }

        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            long speed = this.fileSocketClient.TransferSpeed;
            this.Speed = (speed / (1024 * 1024.0)).ToString("0.00") + "Mb/s";
            this.Progress = (this.fileSocketClient.TransferProgress * 100).ToString("0.00");
            this.OnPropertyChanged("Name");
            this.OnPropertyChanged("FileName");
            this.OnPropertyChanged("TransferStatus");
        }
    }
}