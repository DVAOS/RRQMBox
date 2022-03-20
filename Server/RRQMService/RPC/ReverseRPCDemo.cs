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
using System.Threading.Tasks;

namespace RRQMService.RPC
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

            TcpRpcParser tcpRPCParser = new TcpRpcParser();
            service.AddRpcParser("tcpRPCParser", tcpRPCParser);
            service.RegisterServer<PerformanceServer>();

            tcpRPCParser.Handshaked += (client, e) =>//在客户端握手完成连接时，就反向调用，同时正向RPC由客户端完成。
            {
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
            };

            tcpRPCParser.Setup(new RRQMConfig()
                .SetListenIPHosts(new IPHost[] { new IPHost(7789) })
                .SetVerifyToken("123RPC")
                .SetProxyToken("RPC"))
                .Start();

            Console.WriteLine("服务已启动");
            Console.ReadKey();
        }

        static void TestPerformance()
        {
            TcpRpcParser tcpRpcParser = new TcpRpcParser();
            tcpRpcParser.Handshaked += (client, e) =>
            {
                Task.Run(() =>
                {
                    TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                    {
                        for (int i = 0; i < 100000; i++)
                        {
                            if (i % 1000 == 0)
                            {
                                Console.WriteLine(i);
                            }
                            int value = client.Invoke<int>("ConPerformance", InvokeOption.WaitInvoke, i);
                            if (value != i + 1)
                            {
                                Console.WriteLine("调用结果不一致");
                            }
                        }
                    });
                    Console.WriteLine($"测试完成，用时{timeSpan}");
                });
            };

            tcpRpcParser.Setup(new RRQMConfig()
                .SetListenIPHosts(new IPHost[] { new IPHost(7789) })
                .SetVerifyToken("123RPC")
                .SetProxyToken("RPC"))
                .Start();

            Console.WriteLine("服务已启动");
            Console.ReadKey();
        }
    }

    public class PerformanceServer : RRQMSocket.RPC.ServerProvider
    {
        [RRQMRPC(Async = false)]
        public int ConPerformance(int age)
        {
            return ++age;
        }
    }
}
