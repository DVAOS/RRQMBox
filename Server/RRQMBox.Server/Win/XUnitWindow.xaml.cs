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
using RRQMCore.ByteManager;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.Net;
using System.Text;
using System.Windows;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// XUnitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class XUnitWindow : RRQMWindow
    {
        public XUnitWindow()
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

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            this.CreateTcpService(7789);
            this.CreateUdpService(7790, 7791);
            this.CreateTokenService(7792);
            this.CreateProtocolService(7793);
            CreateRPCService();
        }

        private void CreateRPCService()
        {
            RPCService rpcService = new RPCService();

            rpcService.AddRPCParser("tcpRPCParser_RRQMBinary", CreateRRQMTcpParser(7794, new BinarySerializeConverter()));
            rpcService.AddRPCParser("tcpRPCParser_Json", CreateRRQMTcpParser(7795, new JsonSerializeConverter()));
            rpcService.AddRPCParser("tcpRPCParser_Xml", CreateRRQMTcpParser(7796, new XmlSerializeConverter()));

            rpcService.AddRPCParser("udpRPCParser_RRQMBinary", CreateRRQMUdpParser(7797, new BinarySerializeConverter()));
            rpcService.AddRPCParser("udpRPCParser_Json", CreateRRQMUdpParser(7798, new JsonSerializeConverter()));
            rpcService.AddRPCParser("udpRPCParser_Xml", CreateRRQMUdpParser(7799, new XmlSerializeConverter()));

            rpcService.AddRPCParser("webApiParser_Xml", CreateWebApiParser(7800, new XmlDataConverter()));
            rpcService.AddRPCParser("webApiParser_Json", CreateWebApiParser(7801, new JsonDataConverter()));

            rpcService.AddRPCParser("xmlRpcParser", CreateXmlRpcParser(7802));

            rpcService.AddRPCParser("JsonRpcParser_Tcp", CreateJsonRpcParser(7803, JsonRpcProtocolType.Tcp));
            rpcService.AddRPCParser("JsonRpcParser_Http", CreateJsonRpcParser(7804, JsonRpcProtocolType.Http));

            rpcService.RegisterServer<Server>();//注册服务
        }
        private IRPCParser CreateJsonRpcParser(int port, JsonRpcProtocolType protocolType)
        {
            JsonRpcParser jsonRpcParser = new JsonRpcParser();

            var config = new JsonRpcParserConfig();
            config.BufferLength = 1024;
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };
            config.ProtocolType = protocolType;

            jsonRpcParser.Setup(config);
            jsonRpcParser.Start();
            ShowMsg($"jsonRpcParser解析器添加完成，端口号：{port}，协议：{protocolType}");
            return jsonRpcParser;
        }

        private IRPCParser CreateXmlRpcParser(int port)
        {
            XmlRpcParser xmlRpcParser = new XmlRpcParser();
            var config = new XmlRpcParserConfig();
            config.BufferLength = 1024;
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };

            xmlRpcParser.Setup(config);
            xmlRpcParser.Start();
          
            ShowMsg($"xmlRpcParser解析器添加完成，端口号：{port}");
            return xmlRpcParser;
        }

        private IRPCParser CreateWebApiParser(int port, ApiDataConverter dataConverter)
        {
            WebApiParser webApiParser = new WebApiParser();
            var config = new WebApiParserConfig();
            config.BufferLength = 1024;
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };
            config.ApiDataConverter = dataConverter;
            webApiParser.Setup(config);
            webApiParser.Start();
            ShowMsg($"webApiParser解析器添加完成，端口号：{port}，序列化器：{dataConverter.GetType().Name}");
            return webApiParser;
        }
        private IRPCParser CreateRRQMUdpParser(int port, SerializeConverter serializeConverter)
        {
            UdpRpcParser udpRPCParser = new UdpRpcParser();
            var config = new UdpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };
            config.UseBind = true;
            config.BufferLength = 1024;
            config.ThreadCount = 1;
            config.SerializeConverter = serializeConverter;
            config.ProxyToken = "RPC";
            config.NameSpace = "RRQMTest";

            udpRPCParser.Setup(config);

            udpRPCParser.Start();

            ShowMsg($"UDP解析器添加完成，端口号：{port}，ProxyToken={udpRPCParser.ProxyToken}，序列化器：{serializeConverter.GetType().Name}");
            return udpRPCParser;
        }

        private IRPCParser CreateRRQMTcpParser(int port, SerializeConverter serializeConverter)
        {
            TcpRpcParser tcpRPCParser = new TcpRpcParser();

            //CodeGenerator.AddProxyType(typeof(ProxyClass1));
            //创建配置
            var config = new TcpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };//监听一个IP地址
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.SerializeConverter = serializeConverter;//选用序列化器
            config.VerifyTimeout = 3 * 1000;//令箭验证超时时间，3秒
            config.VerifyToken = "123RPC";//令箭值
            config.ProxyToken = "RPC";//默认服务代理令箭
            config.NameSpace = "RRQMTest";//默认代理代码命名空间

            //载入配置
            tcpRPCParser.Setup(config);

            //启动服务
            tcpRPCParser.Start();

            ShowMsg($"TCP解析器添加完成，端口号：{port}，VerifyToken={tcpRPCParser.VerifyToken}，ProxyToken={tcpRPCParser.ProxyToken}，序列化器：{serializeConverter.GetType().Name}");
            return tcpRPCParser;
        }

        private void CreateProtocolService(int port)
        {
            SimpleProtocolService service = new SimpleProtocolService();
            service.Received += (SimpleProtocolSocketClient arg1, short? arg2, ByteBlock arg3) =>
            {
                ShowMsg($"ProtocolService收到数据，协议为：{arg2}，数据长度为：{arg3.Len - 2}");
                if (arg2 == 10)
                {
                    arg1.Send(10, Encoding.UTF8.GetBytes(arg1.ID));
                }
                else
                {
                    arg1.Send(arg3.Buffer, 2, arg3.Len - 2);
                }
            };

            //属性设置
            var config = new ServiceConfig();
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(port) })
                .SetValue(ServiceConfig.ThreadCountProperty, 1)//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300 * 1000)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServiceConfig.VerifyTokenProperty, "XUnitTest")//设置验证令箭
                .SetValue(TokenServiceConfig.VerifyTimeoutProperty, 3000);//验证超时时间为3秒

            //方法
            service.Setup(config);
            service.Start();
            ShowMsg($"ProtocolService已启动,端口：{port}");
        }

        private void CreateTokenService(int port)
        {
            SimpleTokenService service = new SimpleTokenService();
            service.Received += (SimpleSocketClient arg1, ByteBlock arg2, object arg3) =>
            {
                arg1.Send(arg2);
            };

            //属性设置
            var config = new ServiceConfig();
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(port) })
                .SetValue(ServiceConfig.ThreadCountProperty, 1)//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300 * 1000)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServiceConfig.VerifyTokenProperty, "XUnitTest")//设置验证令箭
                .SetValue(TokenServiceConfig.VerifyTimeoutProperty, 3000);//验证超时时间为3秒

            //方法
            service.Setup(config);
            service.Start();
            ShowMsg($"TokenService已启动,端口：{port}");
        }

        private void CreateUdpService(int bindPort, int targetPort)
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();
            udpSession.Received += (EndPoint endpoint, ByteBlock e) =>
             {
                 udpSession.Send(e);//将接收到的数据发送至默认终端
             };
            var config = new UdpSessionConfig();//UDP配置
            config.SetValue(UdpSessionConfig.DefaultRemotePointProperty, new IPHost($"127.0.0.1:{targetPort}").EndPoint)//设置默认终结点，用于发送
            .SetValue(UdpSessionConfig.UseBindProperty, true)//是否执行绑定，一般作为接收端时需要绑定
            .SetValue(UdpSessionConfig.ListenIPHostsProperty, new IPHost[] { new IPHost($"127.0.0.1:{bindPort}") })//绑定的IPHost，udp只能绑定一个地址。
            .SetValue(UdpSessionConfig.BufferLengthProperty, 2048)//设置缓存
            .SetValue(UdpSessionConfig.ServerNameProperty, "RRQMUdpServer");//设置服务名称

            udpSession.Setup(config);//加载配置
            udpSession.Start();//启动
            ShowMsg($"UdpService已启动，监听端口：{bindPort}，目标端口：{targetPort}");
        }

        private void CreateTcpService(int port)
        {
            SimpleTcpService tcpService = new SimpleTcpService();

            //订阅初始化事件
            tcpService.CreateSocketCliect += (SimpleSocketClient arg1, CreateOption arg2) =>
            {
                arg1.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//设置数据处理适配器
            };

            //订阅连接事件
            tcpService.ClientConnected += (object sender, MesEventArgs e) =>
            {
                ShowMsg("客户端已连接到TcpService");
            };

            //订阅收到消息事件
            tcpService.Received += (SimpleSocketClient arg1, RRQMCore.ByteManager.ByteBlock arg2, object arg3) =>
            {
                arg1.Send(arg2);
            };

            //订阅断开连接事件
            tcpService.ClientDisconnected += (object sender, MesEventArgs e) =>
            {
                ShowMsg("客户端已断开");
            };

            //注入配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost($"127.0.0.1:{port}") };
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = 300 * 1000;//300秒无数据交互将被清理
            config.ClearType = ClearType.Receive | ClearType.Send;//清除客户端时，Receive或Send均在统计之内。
            config.BufferLength = 1024;//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。

            //载入配置
            tcpService.Setup(config);

            //启动
            tcpService.Start();
            ShowMsg($"TcpService已启动,端口：{port}");
        }
    }

    public class XUnitTestServer : ServerProvider
    {
        [RRQMRPC]
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}