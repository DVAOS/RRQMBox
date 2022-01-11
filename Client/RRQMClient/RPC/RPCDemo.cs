using RRQMCore.Run;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Threading;
using System.Threading.Tasks;
using RRQMProxy;

namespace RRQMClient.RPC
{
    public static class RPCDemo
    {
        public static void Start()
        {
            Console.WriteLine("选择测试");
            Console.WriteLine("0.性能测试");
            Console.WriteLine("1.调用MyRpcServer所有方法");
            Console.WriteLine("2.测试调用实例类型");
            Console.WriteLine("3.测试可取消任务的执行");
            Console.WriteLine("4.测试并发性能");
            Console.WriteLine("5.测试ID调用");

            switch (Console.ReadLine())
            {
                case "0":
                    {
                        Test_PerformanceRpcServer();
                        break;
                    }
                case "1":
                    {
                        Test_InvokeMyRpcServer();
                        break;
                    }
                case "2":
                    {
                        Test_InstanceRpcServer();
                        break;
                    }
                case "3":
                    {
                        Test_ElapsedTimeRpcServer();
                        break;
                    }
                case "4":
                    {
                        Test_ConPerformance();
                        break;
                    }
                case "5":
                    {
                        Test_IDInvoke();
                        break;
                    }
                default:
                    break;
            }
        }
        private static TcpRpcClient GetTcpRpcClient()
        {
            TcpRpcClient client = new TcpRpcClient();

            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");
            config.SeparateThreadSend = false;
            client.Setup(config);

            try
            {
                client.Connect("123RPC");
                client.DiscoveryService("RPC");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return client;
        }

        private static void Test_IDInvoke()
        {
            TcpRpcClient client1 = GetTcpRpcClient();
            TcpRpcClient client2 = GetTcpRpcClient();

            RPCService service = new RPCService();
            service.AddRPCParser("client1", client1);
            service.RegisterServer<CallbackServer>();

            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
            {
                for (int j = 0; j < 100000; j++)
                {
                    int result = client2.Invoke<int>(client1.ID, "Performance", InvokeOption.WaitInvoke, j);
                    if (result != j + 1)
                    {
                        Console.WriteLine("调用结果不一致。");
                    }
                    if (j % 1000 == 0)
                    {
                        Console.WriteLine($"已调用{j}次");
                    }
                }
            });

            Console.WriteLine($"测试结束。用时：{timeSpan}");
            Console.ReadKey();
        }

        private static void Test_ConPerformance()
        {
            TcpRpcClient tcpRpcClient = GetTcpRpcClient();

            PerformanceRpcServer rpcServer = new PerformanceRpcServer(tcpRpcClient);
            Console.WriteLine("请输入待测试并发数量");
            int clientCount = int.Parse(Console.ReadLine());
            ThreadPool.SetMinThreads(clientCount + 10, clientCount + 10);
            /*
             并发性能测试内容为，同时多个异步调用同一个方法，
             然后检测其返回值。
             */
            for (int i = 0; i < clientCount; i++)
            {
                Task.Run(() =>
                {
                    TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                    {
                        for (int j = 0; j < 100000000; j++)
                        {
                            int result = tcpRpcClient.Invoke<int>("ConPerformance", InvokeOption.WaitInvoke, j);
                            //if (result!=j+1)
                            //{
                            //    Console.WriteLine("调用结果不一致");
                            //}
                            if (j % 100000 == 0)
                            {
                                Console.WriteLine($"已调用{j}次");
                            }
                        }
                    });

                    Console.WriteLine($"测试结束。用时：{timeSpan}");
                });
            }

            Console.ReadKey();
        }

