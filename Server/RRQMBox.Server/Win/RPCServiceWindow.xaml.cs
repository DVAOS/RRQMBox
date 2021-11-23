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
using RpcArgsClassLib;
using RRQMSkin.MVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// RPCServiceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RPCServiceWindow : RRQMWindow
    {
        public RPCServiceWindow()
        {
            InitializeComponent();
            Server.ShowMsgMethod = this.ShowMsg;
            this.onLineClient = new RRQMList<RpcSocketClient>();
            this.Lb_OnlineClient.ItemsSource = this.onLineClient;
        }

        private RRQMList<RpcSocketClient> onLineClient;

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

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (rpcService != null)
            {
                rpcService.Dispose();
            }
        }

        private RPCService rpcService;
        TcpRpcParser tcpRPCParser;
        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            if (rpcService != null)
            {
                ShowMsg("服务已经启动");
            }
            int threadCount = int.Parse(this.Tb_ThreadCount.Text);
            rpcService = new RPCService();

            CodeGenerator.AddProxyType(typeof(ProxyClass1));

            tcpRPCParser = new TcpRpcParser();
            tcpRPCParser.Connected += this.TcpRPCParser_ClientConnected;
            tcpRPCParser.Disconnected += this.TcpRPCParser_ClientDisconnected;
            var config = new TcpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(7700) };//监听一个IP地址
            config.ThreadCount = threadCount;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.VerifyTimeout = 3 * 1000;//令箭验证超时时间，3秒
            config.VerifyToken = "123RPC";//令箭值
            config.ProxyToken = "RPC";//默认服务代理令箭
            config.NameSpace = "RRQMTest";//默认代理代码命名空间

            tcpRPCParser.Setup(config);
            tcpRPCParser.Start();
            ShowMsg("TCP解析器添加完成，端口号：7700，VerifyToken=123RPC，ProxyToken=RPC");

            UdpRpcParser udpRPCParser = new UdpRpcParser();

            var udpConfig = new UdpRpcParserConfig();
            udpConfig.ListenIPHosts = new IPHost[] { new IPHost(7701) };
            udpConfig.UseBind = true;
            udpConfig.BufferLength = 1024;
            udpConfig.ThreadCount = threadCount;
            udpConfig.ProxyToken = "RPC";
            udpConfig.NameSpace = "RRQMTest";

            udpRPCParser.Setup(udpConfig);
            udpRPCParser.Start();

            ShowMsg("UDP解析器添加完成");

            WebApiParser webApiParser = new WebApiParser();
            var webApiConfig = new WebApiParserConfig();
            webApiConfig.BufferLength = 1024;
            webApiConfig.ThreadCount = threadCount;//设置多线程数量
            webApiConfig.ClearInterval = -1;//规定不清理无数据客户端
            webApiConfig.ListenIPHosts = new IPHost[] { new IPHost(7703) };
            webApiConfig.ApiDataConverter = new JsonDataConverter();
            webApiParser.Setup(webApiConfig);
            webApiParser.Start();
            ShowMsg("webApiParser解析器添加完成");

            XmlRpcParser xmlRpcParser = new XmlRpcParser();
            var xmlRpcConfig = new XmlRpcParserConfig();
            xmlRpcConfig.BufferLength = 1024;
            xmlRpcConfig.ThreadCount = threadCount;//设置多线程数量
            xmlRpcConfig.ClearInterval = -1;//规定不清理无数据客户端
            xmlRpcConfig.ListenIPHosts = new IPHost[] { new IPHost(7704) };
            xmlRpcParser.Setup(xmlRpcConfig);
            xmlRpcParser.Start();
            ShowMsg("xmlRpcParser解析器添加完成");

            JsonRpcParser jsonRpcParser = new JsonRpcParser();
            var jsonRpcConfig = new JsonRpcParserConfig();
            jsonRpcConfig.BufferLength = 1024;
            jsonRpcConfig.ThreadCount = threadCount;//设置多线程数量
            jsonRpcConfig.ClearInterval = -1;//规定不清理无数据客户端
            jsonRpcConfig.ListenIPHosts = new IPHost[] { new IPHost(7705) };
            jsonRpcConfig.ProtocolType =  JsonRpcProtocolType.Tcp;
           
            jsonRpcParser.Setup(jsonRpcConfig);
            jsonRpcParser.Start();
            ShowMsg("jsonRpcParser解析器添加完成");

            rpcService.AddRPCParser("TcpParser", tcpRPCParser);
            rpcService.AddRPCParser("UdpParser", udpRPCParser);
            rpcService.AddRPCParser("webApiParser", webApiParser);
            rpcService.AddRPCParser("xmlRpcParser", xmlRpcParser);
            rpcService.AddRPCParser("jsonRpcParser", jsonRpcParser);

            rpcService.RegisterServer<Server>();//注册服务
            rpcService.RegisterServer<MyOperation>();//注册服务


            ShowMsg("RPC启动完成");
            ShowMsg("使用浏览器访问以下连接测试WebApi");

            foreach (var url in webApiParser.RouteMap.Urls)
            {
                ShowMsg($"http://127.0.0.1:{webApiParser.Monitors[0].IPHost.Port}{url}");
            }
        }

        private void TcpRPCParser_ClientDisconnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Remove((RpcSocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void TcpRPCParser_ClientConnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Add((RpcSocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void CorrugatedButton_Click(object sender, RoutedEventArgs e)
        {
            this.msgBox.Clear();
        }

        private int callBackCount;

        private void CallBaclButton_Click(object sender, RoutedEventArgs e)
        {
            string id = this.Tb_ID.Text;
            Task.Run(() =>
            {
                if (this.rpcService != null)
                {
                    if (this.rpcService.TryGetRPCParser("TcpParser", out IRPCParser parser))
                    {
                        TcpRpcParser tcpRPCParser = (TcpRpcParser)parser;
                        try
                        {
                            callBackCount++;
                            string msg = tcpRPCParser.CallBack<string>(id, 1000, InvokeOption.WaitInvoke, callBackCount);
                            ShowMsg(msg);
                            tcpRPCParser.CallBack<string>(id, 1000, InvokeOption.WaitInvoke, callBackCount);
                            ShowMsg("无返回值调用成功");
                        }
                        catch (Exception ex)
                        {
                            ShowMsg(ex.Message);
                        }
                    }
                }
            });
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Server.isStart = (bool)((CheckBox)sender).IsChecked;
        }

        private void PublishEventButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.rpcService != null && this.rpcService.TryGetRPCParser("TcpParser", out IRPCParser parser))
            {
                TcpRpcParser tcpRPCParser = (TcpRpcParser)parser;
                //tcpRPCParser.PublishEvent<Action<string>>("TestEvent");
            }
        }

        string path = @"E:\CodeOpen\RRQMSocketFramework\TestDemo\Server\RpcArgsClassLib\bin\Debug\net461\RpcArgsClassLib.dll";
        private void AddServerButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = File.ReadAllBytes(path);
            Assembly assembly = Assembly.Load(data);
            Type serverType = assembly.GetType("RpcArgsClassLib.OtherAssemblyServer");
            rpcService.RegisterServer(serverType);
            ShowMsg("服务增加成功");
        }

        private void RemoveServerButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = File.ReadAllBytes(path);
            Assembly assembly = Assembly.Load(data);
            Type serverType = assembly.GetType("RpcArgsClassLib.OtherAssemblyServer");

            rpcService.UnregisterServer(serverType);
            ShowMsg("服务解除成功");
        }

        private void UpdateServerButton_Click(object sender, RoutedEventArgs e)
        {
            //byte[] data = File.ReadAllBytes(path);
            //Assembly assembly = Assembly.Load(data);
            //Type serverType = assembly.GetType("RpcArgsClassLib.OtherAssemblyServer");

            //rpcService.UpdateRegisteredServer(serverType);
            //ShowMsg("服务更新成功");
        }

        private void CompilerButton_Click(object sender, RoutedEventArgs e)
        {
            this.tcpRPCParser.CompilerProxy();
            ShowMsg("编译成功");
        }
    }
}