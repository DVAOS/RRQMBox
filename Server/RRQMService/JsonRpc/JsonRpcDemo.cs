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
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.JsonRpc
{
   public static class JsonRpcDemo
    {
        public static void Start()
        {
            RPCService rpcService = new RPCService();
            rpcService.ShareProxy(new RRQMSocket.IPHost(8848));

            rpcService.AddRPCParser("tcpJsonRpcParser ", CreateTcpJsonRpcParser());
            rpcService.AddRPCParser("httpJsonRpcParser ", CreateHTTPJsonRpcParser());

            rpcService.RegisterServer<Server>();//注册服务
            Console.WriteLine("RPC服务已启动");
        }

        static IRPCParser CreateTcpJsonRpcParser()
        {
            JsonRpcParser jsonRpcParser = new JsonRpcParser();

            JsonRpcParserConfig config = new JsonRpcParserConfig();
            config.ProtocolType = JsonRpcProtocolType.Tcp;//使用Tcp协议，调用时，有且仅有调用消息末尾追加“\r\n”。否则会调用失败。
            config.ListenIPHosts = new RRQMSocket.IPHost[] { new RRQMSocket.IPHost(7705) };
            config.ProxyToken = "RPC";//生成代理时需要验证

            jsonRpcParser.Setup(config);
            jsonRpcParser.Start();
            Console.WriteLine("TCP协议的JsonRpc已启动");
            return jsonRpcParser;
        }

        static IRPCParser CreateHTTPJsonRpcParser()
        {
            JsonRpcParser jsonRpcParser = new JsonRpcParser();

            JsonRpcParserConfig config = new JsonRpcParserConfig();
            config.ProtocolType = JsonRpcProtocolType.Http;//使用Tcp协议，调用时，有且仅有调用消息末尾追加“\r\n”。否则会调用失败。
            config.ListenIPHosts = new RRQMSocket.IPHost[] { new RRQMSocket.IPHost(7706) };
            config.ProxyToken = "RPC";

            jsonRpcParser.Setup(config);
            jsonRpcParser.Start();
            Console.WriteLine("HTTP协议的JsonRpc已启动");
            return jsonRpcParser;
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
