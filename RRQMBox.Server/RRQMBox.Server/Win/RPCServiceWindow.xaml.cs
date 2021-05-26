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
using RRQMSkin.Windows;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;

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
        }

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
            
        }

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            RPCService rpcService = new RPCService();
            rpcService.RegistAllService();//注册所有服务

            TcpRPCParser tcpRPCParser = new TcpRPCParser();
            tcpRPCParser.SerializeConverter = new BinarySerializeConverter();
            tcpRPCParser.Service.VerifyToken = "123RPC";
            tcpRPCParser.ProxyToken = "RPC";
            tcpRPCParser.NameSpace = "RRQMTest";
            tcpRPCParser.Bind(7700, 5);
            ShowMsg("TCP解析器添加完成，端口号：7700，VerifyToken=123RPC，ProxyToken=RPC");

            UdpRPCParser udpRPCParser = new UdpRPCParser();
            udpRPCParser.SerializeConverter = new BinarySerializeConverter();
            udpRPCParser.NameSpace = "RRQMTest";
            udpRPCParser.Bind(7701, 5);
            ShowMsg("UDP解析器添加完成");

            TcpRPCParser tcpXmlRPCParser = new TcpRPCParser();
            tcpXmlRPCParser.SerializeConverter = new XmlSerializeConverter();
            tcpXmlRPCParser.NameSpace = "RRQMTest";
            tcpXmlRPCParser.Bind(7702, 5);
            ShowMsg("TCPXml解析器添加完成");

            WebApiParser webApiParser = new WebApiParser();
            webApiParser.Bind(7703, 5);
            ShowMsg("webApiParser解析器添加完成");

            XmlRpcParser xmlRpcParser = new XmlRpcParser();
            xmlRpcParser.Bind(7704, 5);
            ShowMsg("xmlRpcParser解析器添加完成");

            JsonRpcParser jsonRpcParser = new JsonRpcParser();
            jsonRpcParser.Bind(7705, 5);
            ShowMsg("jsonRpcParser解析器添加完成");

            rpcService.AddRPCParser("TcpParser", tcpRPCParser);
            rpcService.AddRPCParser("UdpParser", udpRPCParser);
            rpcService.AddRPCParser("tcpXmlRPCParser", tcpXmlRPCParser);
            rpcService.AddRPCParser("webApiParser", webApiParser);
            rpcService.AddRPCParser("xmlRpcParser", xmlRpcParser);
            rpcService.AddRPCParser("jsonRpcParser", jsonRpcParser);

            rpcService.OpenRPCServer();
            ShowMsg("RPC启动完成");
            ShowMsg("使用浏览器访问以下连接测试WebApi");

            foreach (var url in webApiParser.RouteMap.Urls)
            {
                ShowMsg($"http://127.0.0.1:{webApiParser.Service.Port}{url}");
            }
        }
    }
}
