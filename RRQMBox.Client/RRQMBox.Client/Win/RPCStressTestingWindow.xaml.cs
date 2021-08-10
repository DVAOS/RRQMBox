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
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// RPCStressTestingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RPCStressTestingWindow : RRQMWindow
    {
        public RPCStressTestingWindow()
        {
            InitializeComponent();
        }

        public RRQMList<RPCTestObject> TestObjects { get; set; }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isTest)
            {
                MessageBox.Show("测试已经在进行");
                return;
            }
            isTest = true;
            int clientCount = int.Parse(this.ClientCount.Text);

            TestObjects = new RRQMList<RPCTestObject>();
            this.DG.ItemsSource = TestObjects;

            Task.Run(() =>
            {
                for (int i = 0; i < clientCount; i++)
                {
                    if (!isTest)
                    {
                        break;
                    }
                    RPCTestObject testObject = new RPCTestObject();
                    testObject.Client = new TcpRPCClient();
                    testObject.Num = i;
                    var config = new TcpRPCClientConfig();
                    config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7700"))
                          .SetValue(TokenClientConfig.VerifyTokenProperty, "123RPC")
                          .SetValue(TcpRPCClientConfig.ProxyTokenProperty, "RPC");
                    try
                    {
                        testObject.Client.Setup(config);
                        testObject.Client.DiscoveryService();
                        testObject.Status = "连接成功";
                    }
                    catch
                    {
                        testObject.Status = "连接失败";
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        TestObjects.Add(testObject);
                    });
                }
                Thread.Sleep(3000);
                GroupSend();
            });

            Task.Run(async () =>
            {
                while (isTest)
                {
                    int len = this.TestObjects.Count;
                    for (int i = 0; i < len; i++)
                    {
                        this.TestObjects[i].ShowInfo();
                    }
                    await Task.Delay(1000);
                }
            });
        }

        private void GroupSend()
        {
            int size = Math.Min(this.TestObjects.Count, 1);//每个线程托管的客户端
            int threadCount;
            if (this.TestObjects.Count % size == 0)
            {
                threadCount = this.TestObjects.Count / size;
            }
            else
            {
                threadCount = this.TestObjects.Count / size + 1;
            }

            ThreadPool.SetMinThreads(threadCount, threadCount);
            RPCTestObject[] allObjects = this.TestObjects.ToArray();

            for (int i = 0; i < threadCount; i++)
            {
                RPCTestObject[] testObjects = new RPCTestObject[size];
                Array.Copy(allObjects, i * size, testObjects, 0, Math.Min(this.TestObjects.Count - i * size, size));
                Task.Run(() =>
                {
                    Run(testObjects);
                });
            }
        }

        private void Run(RPCTestObject[] testObjects)
        {
            while (isTest)
            {
                foreach (var item in testObjects)
                {
                    item.Send();
                }
            }
        }

        private bool isTest;

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            isTest = false;
            foreach (var item in this.TestObjects)
            {
                item.Client.Dispose();
            }
            this.TestObjects.Clear();
        }

        private void Cb_IsAsync_Click(object sender, RoutedEventArgs e)
        {
            //TestObject.IsAsync = (bool)this.Cb_IsAsync.IsChecked;
        }
    }

    public class RPCTestObject : ObservableObject
    {
        public byte[] Data { get; set; }
        public static bool IsAsync { get; set; }
        public int Num { get; set; }

        public TcpRPCClient Client { get; set; }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        private int send;

        private int sendCount;

        public int SendCount
        {
            get { return sendCount; }
            set
            {
                sendCount = value;
                OnPropertyChanged();
            }
        }

        private static object[] os = new object[0];

        public void Send()
        {
            try
            {
                Client.Invoke("PerformanceTest", InvokeOption.OnlySend, os);//14500
                this.send++;
            }
            catch (Exception)
            {
            }
        }

        public void ShowInfo()
        {
            this.SendCount = send;
            send = 0;
        }
    }
}