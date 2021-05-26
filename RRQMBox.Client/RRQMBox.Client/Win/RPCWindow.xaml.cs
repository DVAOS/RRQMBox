using CookComputing.XmlRpc;
using RRQMBox.Client.RPCTest;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RRQMBox.Client.Win
{
    /// <summary>
    /// RPCWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RPCWindow : RRQMWindow
    {
        public RPCWindow()
        {
            InitializeComponent();
        }

        private void ShowMsg(string msg)
        {
            this.UIInvoke(() =>
            {
                this.msgBox.AppendText($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff")}]:{msg}\r\n");
            });
        }

        private void UIInvoke(Action action)
        {
            this.Dispatcher.Invoke(() =>
            {
                action.Invoke();
            });
        }

        private void RRQMBinaryButton_Click(object sender, RoutedEventArgs e)
        {
            BinarySerialize();
        }

        private  void BinarySerialize()
        {
            //序列化默认为二进制

            RPCClient client = new RPCClient();
            client.ReceivedByteBlock += Client_ReceivedByteBlock;

            //开启反向RPC，先注册
            client.RegistService(new CallBackServer());
            client.OpenCallBackRPCServer();

            //TypeInitializeDic pairs = new TypeInitializeDic();
            //pairs.Add("List<RRQMRPC.RRQMTest.Test01>", typeof(List<Test01>));
            //pairs.Add("Dictionary<System.Int32,System.String>", typeof(Dictionary<int, string>));

            ////无法找到的类型，通过TypeInitializeDic显式指定

            client.InitializedRPC(new IPHost("127.0.0.1:7789"));
            Console.WriteLine();
            Console.WriteLine("二进制连接成功");

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.NoFeedback);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            remoteTest.Test11(client.ID);
            remoteTest.Test12();
            remoteTest.Test13();
            remoteTest.Test14();
            remoteTest.Test15(client.ID);//调用服务，然后让服务再回调RPC

            Console.WriteLine("二进制测试完成");
            Console.WriteLine();
        }

        private  void UDPBinarySerialize()
        {
            //UDP序列化默认为二进制

            UdpRPCClient client = new UdpRPCClient();
            client.Bind(8848, 1);

            client.InitializedRPC(new IPHost("127.0.0.1:7790"));
            Console.WriteLine();
            Console.WriteLine("二进制连接成功");

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.NoFeedback);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            Console.WriteLine("UDP二进制测试完成");
            Console.WriteLine();
        }

        private  void Client_ReceivedByteBlock(object sender, RRQMCore.ByteManager.ByteBlock e)
        {
            Console.WriteLine($"收到独立消息：{Encoding.UTF8.GetString(e.Buffer, 0, (int)e.Length)}");
        }

        private  void XmlSerialize()
        {
            RPCClient client = new RPCClient();
            client.SerializeConverter = new XmlSerializeConverter();
            client.InitializedRPC(new IPHost("127.0.0.1:7791"));
            Console.WriteLine();
            Console.WriteLine("Xml连接成功");

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.CanFeedback);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            remoteTest.Test12();
            remoteTest.Test14();

            Console.WriteLine("Xml测试完成");
            Console.WriteLine();
        }

        private  void TestXmlRpc()
        {
            Console.WriteLine("即将测试XmlRpc");
            client iclient;
            XmlRpcClientProtocol protocol;
            iclient = (client)XmlRpcProxyGen.Create(typeof(client));
            protocol = (XmlRpcClientProtocol)iclient;
            protocol.Url = "http://127.0.0.1:7793";
            protocol.KeepAlive = false;

            string mes = iclient.TestXmlRpc("test", 10, 10.00, new Args[] { new Args() { P3 = "P" }, new Args() { P3 = "PP" } }); //调用
            Console.WriteLine($"收到返回数据：{mes}");

            Console.WriteLine("XmlRpc测试结束");
        }
    }

    /// <summary>
    /// 用于反向RPC
    /// </summary>
    public class CallBackServer : ServerProvider
    {
        [RRQMRPCCallBackMethod(1000)]
        public string SayHello(int age)
        {
            string mes = $"Hello,我今年{age}岁了";
            Console.WriteLine(mes);
            return mes;
        }
    }

    public interface client
    {
        [XmlRpcMethod("Server.TestXmlRpc")]
        string TestXmlRpc(string param, int a, double b, Args[] args);
    }

    public class Args
    {
        public int P1 { get; set; }
        public double P2 { get; set; }
        public string P3 { get; set; }
    }
}
