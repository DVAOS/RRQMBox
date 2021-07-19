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
using Newtonsoft.Json;
using RRQMBox.Server.Common;
using RRQMMVVM;
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
using System.Text;
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
            this.onLineClient = new RRQMList<RPCSocketClient>();
            this.Lb_OnlineClient.ItemsSource = this.onLineClient;
        }

        private RRQMList<RPCSocketClient> onLineClient;

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
        TcpRPCParser tcpRPCParser;
        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            if (rpcService != null)
            {
                ShowMsg("服务已经启动");
            }
            int threadCount = int.Parse(this.Tb_ThreadCount.Text);
            rpcService = new RPCService();


            tcpRPCParser = new TcpRPCParser();
            tcpRPCParser.ClientConnected += this.TcpRPCParser_ClientConnected;
            tcpRPCParser.ClientDisconnected += this.TcpRPCParser_ClientDisconnected;
            var config = new ServiceConfig();
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(7700) })
                .SetValue(ServiceConfig.ThreadCountProperty, threadCount)
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 600)
                .SetValue(TcpRPCParserConfig.SerializeConverterProperty, new BinarySerializeConverter())
                .SetValue(TcpRPCParserConfig.ProxyTokenProperty, "RPC")
                .SetValue(TokenServiceConfig.VerifyTokenProperty, "123RPC")
                .SetValue(TcpRPCParserConfig.NameSpaceProperty, "RRQMTest")
                .SetValue(TcpRPCParserConfig.SeparateThreadReceiveProperty, false);

            config.SetValue(TcpRPCParserConfig.SerializeConverterProperty, new JsonSerializeConverter());
            tcpRPCParser.Setup(config);
            tcpRPCParser.Start();
            ShowMsg("TCP解析器添加完成，端口号：7700，VerifyToken=123RPC，ProxyToken=RPC");

            UdpRPCParser udpRPCParser = new UdpRPCParser();
            var udpConfig = new ServiceConfig();
            udpConfig.SetValue(UdpRPCParserConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(7701) })
                .SetValue(UdpRPCParserConfig.UseBindProperty, true)
                .SetValue(UdpRPCParserConfig.BufferLengthProperty, 1024 * 64)
                .SetValue(UdpRPCParserConfig.ThreadCountProperty, 1)
                .SetValue(UdpRPCParserConfig.SerializeConverterProperty, new BinarySerializeConverter())
                .SetValue(UdpRPCParserConfig.ProxyTokenProperty, "RPC")
                .SetValue(UdpRPCParserConfig.NameSpaceProperty, "RRQMTest");

            udpRPCParser.Setup(udpConfig);
            udpRPCParser.Start();

            ShowMsg("UDP解析器添加完成");

            WebApiParser webApiParser = new WebApiParser();

            var webApiConfig = new ServiceConfig();
            webApiConfig.SetValue(WebApiParserConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(7703) })
                .SetValue(WebApiParserConfig.ApiDataConverterProperty, new JsonDataConverter());
            webApiParser.Setup(webApiConfig);
            webApiParser.Start();
            ShowMsg("webApiParser解析器添加完成");

            XmlRpcParser xmlRpcParser = new XmlRpcParser();
            xmlRpcParser.Setup(7704);
            xmlRpcParser.Start();
            ShowMsg("xmlRpcParser解析器添加完成");

            JsonRpcParser jsonRpcParser = new JsonRpcParser();
            var jsonRpcConfig = new ServiceConfig();
            jsonRpcConfig.SetValue(JsonRpcParserConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(7705) })
                .SetValue(JsonRpcParserConfig.JsonFormatConverterProperty, new TestJsonFormatConverter())
                .SetValue(JsonRpcParserConfig.ProtocolTypeProperty, JsonRpcProtocolType.Tcp);
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


            ////通过检索，拿到TcpRPCParser解析器
            //TcpRPCParser parser = (TcpRPCParser)rpcService.RPCParsers["TcpParser"];

            ////通过解析器直接回调
            //string mes = parser.CallBack<string>("ID", 1000, InvokeOption.WaitInvoke, 10);

            ////先拿到socketClient，然后回调
            //if (parser.TryGetSocketClient("ID",out RPCSocketClient socketClient))
            //{
            //   string msg= socketClient.CallBack<string>(1000, InvokeOption.WaitInvoke, 10);
            //}

            ShowMsg("RPC启动完成");
            ShowMsg("使用浏览器访问以下连接测试WebApi");

            foreach (var url in webApiParser.RouteMap.Urls)
            {
                ShowMsg($"http://127.0.0.1:{webApiParser.ListenIPHosts[0].Port}{url}");
            }
        }

        private void TcpRPCParser_ClientDisconnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Remove((RPCSocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void TcpRPCParser_ClientConnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Add((RPCSocketClient)sender);
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
                        TcpRPCParser tcpRPCParser = (TcpRPCParser)parser;
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
                TcpRPCParser tcpRPCParser = (TcpRPCParser)parser;
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
            byte[] data = File.ReadAllBytes(path);
            Assembly assembly = Assembly.Load(data);
            Type serverType = assembly.GetType("RpcArgsClassLib.OtherAssemblyServer");

            rpcService.UpdateRegisteredServer(serverType);
            ShowMsg("服务更新成功");
        }

        private void CompilerButton_Click(object sender, RoutedEventArgs e)
        {
            this.tcpRPCParser.CompilerProxy();
            ShowMsg("编译成功");
        }
    }

    public class JsonSerializeConverter : RRQMSocket.RPC.RRQMRPC.SerializeConverter
    {
        public override object DeserializeParameter(byte[] parameterBytes, Type parameterType)
        {
            if (parameterBytes == null)
            {
                return null;
            }
           
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(parameterBytes), parameterType);
        }

        public override byte[] SerializeParameter(object parameter)
        {
            
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(parameter));
        }
    }
}