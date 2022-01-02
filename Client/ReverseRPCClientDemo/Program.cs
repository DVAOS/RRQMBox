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

namespace ReverseRPCClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RPCService service = new RPCService();
            service.ShareProxy(new IPHost(8848));

            TcpRpcClient client = new TcpRpcClient();

            service.AddRPCParser("client", client);
            service.RegisterServer<CallbackServer>();
            var config = new TcpRpcClientConfig();
            config.ProxyToken = "RPC";
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");
            client.Setup(config);

            Console.ReadKey();
           
           
            client.Connect("123RPC");
            client.DiscoveryService("RPC");
            Console.WriteLine("成功连接");
            Console.ReadKey();
        }
    }
    public class CallbackServer : RRQMSocket.RPC.ServerProvider
    {
        [RRQMRPCCallBack()]
        public int ConPerformance(int age)
        {
            return ++age;
        }
    }
}
