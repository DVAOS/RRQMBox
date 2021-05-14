using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using RRQMMVVM;
using RRQMRPC.RRQMTest;
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;

namespace RPCStressTesting
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public RRQMList<TestObject> TestObjects { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ThreadPool.SetMaxThreads(1000, 1000);

            TestObjects = new RRQMList<TestObject>();
            this.DG.ItemsSource = TestObjects;
            int clientCount = int.Parse(this.ClientCount.Text);
            int testCount = int.Parse(this.TestCount.Text);
            Task.Run(() =>
            {
                for (int i = 0; i < clientCount; i++)
                {
                    TestObject testObject = new TestObject();
                    testObject.Client = new RPCClient();
                    testObject.TestCount = testCount;
                    TypeInitializeDic pairs = new TypeInitializeDic();
                    pairs.Add("List<RRQMRPC.RRQMTest.Test01>", typeof(List<Test01>));
                    pairs.Add("Dictionary<System.Int32,System.String>", typeof(Dictionary<int, string>));

                    //无法找到的类型，通过TypeInitializeDic显式指定
                    try
                    {
                        testObject.Client.InitializedRPC(new IPHost("127.0.0.1:7789"), typeDic: pairs);
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

                foreach (var item in this.TestObjects)
                {
                    item.Start();
                }
            });
        }
    }

    public class TestObject : ObservableObject
    {
        public RPCClient Client { get; set; }

        public int Num { get; set; }

        private string status;

        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        private int successCount;

        public int SuccessCount
        {
            get { return successCount; }
            set 
            { 
                successCount = value;
                if (successCount% tick == 0)
                {
                    OnPropertyChanged();
                }
               
            }
        }

        private int testCount;

        public int TestCount
        {
            get { return testCount; }
            set 
            { 
                testCount = value;
                if (value > 100)
                {
                    tick = (int)(value / 100.0);
                }
            }
        }

        private int tick=1;

        private TimeSpan timeSpan;

        public TimeSpan TimeSpan
        {
            get { return timeSpan; }
            set { timeSpan = value; OnPropertyChanged(); }
        }

        public void Start()
        {
            Thread thread = new Thread(()=> 
            {
                this.TimeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                {
                    int count = 0;
                    while (count < TestCount)
                    {
                        try
                        {
                            object[] ps = new object[0];
                            Client.RPCInvoke("TestNullReturnNullParameter", ref ps, InvokeOption.NoFeedback);
                            SuccessCount++;
                        }
                        finally
                        {
                            count++;
                        }
                    }
                });
            });
            thread.Start();
        }
    }
}