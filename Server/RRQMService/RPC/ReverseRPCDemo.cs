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
            RPCService service = new RPCService();

            TcpRpcParser tcpRPCParser = new TcpRpcParser();
            service.AddRPCParser("tcpRPCParser", tcpRPCParser);
            service.RegisterServer<PerformanceServer>();

            tcpRPCParser.Connected += (client, e) =>//在客户端完成连接时，就反向调用，同时正向RPC由客户端完成。
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
            //创建配置
            var config = new TcpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(7789) };//监听一个IP地址
            config.VerifyToken = "123RPC";//令箭值
            config.ProxyToken = "RPC";//默认服务代理令箭
            //载入配置
            tcpRPCParser.Setup(config);

            //启动服务
            tcpRPCParser.Start();
            Console.WriteLine("服务已启动");
            Console.ReadKey();
        }

        static void TestPerformance()
        {
            TcpRpcParser tcpRPCParser = new TcpRpcParser();
            tcpRPCParser.Connected += (client, e) =>
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
            //创建配置
            var config = new TcpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(7789) };//监听一个IP地址
            config.VerifyToken = "123RPC";//令箭值
            config.ProxyToken = "RPC";//默认服务代理令箭
            //载入配置
            tcpRPCParser.Setup(config);

            //启动服务
            tcpRPCParser.Start();
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
