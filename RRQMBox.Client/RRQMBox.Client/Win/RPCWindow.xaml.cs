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
using CookComputing.XmlRpc;
using RRQMBox.Client.Common;
using RRQMBox.Client.RPCTest;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

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
            RemoteTest.ShowMsgMethod = this.ShowMsg;
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

        private void RRQMUDPBinaryButton_Click(object sender, RoutedEventArgs e)
        {
            UDPBinarySerialize();
        }

        private void XmlRpcButton_Click(object sender, RoutedEventArgs e)
        {
            
                TestXmlRpc();
           
        }

        private void JsonRpcButton_Click(object sender, RoutedEventArgs e)
        {
           
                TestJsonRpc();
           
        }

        private void IDInvokenButton_Click(object sender, RoutedEventArgs e)
        {
            IDRPCWindow window = new IDRPCWindow();
            window.Show();
        }

        private void BinarySerialize()
        {
            //序列化默认为二进制
            TcpRPCClient client = new TcpRPCClient();
            var config = new TcpRPCClientConfig();
            config.SetValue(RRQMConfig.LoggerProperty, new MsgLog(this.ShowMsg))
                .SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7700"))
                .SetValue(TokenClientConfig.VerifyTokenProperty, "123RPC");

            //开启反向RPC，先注册
            client.RegistServer(new CallBackServer());
            client.OpenCallBackServer();

            client.Setup(config);
            client.Connect();

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.OnlySend);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test11(client.ID);
            remoteTest.Test12();
            remoteTest.Test13();
            remoteTest.Test14();
            remoteTest.Test15(client.ID);//调用服务，然后让服务再回调RPC

            ShowMsg("二进制测试完成");
        }

        private void GetProxyFileButton_Click(object sender, RoutedEventArgs e)
        {
            TcpRPCClient client = new TcpRPCClient();
            var config = new TcpRPCClientConfig();
            config.SetValue(RRQMConfig.LoggerProperty, new MsgLog(this.ShowMsg))
                .SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7700"))
                .SetValue(TokenClientConfig.VerifyTokenProperty, "123RPC")
                .SetValue(TcpRPCClientConfig.ProxyTokenProperty, "RPC");

            client.Setup(config);
            client.Connect();

            RPCProxyInfo proxyInfo = client.GetProxyInfo();
            if (proxyInfo != null)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    foreach (var item in proxyInfo.Codes)
                    {
                        File.WriteAllText(Path.Combine(fbd.SelectedPath, item.Name), item.Code);
                    }
                }
            }
        }

        private void UDPBinarySerialize()
        {
            UdpRPCClient client = new UdpRPCClient();

            var config = new ServerConfig();
            config.SetValue(UdpRPCClientConfig.DefaultRemotePointProperty, new IPHost("127.0.0.1:7701").EndPoint)
                .SetValue(UdpRPCClientConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(8848) })
                .SetValue(RRQMConfig.BufferLengthProperty, 1024 * 64)
                .SetValue(UdpRPCClientConfig.ThreadCountProperty, 1)
                .SetValue(UdpRPCClientConfig.UseBindProperty, true);
            client.Setup(config);
            client.Start();

            client.InitializeRPC();

            ShowMsg("UDP初始化成功");

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.OnlySend);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test12();
            remoteTest.Test13();
            remoteTest.Test14();
            ShowMsg("UDP二进制测试完成");
        }

        //private void Client_ReceivedByteBlock(object sender, RRQMCore.ByteManager.ByteBlock e)
        //{
        //    ShowMsg($"收到独立消息：{Encoding.UTF8.GetString(e.Buffer, 0, (int)e.Length)}");
        //}



        private void TestXmlRpc()
        {
            ShowMsg("即将测试XmlRpc");
            IClient iclient;
            XmlRpcClientProtocol protocol;
            iclient = (IClient)XmlRpcProxyGen.Create(typeof(IClient));
            protocol = (XmlRpcClientProtocol)iclient;
            protocol.Url = "http://127.0.0.1:7704";
            protocol.KeepAlive = false;

            string mes = iclient.TestXmlRpc("test", 10, 10.00, new Args[] { new Args() { P3 = "P" }, new Args() { P3 = "PP" } }); //调用
            ShowMsg($"收到返回数据：{mes}");

            ShowMsg("XmlRpc测试结束");
        }

        private void TestJsonRpc()
        {
            SimpleTcpClient tcpClient = new SimpleTcpClient();
            tcpClient.Received += this.TcpClient_Received;
            var config = new TcpClientConfig();
            config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7705"));
            tcpClient.Setup(config);
            tcpClient.Connect();
            tcpClient.Send(Encoding.UTF8.GetBytes("{\"jsonrpc\":\"2.0\",\"method\":\"TestJsonRpc\",\"params\":[5],\"id\":1}\r\n"));
        }

        private void TcpClient_Received(RRQMCore.ByteManager.ByteBlock arg1, object arg2)
        {
            string s = Encoding.UTF8.GetString(arg1.Buffer, 0, (int)arg1.Length);
            ShowMsg("JsonRpc返回数据：" + s);
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
            return mes;
        }
    }

    public interface IClient
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