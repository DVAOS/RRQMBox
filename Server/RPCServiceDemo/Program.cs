using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RPCServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //实例化RPCService
            RPCService rpcService = new RPCService();

            //添加解析器，解析器根据传输协议，序列化方式的不同，调用RPC服务
            rpcService.AddRPCParser("tcpRpcParser", CreateRRQMTcpParser(7794));
            rpcService.AddRPCParser("udpRpcParser", CreateRRQMUdpParser(7797));
            rpcService.AddRPCParser("webApiParser_Xml", CreateWebApiParser(7800, new XmlDataConverter()));
            rpcService.AddRPCParser("webApiParser_Json", CreateWebApiParser(7801, new JsonDataConverter()));
            rpcService.AddRPCParser("xmlRpcParser", CreateXmlRpcParser(7802));
            rpcService.AddRPCParser("JsonRpcParser_Tcp", CreateJsonRpcParser(7803, JsonRpcProtocolType.Tcp));
            rpcService.AddRPCParser("JsonRpcParser_Http", CreateJsonRpcParser(7804, JsonRpcProtocolType.Http));

            //注册服务
            rpcService.RegisterServer<MyRpcServer>();

            //注册当前程序集的所有服务
            //rpcService.RegisterAllServer();
            Console.WriteLine("RPC服务已启动");

            Console.WriteLine("按任意键显示代理代码");
            Console.ReadKey();

            if (rpcService.TryGetRPCParser("tcpRpcParser", out IRPCParser parser))
            {
                if (parser is TcpRpcParser tcpRpcParser)
                {
                    CellCode[] cellCodes = tcpRpcParser.Codes;
                    foreach (var item in cellCodes)
                    {
                        Console.WriteLine(item.Code);

                        File.WriteAllText(item.Name+".cs",item.Code);
                    }

                    tcpRpcParser.CompilerProxy();
                }
            }

            Console.ReadKey();


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
            UdpRpcParser udpRPCParser = new UdpRpcParser();
            var config = new UdpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };
            config.UseBind = true;
            config.BufferLength = 1024;
            config.ThreadCount = 1;
            config.ProxyToken = "RPC";
            config.NameSpace = "RRQMTest";

            udpRPCParser.Setup(config);

            udpRPCParser.Start();

            Console.WriteLine($"UDP解析器添加完成，端口号：{port}，ProxyToken={udpRPCParser.ProxyToken}");
            return udpRPCParser;
        }
        private static IRPCParser CreateRRQMTcpParser(int port)
        {
            TcpRpcParser tcpRPCParser = new TcpRpcParser();

            //声明配置
            var config = new TcpRpcParserConfig();

            //继承TcpService配置
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };//同时监听两个地址
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.ServerName = "RRQMService";//服务名称
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.ThreadCount = 5;//多线程数量，当SeparateThreadReceive为false时，该值只决定BytePool的数量。
            config.Backlog = 30;
            config.ClearInterval = 60 * 1000;//60秒无数据交互会清理客户端
            config.ClearType = ClearType.Receive | ClearType.Send;//清理统计
            config.MaxCount = 10000;//最大连接数

            //继承TokenService配置
            config.VerifyToken = "123RPC";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //继承ProcotolService配置
            config.CanResetID = true;//是否重新设置ID

            //继承TcpRpcParser配置，以实现RPC交互
            config.NameSpace = "RRQMTest";
            config.RPCVersion = new Version(1, 0, 0, 0);//定义此次发布的RPC版本。
            config.ProxyToken = "RPC";//代理令箭，当客户端获取代理文件,或服务时需验证令箭

            //载入配置
            tcpRPCParser.Setup(config);

            //启动服务
            tcpRPCParser.Start();

            Console.WriteLine($"TCP解析器添加完成，端口号：{port}，VerifyToken={tcpRPCParser.VerifyToken}，ProxyToken={tcpRPCParser.ProxyToken}");
            return tcpRPCParser;
        }
    }


    public class MyRpcServer : ServerProvider
    {
        [RRQMRPC]
        public string TestOne(int id)//同步服务
        {
            return $"若汝棋茗,id={id}";
        }

        [RRQMRPC("TestOne_Name")]//在重载服务时需要重新设定服务唯一键
        public string TestOne(int id, string name)
        {
            return $"若汝棋茗,Name={name},id={id}";
        }

        [RRQMRPC]
        public Task<string> AsyncTestOne(int id)//异步服务,尽量不要用Async结尾，不然生成的异步代码方法将出现两个Async
        {
            return Task.Run(() =>
            {
                return $"若汝棋茗,id={id}";
            });
        }
    }
}
