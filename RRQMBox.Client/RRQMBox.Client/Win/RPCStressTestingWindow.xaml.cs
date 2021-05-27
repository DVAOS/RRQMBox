using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
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

            byte[] data = Encoding.UTF8.GetBytes(this.Tb_TestContent.Text);
            //byte[] data =new byte[1024];

            TestObject.IsAsync = (bool)this.Cb_IsAsync.IsChecked;
            Task.Run(() =>
            {
                for (int i = 0; i < clientCount; i++)
                {
                    if (!isTest)
                    {
                        break;
                    }
                    RPCTestObject testObject = new RPCTestObject();
                    testObject.Client = new RPCClient();
                    testObject.Data = data;
                    testObject.Num = i;

                    try
                    {
                        testObject.Client.InitializedRPC(new IPHost("127.0.0.1:7700"), "123RPC");
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
            int size = Math.Min(this.TestObjects.Count, 50);//每个线程托管的客户端
            int threadCount = this.TestObjects.Count / size + 1;
            ThreadPool.SetMinThreads(threadCount, 10);
            RPCTestObject[] allObjects = this.TestObjects.ToArray();
            for (int i = 0; i < threadCount; i++)
            {
                RPCTestObject[] testObjects = new RPCTestObject[size];
                Array.Copy(allObjects, i * size, testObjects, 0, size);
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
            TestObject.IsAsync = (bool)this.Cb_IsAsync.IsChecked;
        }
    }

    public class RPCTestObject : ObservableObject
    {
        public byte[] Data { get; set; }
        public static bool IsAsync { get; set; }
        public int Num { get; set; }

        public RPCClient Client { get; set; }

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
                Client.RPCInvoke("PerformanceTest", InvokeOption.NoFeedback, os);
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