        private static void Test_ElapsedTimeRpcServer()
        {
            TcpRpcClient tcpRpcClient = GetTcpRpcClient();

            ElapsedTimeRpcServer rpcServer = new ElapsedTimeRpcServer(tcpRpcClient);

            int tick = 100;

            Console.WriteLine($"已调用{nameof(rpcServer.DelayInvoke)},延迟{tick * 100}毫秒后完成");
            Console.WriteLine("按任意键取消任务。");

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                InvokeOption invokeOption = new InvokeOption();
                invokeOption.Timeout = 1000 * 60;
                invokeOption.FeedbackType = FeedbackType.WaitInvoke;
                invokeOption.SerializationType = RRQMCore.Serialization.SerializationType.RRQMBinary;

                invokeOption.CancellationToken = tokenSource.Token;

                //实际上当为false时，并不是返回的值，而是default值
                bool status = rpcServer.DelayInvoke(tick, invokeOption);
                Console.WriteLine(status);
            });

            Console.ReadKey();
            tokenSource.Cancel();
        }

        private static void Test_InstanceRpcServer()
        {
            Console.WriteLine("接下来，将由两个客户端，分别调用5次，并输出结果");
            Console.WriteLine("选择调用模式");

            Console.WriteLine("1.GlobalInstance");
            Console.WriteLine("2.CustomInstance");
            Console.WriteLine("3.NewInstance");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TcpRpcClient tcpRpcClient = GetTcpRpcClient();
                            InstanceRpcServer rpcServer = new InstanceRpcServer(tcpRpcClient);

                            InvokeOption invokeOption = new InvokeOption()
                            {
                                FeedbackType = FeedbackType.WaitInvoke,
                                InvokeType = RRQMSocket.RPC.InvokeType.GlobalInstance,
                                SerializationType = RRQMCore.Serialization.SerializationType.RRQMBinary
                            };

                            for (int j = 0; j < 5; j++)
                            {
                                Console.WriteLine($"由ID={tcpRpcClient.ID}的客户端调用{nameof(rpcServer.Increment)}调用成功，结果={rpcServer.Increment(invokeOption)}");
                            }
                            tcpRpcClient.Dispose();
                            Console.WriteLine();
                        }
                        break;
                    }
                case "2":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TcpRpcClient tcpRpcClient = GetTcpRpcClient();
                            InstanceRpcServer rpcServer = new InstanceRpcServer(tcpRpcClient);

                            InvokeOption invokeOption = new InvokeOption()
                            {
                                FeedbackType = FeedbackType.WaitInvoke,
                                InvokeType = RRQMSocket.RPC.InvokeType.CustomInstance,
                                SerializationType = RRQMCore.Serialization.SerializationType.RRQMBinary
                            };

                            for (int j = 0; j < 5; j++)
                            {
                                Console.WriteLine($"由ID={tcpRpcClient.ID}的客户端调用{nameof(rpcServer.Increment)}调用成功，结果={rpcServer.Increment(invokeOption)}");
                            }
                            Console.WriteLine();
                        }
                        break;
                    }
                case "3":
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TcpRpcClient tcpRpcClient = GetTcpRpcClient();
                            InstanceRpcServer rpcServer = new InstanceRpcServer(tcpRpcClient);

                            InvokeOption invokeOption = new InvokeOption()
                            {
                                FeedbackType = FeedbackType.WaitInvoke,
                                InvokeType = RRQMSocket.RPC.InvokeType.NewInstance,
                                SerializationType = RRQMCore.Serialization.SerializationType.RRQMBinary
                            };

                            for (int j = 0; j < 5; j++)
                            {
                                Console.WriteLine($"由ID={tcpRpcClient.ID}的客户端调用{nameof(rpcServer.Increment)}调用成功，结果={rpcServer.Increment(invokeOption)}");
                            }
                            Console.WriteLine();
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        private static async void Test_InvokeMyRpcServer()
        {
            TcpRpcClient client = GetTcpRpcClient();

            //3.实例化服务代理，传入IRpcClient

            RpcServer rpcServer = new RpcServer(client);

            //4.通过MyRpcServer代理类直接调用

            Console.WriteLine($"{nameof(rpcServer.TestOne)}调用成功，结果={rpcServer.TestOne(10)}");
            Console.WriteLine($"{nameof(rpcServer.TestOneAsync)}调用成功，结果={await rpcServer.TestOneAsync(10)}");

            rpcServer.TestOut(out int out1);
            Console.WriteLine($"{nameof(rpcServer.TestOut)}调用成功，结果={out1}");

            int ref1 = 10;
            rpcServer.TestRef(ref ref1);
            Console.WriteLine($"{nameof(rpcServer.TestRef)}调用成功，结果={ref1}");

            Console.WriteLine($"{nameof(rpcServer.TestOne_Name)}调用成功，结果={rpcServer.TestOne_Name(10, "RRQM")}");
            Console.WriteLine($"{nameof(rpcServer.TestOne_NameAsync)}调用成功，结果={await rpcServer.TestOne_NameAsync(10, "RRQM")}");
        }

        private static void Test_PerformanceRpcServer()
        {
            int count = 0;

            Console.WriteLine("请输入待测试客户端数量");
            int clientCount = int.Parse(Console.ReadLine());
            ThreadPool.SetMinThreads(clientCount + 10, clientCount + 10);

            for (int i = 0; i < clientCount; i++)
            {
                TcpRpcClient client = GetTcpRpcClient();

                PerformanceRpcServer rpcServer = new PerformanceRpcServer(client);

                Task.Run(() =>
                {
                    while (true)
                    {
                        rpcServer.Performance();
                        count++;
                    }
                });
            }

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                Console.WriteLine($"调用{count}次");
                count = 0;
            });
            loopAction.RunAsync();
        }
    }

    public class CallbackServer : RRQMSocket.RPC.ServerProvider
    {
        [RRQMRPCCallBack()]
        public int Performance(int i)
        {
            return ++i;
        }
    }
}
