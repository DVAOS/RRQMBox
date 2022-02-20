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
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCPerformance
{
    public static class RRQMRPCTCP
    {
        public static void Start()
        {
            RPCService rpcService = new RPCService();

            TcpRpcParser tcpRPCParser = new TcpRpcParser();
            //声明配置
            var config = new TcpRpcParserConfig();

            //继承TcpService配置
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789") };

            //继承TokenService配置
            config.VerifyToken = "123RPC";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //继承TcpRpcParser配置，以实现RPC交互
            config.ProxyToken = "RPC";//代理令箭，当客户端获取代理文件,或服务时需验证令箭
            tcpRPCParser.Setup(config);
            tcpRPCParser.Start();

            rpcService.AddRPCParser("TcpParser", tcpRPCParser);
            Console.WriteLine("TCP解析器添加完成");

            rpcService.RegisterServer<TestController>();
            Console.WriteLine("RPC服务已启动");
        }
    }
}
