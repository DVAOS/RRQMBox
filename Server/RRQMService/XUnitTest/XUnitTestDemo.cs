//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.ByteManager;
using RRQMService.XUnitTest.Server;
using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.Net;
using System.Text;

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
            RpcService rpcService = new RpcService();

            CodeGenerator.AddProxyType<RpcArgsClassLib.ProxyClass1>();
            CodeGenerator.AddProxyType<RpcArgsClassLib.ProxyClass2>(deepSearch: true);

            rpcService.AddRpcParser("tcpRPCParser", CreateRRQMTcpParser(7794));

            rpcService.AddRpcParser("udpRPCParser", CreateRRQMUdpParser(7797));

            rpcService.AddRpcParser("webApiParser_Xml", CreateWebApiParser(7800, new XmlDataConverter()));
            rpcService.AddRpcParser("webApiParser_Json", CreateWebApiParser(7801, new JsonDataConverter()));

            rpcService.AddRpcParser("xmlRpcParser", CreateXmlRpcParser(7802));

            rpcService.AddRpcParser("JsonRpcParser_Tcp", CreateTcpJsonRpcParser(7803));
            Console.WriteLine($"JsonRpcParser_Tcp解析器添加完成，端口号：{7803}");

            rpcService.AddRpcParser("JsonRpcParser_Http", CreateHTTPJsonRpcParser(7804));
            Console.WriteLine($"JsonRpcParser_Http解析器添加完成，端口号：{7804}");

            rpcService.RegisterServer<XUnitTestController>();//注册服务
            rpcService.ShareProxy(new IPHost(8848));
            Console.WriteLine("RPC代理已开启分享，请通过8848端口获取。");
            Console.WriteLine();
            Console.WriteLine("以下连接用于测试webApi");
            Console.WriteLine($"使用：http://127.0.0.1:7801/XUnitTest/Sum?a=10&b=20");
            Console.WriteLine($"使用：http://127.0.0.1:7801/XUnitTest/Test15_ReturnArgs");
            Console.WriteLine($"使用：http://127.0.0.1:7801/XUnitTest/HttpGetGetListClass01?length=10");


            //RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.WebApi, "RPC");

            //string code = CodeGenerator.ConvertToCode("RRQM", proxyInfo.Codes);
            //Console.WriteLine(code);
        }

        static IRpcParser CreateTcpJsonRpcParser(int port)
        {
            TcpService service = new TcpService();
            service.Connecting += (client, e) =>
            {
                e.DataHandlingAdapter = new TerminatorPackageAdapter(client.MaxPackageSize, "\r\n");
            };

            service.Setup(new RRQMConfig().UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(port) }))
                .Start();

            return service.AddPlugin<JsonRpcParserPlugin>()
                 .SetProxyToken("RPC");
        }

        static IRpcParser CreateHTTPJsonRpcParser(int port)
        {
            HttpService service = new HttpService();

            service.Setup(new RRQMConfig().UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(port) }))
                .Start();

            return service.AddPlugin<JsonRpcParserPlugin>()
                 .SetProxyToken("RPC")
                 .SetJsonRpcUrl("/jsonRpc");
        }

        private static IRpcParser CreateXmlRpcParser(int port)
        {
            HttpService service = new HttpService();

            service.Setup(new RRQMConfig().UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(port) }))
                .Start();

            return service.AddPlugin<XmlRpcParserPlugin>()
                 .SetProxyToken("RPC");
        }

        private static IRpcParser CreateWebApiParser(int port, ApiDataConverter dataConverter)
        {
            HttpService service = new HttpService();

            service.Setup(new RRQMConfig()
                .UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(port) }))
                .Start();

            return service.AddPlugin<WebApiParserPlugin>()
                 .SetProxyToken("RPC")
                 .SetApiDataConverter(dataConverter);
        }

        private static IRpcParser CreateRRQMUdpParser(int port)
        {
            UdpRpc udpRpcParser = new UdpRpc();

            RRQMConfig config = new RRQMConfig();
            config.SetBindIPHost(new IPHost(port))
                .SetBufferLength(1024)
                .SetThreadCount(1)
                .SetProxyToken("RPC");

            udpRpcParser.Setup(config);

            udpRpcParser.Start();

            Console.WriteLine($"UDP解析器添加完成，端口号：{port}，ProxyToken={udpRpcParser.ProxyToken}");
            return udpRpcParser;
        }

        private static IRpcParser CreateRRQMTcpParser(int port)
        {
            TcpRpcParser tcpRPCParser = new TcpRpcParser();

            RRQMConfig config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost(port) })
                .SetBufferLength(1024)
                .SetThreadCount(1)
                .SetProxyToken("RPC")
                .SetVerifyToken("123RPC");

            //载入配置
            tcpRPCParser.Setup(config);

            //启动服务
            tcpRPCParser.Start();

            Console.WriteLine($"TCP解析器添加完成，端口号：{port}，VerifyToken={tcpRPCParser.VerifyToken}，ProxyToken={tcpRPCParser.ProxyToken}");
            return tcpRPCParser;
        }

        private static void CreateProtocolService(int port)
        {
            ProtocolService service = new ProtocolService();
            service.Received += (client, protocol, byteBlock) =>
            {
                Console.WriteLine($"ProtocolService收到数据，协议为：{protocol}，数据长度为：{byteBlock.Len - 2}");
                if (protocol == 10)
                {
                    client.Send(10, Encoding.UTF8.GetBytes(client.ID));
                }
                else
                {
                    client.Send(byteBlock.Buffer, 2, byteBlock.Len - 2);
                }
            };

            RRQMConfig config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost(port) })
                .SetBufferLength(1024)
                .SetThreadCount(1)
                .SetVerifyToken("XUnitTest");

            //方法
            service.Setup(config);
            service.Start();
            Console.WriteLine($"ProtocolService已启动,端口：{port}");
        }

        private static void CreateTokenService(int port)
        {
            TokenService service = new TokenService();
            service.Received += (client, byteBlock, requestInfo) =>
            {
                client.Send(byteBlock);
            };

            service.Connecting += (arg1, arg2) =>
            {
                arg1.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
            };

            RRQMConfig config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost(port) })
                .SetBufferLength(1024)
                .SetThreadCount(1)
                .SetVerifyToken("XUnitTest");

            //方法
            service.Setup(config);
            service.Start();
            Console.WriteLine($"TokenService已启动,端口：{port}");
        }

        private static void CreateUdpService(int bindPort, int targetPort)
        {
            UdpSession udpSession = new UdpSession();
            udpSession.Received += (EndPoint endpoint, ByteBlock e) =>
            {
                udpSession.Send(endpoint, e);//将接收到的数据发送至默认终端
            };

            RRQMConfig config = new RRQMConfig();
            config.SetBindIPHost(new IPHost($"127.0.0.1:{bindPort}"));

            udpSession.Setup(config);//加载配置
            udpSession.Start();//启动
            Console.WriteLine($"UdpService已启动，监听端口：{bindPort}，目标端口：{targetPort}");
        }

        private static void CreateTcpService(int port)
        {
            TcpService tcpService = new TcpService();

            //订阅初始化事件
            tcpService.Connecting += (arg1, arg2) =>
            {
                arg1.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//设置数据处理适配器
            };

            //订阅连接事件
            tcpService.Connected += (client, e) =>
            {
                Console.WriteLine("客户端已连接到TcpService");
            };

            //订阅收到消息事件
            tcpService.Received += (client, byteBlock, requestInfo) =>
            {
                client.Send(byteBlock);
            };

            //订阅断开连接事件
            tcpService.Disconnected += (client, e) =>
            {
                Console.WriteLine("客户端已断开");
            };

            RRQMConfig config = new RRQMConfig();
            config.SetListenIPHosts(new IPHost[] { new IPHost(port) })
                .SetBufferLength(1024)
                .SetThreadCount(1);

            //载入配置
            tcpService.Setup(config);

            //启动
            tcpService.Start();
            Console.WriteLine($"TcpService已启动,端口：{port}");
        }
    }
}
