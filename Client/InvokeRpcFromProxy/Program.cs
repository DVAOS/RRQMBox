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
using RRQMCore.Run;
using RRQMRPC.RRQMTest;
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvokeRpcFromProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择测试");
            Console.WriteLine("0.性能测试");
            Console.WriteLine("1.调用MyRpcServer所有方法");
            Console.WriteLine("2.测试调用实例类型");
            Console.WriteLine("3.测试可取消任务的执行");

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
                default:
                    break;
            }
            Console.ReadKey();
        }

        private static TcpRpcClient GetTcpRpcClient()
        {
            TcpRpcClient client = new TcpRpcClient();

            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7794");
            config.ProxyToken = "RPC";

            client.Setup(config);

            try
            {
                client.Connect("123RPC");
                client.DiscoveryService();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return client;
        }

        private static void Test_ElapsedTimeRpcServer()
        {
            TcpRpcClient tcpRpcClient = GetTcpRpcClient();

            ElapsedTimeRpcServer rpcServer = new ElapsedTimeRpcServer(tcpRpcClient);

            int tick = 100;

            Console.WriteLine($"已调用{nameof(rpcServer.DelayInvoke)},延迟{tick * 100}毫秒后完成");
            Console.WriteLine("按任意键取消任务。");

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Task.Run(()=> 
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

            MyRpcServer rpcServer = new MyRpcServer(client);

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

    public class RpcHandler
    {
        private TcpRpcClient client;

        private IMyRpcServer myRpcServer;

        public TcpRpcClient Client => client;

        public string IPHost { get => "127.0.0.1:7794"; }


        /// <summary>
        /// 检验client是否在线
        /// 不在线，则重新初始化。
        /// </summary>
        /// <returns></returns>
        public bool CheckOnline()
        {
            if (string.IsNullOrEmpty(IPHost))
            {
                return false;
            }
            try
            {
                if (client == null)
                {
                    client = new TcpRpcClient();
                    myRpcServer = new MyRpcServer(client);

                    TcpRpcClientConfig config = new TcpRpcClientConfig();
                    config.RemoteIPHost = new IPHost(IPHost);
                    config.ProxyToken = "FileServerRPC";
                    client.Setup(config);

                    try
                    {
                        client.Connect("FileServer");
                        client.DiscoveryService();
                    }
                    catch
                    {
                        return false;
                    }
                }
                if (!client.Online)
                {
                    client.Dispose();
                    client = null;
                    myRpcServer = null;
                    return CheckOnline();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 此处是示例调用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string TestOne(int id)
        {
            try
            {
                if (CheckOnline())
                {
                    return myRpcServer.TestOne(id);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);//模拟日志记录
                return null;
            }

        }
    }
}
