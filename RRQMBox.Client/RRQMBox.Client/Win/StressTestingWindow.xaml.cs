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
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// StressTestingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StressTestingWindow : RRQMWindow
    {
        public StressTestingWindow()
        {
            InitializeComponent();
        }

        public RRQMList<TestObject> TestObjects { get; set; }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isTest)
            {
                MessageBox.Show("测试已经在进行");
                return;
            }
            isTest = true;
            int clientCount = int.Parse(this.ClientCount.Text);

            TestObjects = new RRQMList<TestObject>();
            this.DG.ItemsSource = TestObjects;

            byte[] data = Encoding.UTF8.GetBytes(this.Tb_TestContent.Text);
            //byte[] data =new byte[1024*10];

            TestObject.IsAsync = (bool)this.Cb_IsAsync.IsChecked;
            Task.Run(() =>
            {
                for (int i = 0; i < clientCount; i++)
                {
                    if (!isTest)
                    {
                        break;
                    }
                    TestObject testObject = new TestObject();

                    try
                    {
                        testObject.Client = new SimpleTcpClient();
                        testObject.Data = data;
                        testObject.Num = i;

                        var config = new TcpClientConfig();
                        config.SetValue(TcpClientConfig.OnlySendProperty, true)
                        .SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("192.168.1.12:7790"))
                        .SetValue(TcpClientConfig.DataHandlingAdapterProperty, new FixedHeaderDataHandlingAdapter())
                        .SetValue(TcpClientConfig.SeparateThreadSendProperty, true);

                        testObject.Client.Setup(config);
                        testObject.Client.Connect();
                        testObject.Status = "连接成功";
                    }
                    catch (Exception ex)
                    {
                        testObject.Status = ex.Message;
                    }
                    this.Dispatcher.Invoke(() =>
                    {
                        TestObjects.Add(testObject);
                    });
                }

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
            int size = Math.Min(this.TestObjects.Count, 5);//每个线程托管的客户端
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
            TestObject[] allObjects = this.TestObjects.ToArray();

            for (int i = 0; i < threadCount; i++)
            {
                TestObject[] testObjects = new TestObject[size];
                Array.Copy(allObjects, i * size, testObjects, 0, Math.Min(this.TestObjects.Count - i * size, size));
                Task.Run(() =>
                {
                    Run(testObjects);
                });
            }
        }

        private void Run(TestObject[] testObjects)
        {
            while (isTest)
            {
                foreach (var item in testObjects)
                {
                    item.Send();
                    //return;
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
            TestObject.IsAsync = (bool)this.Cb_IsAsync.IsChecked;
        }

        private void Cb_IsSend_Click(object sender, RoutedEventArgs e)
        {
            if (Cb_IsSend.IsChecked == true)
            {
                TestObject.IsSend = true;
                TestObject.waitHandle.Set();
            }
            else
            {
                TestObject.IsSend = false;
            }
        }
    }

    public class TestObject : ObservableObject
    {
        public byte[] Data { get; set; }
        public static bool IsAsync { get; set; }
        public int Num { get; set; }
        public static EventWaitHandle waitHandle = new AutoResetEvent(false);
        public static bool IsSend = true;
        private SimpleTcpClient client;

        public SimpleTcpClient Client
        {
            get { return client; }
            set
            {
                client = value;
                client.DisconnectedService += this.Client_DisconnectedService;
            }
        }

        private void Client_DisconnectedService(object sender, MesEventArgs e)
        {
            this.Status = "断开连接";
        }

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

        public void Send()
        {
            try
            {
                if (!IsSend)
                {
                    waitHandle.WaitOne();
                }
                if (IsAsync)
                {
                    Client.SendAsync(Data);
                }
                else
                {
                    Client.Send(Data);
                }
                this.send++;
            }
            catch
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