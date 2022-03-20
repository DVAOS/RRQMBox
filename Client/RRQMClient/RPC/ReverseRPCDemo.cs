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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.RPC
{
   public static class ReverseRPCDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.测试反向RPC性能");
            Console.WriteLine("2.测试RPC和反向RPC同时调用性能");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestPerformance();
                        break;
                    }
                case "2":
                    {
                        TestSleepPerformance();
                        break;
                    }
                default:
                    break;
            }
        }
        static void TestSleepPerformance()
        {
            RpcService service = new RpcService();
            service.ShareProxy(new IPHost(8848));//分享反向代理RPC代理文件，不使用代理时，可以不用。

            TcpRpcClient client = new TcpRpcClient();

            service.AddRpcParser("client", client);//添加解析
            service.RegisterServer<ReverseCallbackServer>();//注册服务

            client.Setup(new RRQMConfig()
                .SetRemoteIPHost(new IPHost("127.0.0.1:7789"))
                .SetProxyToken("RPC"));

            client.Connect("123RPC");
            client.DiscoveryService("RPC");
            Console.WriteLine("成功连接");

            Task.Run(() =>
            {
                int i = 0;
                while (true)
                {
                    if (i % 100 == 0)
                    {
                        Console.WriteLine(i);
                    }
                    int value = client.Invoke<int>("ConPerformance", InvokeOption.WaitInvoke, i++);
                    if (value != i)
                    {
                        Console.WriteLine("调用结果不一致");
                    }
                    //await Task.Delay(10);
                }
            });
        }

        static void TestPerformance()
        {
            RpcService service = new RpcService();
            //service.ShareProxy(new IPHost(8848));//分享反向代理RPC代理文件，需要时调用

            TcpRpcClient client = new TcpRpcClient();

            service.AddRpcParser("client", client);//添加解析
            service.RegisterServer<ReverseCallbackServer>();//注册服务
            client.Setup(new RRQMConfig()
                .SetRemoteIPHost(new IPHost("127.0.0.1:7789"))
                .SetProxyToken("RPC"));
            client.Connect("123RPC");
            client.DiscoveryService("RPC");
            Console.WriteLine("成功连接");
        }
    }
    public class ReverseCallbackServer : ServerProvider
    {
        [RRQMRPCCallBack(Async = true)]
        public int ConPerformance(int age)
        {
            return ++age;
        }
    }
}
