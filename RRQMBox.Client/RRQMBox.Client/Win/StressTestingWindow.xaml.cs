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
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;

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

            //byte[] data = Encoding.UTF8.GetBytes(this.Tb_TestContent.Text);
            byte[] data =new byte[1024*10];

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
                    testObject.Client = new TcpClient();
                    testObject.Data = data;
                    testObject.Num = i;

                    try
                    {
                        testObject.Client.Connect(new IPHost("127.0.0.1:7790"));
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
            int size = Math.Min(this.TestObjects.Count, 500);//每个线程托管的客户端
            int threadCount = this.TestObjects.Count / size + 1;
            ThreadPool.SetMinThreads(threadCount,10);
            TestObject[] allObjects = this.TestObjects.ToArray();
            for (int i = 0; i < threadCount; i++)
            {
                TestObject[] testObjects = new TestObject[size];
                Array.Copy(allObjects, i*size,testObjects,0,size);
                Task.Run(()=> 
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

    public class TestObject : ObservableObject
    {
        public byte[] Data { get; set; }
        public static bool IsAsync { get; set; }
        public int Num { get; set; }

        public TcpClient Client { get; set; }

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
