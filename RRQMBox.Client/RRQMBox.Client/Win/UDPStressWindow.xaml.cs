using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// UDPStressWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UDPStressWindow : RRQMWindow
    {
        public UDPStressWindow()
        {
            InitializeComponent();
        }
        public RRQMList<UDPTestObject> TestObjects { get; set; }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isTest)
            {
                MessageBox.Show("测试已经在进行");
                return;
            }
            isTest = true;
            int clientCount = int.Parse(this.ClientCount.Text);

            TestObjects = new RRQMList<UDPTestObject>();
            this.DG.ItemsSource = TestObjects;

            byte[] data = Encoding.UTF8.GetBytes(this.Tb_TestContent.Text);
            //byte[] data =new byte[1024*10];

            UDPTestObject.IsAsync = (bool)this.Cb_IsAsync.IsChecked;
            Task.Run(() =>
            {

                for (int i = 0; i < clientCount; i++)
                {
                    if (!isTest)
                    {
                        break;
                    }
                    UDPTestObject testObject = new UDPTestObject();

                    try
                    {

                        testObject.Client = new SimpleUdpSession();
                        testObject.Data = data;
                        testObject.Num = i;

                        var config = new UdpSessionConfig();
                        config.SetValue(UdpSessionConfig.DefaultRemotePointProperty, new IPHost("127.0.0.1:8848").EndPoint);

                        testObject.Client.Setup(config);
                        testObject.Client.Start();
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
            UDPTestObject[] allObjects = this.TestObjects.ToArray();

            for (int i = 0; i < threadCount; i++)
            {
                UDPTestObject[] testObjects = new UDPTestObject[size];
                Array.Copy(allObjects, i * size, testObjects, 0, Math.Min(this.TestObjects.Count - i * size, size));
                Task.Run(() =>
                {
                    Run(testObjects);
                });
            }
        }

        private void Run(UDPTestObject[] testObjects)
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

        bool isTest;
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
            UDPTestObject.IsAsync = (bool)this.Cb_IsAsync.IsChecked;
        }

        private void Cb_IsSend_Click(object sender, RoutedEventArgs e)
        {
            if (Cb_IsSend.IsChecked == true)
            {
                UDPTestObject.IsSend = true;
                UDPTestObject.waitHandle.Set();
            }
            else
            {
                UDPTestObject.IsSend = false;
            }
        }
    }
    public class UDPTestObject : ObservableObject
    {
        public byte[] Data { get; set; }
        public static bool IsAsync { get; set; }
        public int Num { get; set; }
        public static EventWaitHandle waitHandle = new AutoResetEvent(false);
        public static bool IsSend = true;
        private SimpleUdpSession client;

        public SimpleUdpSession Client
        {
            get { return client; }
            set
            {
                client = value;
            }
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
