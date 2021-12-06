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
using FileServiceGUI.Models;
using FileServiceGUI.Win;
using Microsoft.Win32;
using RRQMCore.Run;
using RRQMSkin.MVVM;
using RRQMSocket;
using RRQMSocket.FileTransfer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileServiceGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += this.MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.clients = new RRQMList<FileSocketClient>();
            this.remoteModels = new RRQMList<TransferModel>();
            this.localModels = new RRQMList<TransferModel>();

            this.ListBox_Clients.ItemsSource = this.clients;
            this.ListBox_RemoteTransfer.ItemsSource = this.remoteModels;
            this.ListBox_LocalTransfer.ItemsSource = this.localModels;

            this.Start();
        }

        private RRQMList<FileSocketClient> clients;
        private RRQMList<TransferModel> remoteModels;
        private RRQMList<TransferModel> localModels;

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

        FileService fileService;
        private void Start()
        {
            //启动
            if (fileService != null)
            {
                return;
            }
            fileService = new FileService();
            fileService.Connected += this.FileService_Connected;
            fileService.Disconnected += this.FileService_Disconnected;
            fileService.BeforeFileTransfer += this.FileService_BeforeFileTransfer;
            var config = new FileServiceConfig()
            {
                ListenIPHosts = new IPHost[] { new IPHost(7789) },
                VerifyToken = "FileService",
                ClearInterval=300*1000
            };

            try
            {
                fileService.Setup(config);
                fileService.Start();
                ShowMsg("启动成功");
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void FileService_BeforeFileTransfer(FileSocketClient client, FileOperationEventArgs e)
        {
            TransferModel model = new TransferModel();
            model.FileOperator = e.FileOperator;
            model.TransferType = e.TransferType;

            switch (e.TransferType)
            {
                case TransferType.Push:
                    model.FilePath = e.FileRequest.SavePath;
                    model.FileLength = FileUtility.ToFileLengthString(e.FileInfo.FileLength);
                    break;
                case TransferType.Pull:
                    model.FilePath = e.FileRequest.Path;
                    model.FileLength = FileUtility.ToFileLengthString(new FileInfo(e.FileRequest.Path).Length);
                    break;
                default:
                    break;
            }
            model.Start();

            UIInvoke(() =>
            {
                this.remoteModels.Add(model);
            });
        }

        private void FileService_Disconnected(FileSocketClient client, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.clients.Remove(client);
            });
        }

        private void FileService_Connected(FileSocketClient client, MesEventArgs e)
        {
            UIInvoke(() =>
            {
                this.clients.Add(client);
            });
        }

        TransferModel transferModel;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).SelectedItem is TransferModel transferModel)
            {
                this.transferModel = transferModel;
            }
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (transferModel == null || transferModel.FileOperator.Result.ResultCode != RRQMCore.ResultCode.Default)
            {
                MessageBox.Show("请选择一个条目，然后控制。");
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(((TextBox)sender).Text))
                {
                    return;
                }
                this.transferModel.FileOperator.SetMaxSpeed(int.Parse(((TextBox)sender).Text));
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //取消
            if (transferModel == null || transferModel.FileOperator.Result.ResultCode != RRQMCore.ResultCode.Default)
            {
                MessageBox.Show("请选择一个条目，然后控制。");
                return;
            }
            try
            {
                this.transferModel.FileOperator.SetCancellationTokenSource(new System.Threading.CancellationTokenSource());
                this.transferModel.FileOperator.Cancel();
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //Push
            if (this.ListBox_Clients.SelectedItem is FileSocketClient client)
            {
                PushWindow pushWindow = new PushWindow();
                if (pushWindow.SelectRequest(out FileRequest fileRequest))
                {
                    if (this.cb_resume.IsChecked == true)
                    {
                        fileRequest.Flags = TransferFlags.BreakpointResume;
                    }
                    fileRequest.Overwrite = (bool)this.cb_overwrite.IsChecked;
                    FileOperator fileOperator = new FileOperator();

                    FileInfo fileInfo = new FileInfo(fileRequest.Path);

                    TransferModel model = new TransferModel()
                    {
                        FileLength = FileUtility.ToFileLengthString(fileInfo.Length),
                        FilePath = fileRequest.Path,
                        FileOperator = fileOperator,
                        TransferType = TransferType.Push
                    };
                    model.Start();

                    this.localModels.Add(model);
                    client.PushFileAsync(fileRequest, fileOperator);
                }
            }
            else
            {
                MessageBox.Show("请选择一个客户端");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Pull
            if (this.ListBox_Clients.SelectedItem is FileSocketClient client)
            {
                PullWindow pullWindow = new PullWindow();
                if (pullWindow.SelectRequest(out FileRequest fileRequest))
                {
                    if (this.cb_resume.IsChecked == true)
                    {
                        fileRequest.Flags = TransferFlags.BreakpointResume;
                    }
                    fileRequest.Overwrite = (bool)this.cb_overwrite.IsChecked;
                    FileOperator fileOperator = new FileOperator();

                    TransferModel model = new TransferModel()
                    {
                        FileLength = "未知",
                        FilePath = fileRequest.Path,
                        FileOperator = fileOperator,
                        TransferType = TransferType.Pull
                    };
                    model.Start();

                    this.localModels.Add(model);
                    client.PullFileAsync(fileRequest, fileOperator);
                }
            }
            else
            {
                MessageBox.Show("请选择一个客户端");
            }
        }
    }
}
