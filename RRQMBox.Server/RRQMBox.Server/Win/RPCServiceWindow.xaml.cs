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
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
//using RRQMSocket.RPC;
//using RRQMSocket.RPC.JsonRpc;
//using RRQMSocket.RPC.RRQMRPC;
//using RRQMSocket.RPC.WebApi;
//using RRQMSocket.RPC.XmlRpc;

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

        RRQMList<RPCSocketClient> onLineClient;
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
        RPCService rpcService;
        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            if (rpcService != null)
            {
                ShowMsg("服务已经启动");
            }
            int threadCount = int.Parse(this.Tb_ThreadCount.Text);
            rpcService = new RPCService();
            rpcService.RegistAllServer();//注册所有服务

            TcpRPCParser tcpRPCParser = new TcpRPCParser();
            tcpRPCParser.ClientConnected += this.TcpRPCParser_ClientConnected;
            tcpRPCParser.ClientDisconnected += this.TcpRPCParser_ClientDisconnected;
            var config = new ServerConfig();
            config.SetValue(ServerConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(7700) })
                .SetValue(ServerConfig.ThreadCountProperty, threadCount)
                .SetValue(TcpServerConfig.ClearIntervalProperty, 600)
                .SetValue(TcpRPCParserConfig.SerializeConverterProperty, new BinarySerializeConverter())
                .SetValue(TcpRPCParserConfig.ProxyTokenProperty, "RPC")
                .SetValue(TokenServerConfig.VerifyTokenProperty, "123RPC")
                .SetValue(TcpRPCParserConfig.NameSpaceProperty, "RRQMTest");
            tcpRPCParser.Setup(config);
            tcpRPCParser.Start();
            ShowMsg("TCP解析器添加完成，端口号：7700，VerifyToken=123RPC，ProxyToken=RPC");

            //UdpRPCParser udpRPCParser = new UdpRPCParser();
            //udpRPCParser.SerializeConverter = new BinarySerializeConverter();
            //udpRPCParser.NameSpace = "RRQMTest";
            //udpRPCParser.Bind(7701, threadCount);
            //ShowMsg("UDP解析器添加完成");

            //TcpRPCParser tcpXmlRPCParser = new TcpRPCParser();
            //tcpXmlRPCParser.SerializeConverter = new XmlSerializeConverter();
            //tcpXmlRPCParser.NameSpace = "RRQMTest";
            //tcpXmlRPCParser.Bind(7702, threadCount);
            //ShowMsg("TCPXml解析器添加完成");

            //WebApiParser webApiParser = new WebApiParser();
            //webApiParser.Bind(7703, threadCount);
            //ShowMsg("webApiParser解析器添加完成");

            //XmlRpcParser xmlRpcParser = new XmlRpcParser();
            //xmlRpcParser.Bind(7704, threadCount);
            //ShowMsg("xmlRpcParser解析器添加完成");

            //JsonRpcParser jsonRpcParser = new JsonRpcParser();
            //jsonRpcParser.Bind(7705, threadCount);
            //ShowMsg("jsonRpcParser解析器添加完成");

            rpcService.AddRPCParser("TcpParser", tcpRPCParser);
            //rpcService.AddRPCParser("UdpParser", udpRPCParser);
            //rpcService.AddRPCParser("tcpXmlRPCParser", tcpXmlRPCParser);
            //rpcService.AddRPCParser("webApiParser", webApiParser);
            //rpcService.AddRPCParser("xmlRpcParser", xmlRpcParser);
            //rpcService.AddRPCParser("jsonRpcParser", jsonRpcParser);

            rpcService.OpenServer();
            ShowMsg("RPC启动完成");
            //ShowMsg("使用浏览器访问以下连接测试WebApi");

            //foreach (var url in webApiParser.RouteMap.Urls)
            //{
            //    ShowMsg($"http://127.0.0.1:{webApiParser.Service.Port}{url}");
            //}
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

        int callBackCount;
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
                            string msg = tcpRPCParser.CallBack<string>(id, 1000, InvokeOption.CanFeedback, callBackCount);
                            ShowMsg(msg);
                            tcpRPCParser.CallBack<string>(id, 1000, InvokeOption.CanFeedback, callBackCount);
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
            if (this.rpcService!=null && this.rpcService.TryGetRPCParser("TcpParser", out IRPCParser parser))
            {
                TcpRPCParser tcpRPCParser =(TcpRPCParser) parser;
                tcpRPCParser.PublishEvent<Action<string>>("TestEvent");
            }
        }
    }
}
