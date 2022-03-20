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
using RRQMSocket.Http;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using System;

namespace RRQMService.JsonRpc
{
    public static class JsonRpcDemo
    {
        public static void Start()
        {
            RpcService rpcService = new RpcService();
            rpcService.ShareProxy(new IPHost(8848));

            rpcService.AddRpcParser("tcpJsonRpcParser ", CreateTcpJsonRpcParser());
            rpcService.AddRpcParser("httpJsonRpcParser ", CreateHTTPJsonRpcParser());

            rpcService.RegisterServer<Server>();//注册服务
            Console.WriteLine("RPC服务已启动");
        }

        static IRpcParser CreateTcpJsonRpcParser()
        {
            TcpService service = new TcpService();
            service.Connecting += (client, e) =>
            {
                e.DataHandlingAdapter = new TerminatorPackageAdapter(client.MaxPackageSize,"\r\n");
            };

            service.Setup(new RRQMConfig()
                .UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(7705) }))
                .Start();

            return service.AddPlugin<JsonRpcParserPlugin>()
                 .SetProxyToken("RPC");
        }

        static IRpcParser CreateHTTPJsonRpcParser()
        {
            HttpService service = new HttpService();

            service.Setup(new RRQMConfig().UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(7706) }))
                .Start();

            return service.AddPlugin<JsonRpcParserPlugin>()
                 .SetProxyToken("RPC")
                 .SetJsonRpcUrl("/jsonRpc");
        }
    }

    public class Server : ServerProvider
    {
        [JsonRpc]
        public string TestJsonRpc(string str)
        {
            return "RRQM" + str;
        }

        [JsonRpc]
        public int Performance(int num)
        {
            return ++num;
        }
    }
}
