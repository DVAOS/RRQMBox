using RRQMCore.ByteManager;
using RRQMService.XUnitTest.Server;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.XUnitTest
{
    public static class XUnitTestDemo
    {
        public static void Start()
        {
            CreateTcpService(7789);
            CreateUdpService(7790, 7791);
            CreateTokenService(7792);
            CreateProtocolService(7793);
            CreateRPCService();//7794,7797,7800,7801,7802,7803,7804,8848
        }
        private static void CreateRPCService()
        {
            RPCService rpcService = new RPCService();

            CodeGenerator.AddProxyType<RpcArgsClassLib.ProxyClass1>();
            CodeGenerator.AddProxyType<RpcArgsClassLib.ProxyClass2>(deepSearch: true);

            rpcService.AddRPCParser("tcpRPCParser", CreateRRQMTcpParser(7794));

            rpcService.AddRPCParser("udpRPCParser", CreateRRQMUdpParser(7797));

            rpcService.AddRPCParser("webApiParser_Xml", CreateWebApiParser(7800, new XmlDataConverter()));
            rpcService.AddRPCParser("webApiParser_Json", CreateWebApiParser(7801, new JsonDataConverter()));

            rpcService.AddRPCParser("xmlRpcParser", CreateXmlRpcParser(7802));

            rpcService.AddRPCParser("JsonRpcParser_Tcp", CreateJsonRpcParser(7803, JsonRpcProtocolType.Tcp));
            rpcService.AddRPCParser("JsonRpcParser_Http", CreateJsonRpcParser(7804, JsonRpcProtocolType.Http));
            rpcService.RegisterServer<XUnitTestServer>();//注册服务

            rpcService.ShareProxy(new IPHost(8848));
            Console.WriteLine("RPC代理已开启分享，请通过8848端口获取。");

            //RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.RRQMRPC | RpcType.JsonRpc | RpcType.XmlRpc, "RPC");

            //string code = CodeGenerator.ConvertToCode("RRQM", proxyInfo.Codes);
            //byte[] data = SerializeConvert.RRQMBinarySerialize(proxyInfo);
            //var obj = SerializeConvert.RRQMBinaryDeserialize<RpcProxyInfo>(data, 0);
        }

        private static IRPCParser CreateJsonRpcParser(int port, JsonRpcProtocolType protocolType)
        {
            JsonRpcParser jsonRpcParser = new JsonRpcParser();

            var config = new JsonRpcParserConfig();
            config.BufferLength = 1024;
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };
            config.ProtocolType = protocolType;
            config.ProxyToken = "RPC";
            jsonRpcParser.Setup(config);
            jsonRpcParser.Start();
            Console.WriteLine($"jsonRpcParser解析器添加完成，端口号：{port}，协议：{protocolType}");
            return jsonRpcParser;
        }

        private static IRPCParser CreateXmlRpcParser(int port)
        {
            XmlRpcParser xmlRpcParser = new XmlRpcParser();
            var config = new XmlRpcParserConfig();
            config.BufferLength = 1024;
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };
            config.ProxyToken = "RPC";
            xmlRpcParser.Setup(config);
            xmlRpcParser.Start();

            Console.WriteLine($"xmlRpcParser解析器添加完成，端口号：{port}");
            return xmlRpcParser;
        }

        private static IRPCParser CreateWebApiParser(int port, ApiDataConverter dataConverter)
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
            Console.WriteLine($"webApiParser解析器添加完成，端口号：{port}，序列化器：{dataConverter.GetType().Name}");
            return webApiParser;
        }

        private static IRPCParser CreateRRQMUdpParser(int port)
        {
            UdpRpc udpRPCParser = new UdpRpc();
            var config = new UdpRpcParserConfig();
            config.BindIPHost = new IPHost(port);
            config.BufferLength = 1024;
            config.ThreadCount = 1;
            config.ProxyToken = "RPC";

            udpRPCParser.Setup(config);

            udpRPCParser.Start();

            Console.WriteLine($"UDP解析器添加完成，端口号：{port}，ProxyToken={udpRPCParser.ProxyToken}");
            return udpRPCParser;
        }

        private static IRPCParser CreateRRQMTcpParser(int port)
        {
            TcpRpcParser tcpRPCParser = new TcpRpcParser();

            //创建配置
            var config = new TcpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };//监听一个IP地址
            config.ThreadCount = 1;//设置多线程数量
            config.ClearInterval = -1;//规定不清理无数据客户端
            config.VerifyTimeout = 3 * 1000;//令箭验证超时时间，3秒
            config.VerifyToken = "123RPC";//令箭值
            config.ProxyToken = "RPC";//默认服务代理令箭
            //载入配置
            tcpRPCParser.Setup(config);

            //启动服务
            tcpRPCParser.Start();

            Console.WriteLine($"TCP解析器添加完成，端口号：{port}，VerifyToken={tcpRPCParser.VerifyToken}，ProxyToken={tcpRPCParser.ProxyToken}");
            return tcpRPCParser;
        }

        private static void CreateProtocolService(int port)
        {
            SimpleProtocolService service = new SimpleProtocolService();
            service.Received += (SimpleProtocolSocketClient arg1, short arg2, ByteBlock arg3) =>
            {
                Console.WriteLine($"ProtocolService收到数据，协议为：{arg2}，数据长度为：{arg3.Len - 2}");
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
            config.SetValue(TcpServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(port) })
                .SetValue(ServiceConfig.ThreadCountProperty, 1)//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300 * 1000)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServiceConfig.VerifyTokenProperty, "XUnitTest")//设置验证令箭
                .SetValue(TokenServiceConfig.VerifyTimeoutProperty, 3000);//验证超时时间为3秒

            //方法
            service.Setup(config);
            service.Start();
            Console.WriteLine($"ProtocolService已启动,端口：{port}");
        }

        private static void CreateTokenService(int port)
        {
            SimpleTokenService service = new SimpleTokenService();
            service.Received += (arg1, arg2, arg3) =>
            {
                arg1.Send(arg2);
            };

            service.Connecting += (arg1, arg2) =>
            {
                arg1.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
            };

            //属性设置
            var config = new ServiceConfig();
            config.SetValue(TcpServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(port) })
                .SetValue(ServiceConfig.ThreadCountProperty, 1)//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300 * 1000)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServiceConfig.VerifyTokenProperty, "XUnitTest")//设置验证令箭
                .SetValue(TokenServiceConfig.VerifyTimeoutProperty, 3000);//验证超时时间为3秒

            //方法
            service.Setup(config);
            service.Start();
            Console.WriteLine($"TokenService已启动,端口：{port}");
        }

        private static void CreateUdpService(int bindPort, int targetPort)
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();
            udpSession.Received += (EndPoint endpoint, ByteBlock e) =>
            {
                udpSession.Send(endpoint, e);//将接收到的数据发送至默认终端
            };
            var config = new UdpSessionConfig();//UDP配置
            config.BindIPHost = new IPHost($"127.0.0.1:{bindPort}");

            udpSession.Setup(config);//加载配置
            udpSession.Start();//启动
            Console.WriteLine($"UdpService已启动，监听端口：{bindPort}，目标端口：{targetPort}");
        }

        private static void CreateTcpService(int port)
        {
            SimpleTcpService tcpService = new SimpleTcpService();

            //订阅初始化事件
            tcpService.Connecting += (arg1, arg2) =>
            {
                arg1.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//设置数据处理适配器
            };

            //订阅连接事件
            tcpService.Connected += (SimpleSocketClient client, MesEventArgs e) =>
            {
                Console.WriteLine("客户端已连接到TcpService");
            };

            //订阅收到消息事件
            tcpService.Received += (SimpleSocketClient arg1, RRQMCore.ByteManager.ByteBlock arg2, object arg3) =>
            {
                arg1.Send(arg2);
            };

            //订阅断开连接事件
            tcpService.Disconnected += (SimpleSocketClient client, MesEventArgs e) =>
            {
                Console.WriteLine("客户端已断开");
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
            Console.WriteLine($"TcpService已启动,端口：{port}");
        }
    }
}
