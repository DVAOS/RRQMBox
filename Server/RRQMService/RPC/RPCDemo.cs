using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.RPC
{
    public static class RPCDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.启动RPC服务器");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        StartRPCService();
                        break;
                    }
                default:
                    break;
            }
        }

        static void StartRPCService()
        {
            //实例化RPCService
            RPCService rpcService = new RPCService();

            //添加解析器，解析器根据传输协议，序列化方式的不同，调用RPC服务
            rpcService.AddRPCParser("tcpRpcParser", CreateRRQMTcpParser(7789));

            //注册服务
            rpcService.RegisterAllServer();

            //注册当前程序集的所有服务
            //rpcService.RegisterAllServer();
            Console.WriteLine("RPC服务已启动");

            rpcService.ShareProxy(new IPHost(8848));
            Console.WriteLine("代理文件以8848端口分享，如果不想分享代理，请注释分享。分享代理和调用互不影响。");
            Console.WriteLine("按任意键显示代理代码");
            Console.ReadKey();

            RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.RRQMRPC, "RPC");
            string code = CodeGenerator.ConvertToCode(proxyInfo.Namespace, proxyInfo.Codes);

            Console.WriteLine(code);
            Console.ReadKey();
        }

        private static IRPCParser CreateRRQMTcpParser(int port)
        {
            TcpRpcParser qosTcpRpcParser = new TcpRpcParser();

            //声明配置
            var config = new TcpRpcParserConfig();

            //继承TcpService配置
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };//同时监听两个地址
            //继承TokenService配置
            config.VerifyToken = "123RPC";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时
            config.ThreadCount = 20;

            //继承ProcotolService配置
            config.CanResetID = true;//是否重新设置ID

            //继承TcpRpcParser配置，以实现RPC交互
            config.ProxyToken = "RPC";//代理令箭，当客户端获取代理文件,或服务时需验证令箭

            //载入配置
            qosTcpRpcParser.Setup(config);

            //启动服务
            qosTcpRpcParser.Start();

            Console.WriteLine($"TCP解析器添加完成，端口号：{port}，VerifyToken={qosTcpRpcParser.VerifyToken}，ProxyToken={qosTcpRpcParser.ProxyToken}");
            return qosTcpRpcParser;
        }
    }
}
