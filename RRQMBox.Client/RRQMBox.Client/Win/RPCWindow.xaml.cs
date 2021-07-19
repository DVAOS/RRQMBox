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
using Newtonsoft.Json;
using RRQMBox.Client.Common;
using RRQMBox.Client.RPCTest;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.Collections.Generic;
using System.IO;
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

        TcpRPCClient client;
        private void BinarySerialize()
        {
            //序列化默认为二进制
            if (client == null)
            {
                client = new TcpRPCClient();
                client.ServiceDiscovered += this.Client_RPCInitialized1;
                var config = new TcpRPCClientConfig();
                config.SetValue(RRQMConfig.LoggerProperty, new MsgLog(this.ShowMsg))
                    .SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7700"))
                    .SetValue(TokenClientConfig.VerifyTokenProperty, "123RPC")
                    .SetValue(TcpRPCClientConfig.ProxyTokenProperty, "RPC");

                
                config.SetValue(TcpRPCClientConfig.SerializeConverterProperty,new JsonSerializeConverter());

                //开启反向RPC，先注册
                client.RegisterServer<CallBackServer>();
                client.Setup(config);

                client.DiscoveryService();

                //与InitializeRPC等效。
                //client.Connect();

            }

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
            remoteTest.Test16();
            remoteTest.Test17();
            remoteTest.Test18();
            remoteTest.Test19();

            ShowMsg("二进制测试完成");
        }

        private void Client_RPCInitialized1(object sender, MesEventArgs e)
        {
            ShowMsg("初始化完成");
        }

        private void ReGetServerProxyButton_Click(object sender, RoutedEventArgs e)
        {
            this.client.DiscoveryService(true);
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
            client.ServiceDiscovered += this.Client_RPCInitialized;
            var config = new ServiceConfig();
            config.SetValue(UdpRPCClientConfig.DefaultRemotePointProperty, new IPHost("127.0.0.1:7701").EndPoint)
                .SetValue(UdpRPCClientConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(8848) })
                .SetValue(RRQMConfig.BufferLengthProperty, 1024 * 64)
                .SetValue(UdpRPCClientConfig.ThreadCountProperty, 1)
                .SetValue(UdpRPCClientConfig.UseBindProperty, true)
                .SetValue(UdpRPCClientConfig.ProxyTokenProperty, "RPC");
            client.Setup(config);
            client.Start();

            client.DiscoveryService();

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

        private void Client_RPCInitialized(object sender, MesEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TestXmlRpc()
        {
            ShowMsg("即将使用XmlRpcV2测试XmlRpc");
            IClient iclient;
            XmlRpcClientProtocol protocol;
            iclient = (IClient)XmlRpcProxyGen.Create(typeof(IClient));
            protocol = (XmlRpcClientProtocol)iclient;
            protocol.Url = "http://127.0.0.1:7704";
            protocol.KeepAlive = false;

            string mes = iclient.TestXmlRpc("test", 10, 10.00, new Args[] { new Args() { P3 = "P" }, new Args() { P3 = "PP" } }); //调用
            ShowMsg($"收到返回数据：{mes}");

            ShowMsg("即将测试RRQMXmlRpc");
            XmlRPCClient client = new XmlRPCClient();
            var config = new TcpRPCClientConfig();
            config.SetValue(RRQMConfig.LoggerProperty, new MsgLog(this.ShowMsg))
                .SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7704"));

            client.Setup(config);
            client.Connect();

            RemoteTest remoteTest = new RemoteTest(client);
            remoteTest.Test01(InvokeOption.WaitInvoke);
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            remoteTest.Test12();
            remoteTest.Test14();
            ShowMsg("RRQMXmlRpc测试完成");
        }

        private void TestJsonRpc()
        {
            JsonRPCClient client = new JsonRPCClient();
            var config = new TcpRPCClientConfig();
            config.SetValue(RRQMConfig.LoggerProperty, new MsgLog(this.ShowMsg))
                .SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7705"))
                .SetValue(JsonRPCClientConfig.JsonFormatConverterProperty, new TestJsonFormatConverter())//此处序列化器使用的是Json库
                .SetValue(JsonRPCClientConfig.ProtocolTypeProperty, JsonRpcProtocolType.Tcp);

            client.Setup(config);

            client.Connect();

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.WaitInvoke);
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            remoteTest.Test12();
            remoteTest.Test13();
            remoteTest.Test14();
            remoteTest.Test16();
            remoteTest.Test17();
            remoteTest.Test19();
            ShowMsg("JsonRPC测试完成");
        }


    }

    /// <summary>
    /// 用于反向RPC
    /// </summary>
    public class CallBackServer : ServerProvider
    {
        [RRQMRPCCallBackMethod(1000)]//此处使用int值标识MethodToken
        public string SayHello(int age)
        {
            string mes = $"Hello,我今年{age}岁了";
            return mes;
        }
    }

    public interface IClient
    {
        [XmlRpcMethod("TestXmlRpc")]
        string TestXmlRpc(string param, int a, double b, Args[] args);
    }

    public class Args
    {
        public int P1 { get; set; }
        public double P2 { get; set; }
        public string P3 { get; set; }
    }
}