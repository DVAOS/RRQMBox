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
using RRQMService.RPC.Server;
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
            rpcService.RegisterServer<RpcServer>();
            rpcService.RegisterServer<PerformanceRpcServer>();
            rpcService.RegisterServer<ElapsedTimeRpcServer>();
            rpcService.RegisterServer<InstanceRpcServer>();
            rpcService.RegisterServer<GetCallerRpcServer>();

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
