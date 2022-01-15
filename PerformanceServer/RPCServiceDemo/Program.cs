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
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPCServiceDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //实例化RPCService
            RPCService rpcService = new RPCService();

            //添加解析器，解析器根据传输协议，序列化方式的不同，调用RPC服务
            rpcService.AddRPCParser("qosTcpRpcParser", CreateRRQMTcpParser(7794));

            //注册服务
            rpcService.RegisterAllServer();

            //注册当前程序集的所有服务
            //rpcService.RegisterAllServer();
            Console.WriteLine("RPC服务已启动");

            Console.WriteLine("按任意键显示代理代码");
            Console.ReadKey();

            RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.RRQMRPC, "RPC");
            string code = CodeGenerator.ConvertToCode(proxyInfo.Namespace, proxyInfo.Codes);

            Console.WriteLine(code);
            Console.ReadKey();
        }

        private static IRPCParser CreateRRQMTcpParser(int port)
        {
            QosTcpRpcParser qosTcpRpcParser = new QosTcpRpcParser();

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

    internal class QosTcpRpcParser : TcpRpcParser
    {
        /// <summary>
        /// 在获取代理时筛选，
        /// 仅筛选代理代码功能，并不能决定服务能不能调用
        /// </summary>
        /// <param name="args"></param>
        public override void GetProxyInfo(GetProxyInfoArgs args)
        {
            if (args.ProxyToken.StartsWith("RPC"))
            {
                string ser = args.ProxyToken.Replace("RPC", string.Empty);
                foreach (var item in this.RPCService.ServerProviders)
                {
                    if (item.GetType().Name.Contains(ser))
                    {
                        ServerCellCode serverCellCode = CodeGenerator.Generator<RRQMRPCAttribute>(item.GetType());
                        args.Codes.Add(serverCellCode);
                    }
                }
            }
            else
            {
                args.IsSuccess = false;
                args.ErrorMessage = "你不配拥有代理文件";
            }
        }

        /// <summary>
        /// 在客户端发现服务时调用，
        /// 决定该客户端能不能调用某个服务（或服务函数）
        /// </summary>
        /// <param name="proxyToken"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public override MethodItem[] GetRegisteredMethodItems(string proxyToken, ICaller caller)
        {
            if (proxyToken.StartsWith("RPC"))
            {
                string ser = proxyToken.Replace("RPC", string.Empty);

                //全部服务
                MethodItem[] methodItems = this.MethodStore.GetAllMethodItem();

                return methodItems.Where(m => m.ServerName.Contains(ser)).ToArray();
            }
            else
            {
                return null;
            }
        }
    }

    

   
}