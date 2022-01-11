using RRQMCore.Run;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.JsonRpc
{
    public static class JsonRpcDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.测试Tcp_JsonRpc");
            Console.WriteLine("2.测试Http_JsonRpc");
            Console.WriteLine("3.性能测试");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestTcpJsonRpcParser();
                        break;
                    }
                case "2":
                    {
                        TestHttpJsonRpcParser();
                        break;
                    }
                case "3":
                    {
                        Test_PerformanceRpcServer();
                        break;
                    }
                default:
                    break;
            }
        }
        private static void Test_PerformanceRpcServer()
        {
            JsonRpcClient jsonRpcClient = new JsonRpcClient();

            var config = new JsonRpcClientConfig();

            config.ProtocolType = JsonRpcProtocolType.Tcp;
            config.RemoteIPHost = new RRQMSocket.IPHost("127.0.0.1:7705");

            //config.ProtocolType = JsonRpcProtocolType.Http;
            //config.RemoteIPHost = new RRQMSocket.IPHost("127.0.0.1:7706");

            jsonRpcClient.Setup(config);

            jsonRpcClient.Connect();
            Console.WriteLine("连接成功");

            int count = 0;

            Task.Run(() =>
            {
                while (true)
                {
                    int p = count++;
                    int result = jsonRpcClient.Invoke<int>("Performance", InvokeOption.WaitInvoke, p);
                    if (result != p + 1)
                    {
                        Console.WriteLine("调用不一致。");
                    }
                }
            });
            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                Console.WriteLine($"调用{count}次");
                count = 0;
            });
            loopAction.RunAsync();
            Console.ReadKey();
        }

        static void TestProxy()
        {
            JsonRpcClient jsonRpcClient = new JsonRpcClient();

            var config = new JsonRpcClientConfig();
            config.ProtocolType = JsonRpcProtocolType.Http;
            config.RemoteIPHost = new RRQMSocket.IPHost("127.0.0.1:7706");

            jsonRpcClient.Setup(config);

            jsonRpcClient.Connect();
            Console.WriteLine("连接成功");

            RRQMProxy.Server server = new RRQMProxy.Server(jsonRpcClient);//载入连接器
            while (true)
            {
                string result = server.TestJsonRpc(Console.ReadLine());
                Console.WriteLine($"返回结果:{result}");
            }
        }

        static void TestHttpJsonRpcParser()
        {
            JsonRpcClient jsonRpcClient = new JsonRpcClient();

            var config = new JsonRpcClientConfig();
            config.ProtocolType = JsonRpcProtocolType.Http;
            config.RemoteIPHost = new RRQMSocket.IPHost("127.0.0.1:7706");

            jsonRpcClient.Setup(config);

            jsonRpcClient.Connect();
            Console.WriteLine("连接成功");

            while (true)
            {
                string result = jsonRpcClient.Invoke<string>("TestJsonRpc", InvokeOption.WaitInvoke, Console.ReadLine());
                Console.WriteLine($"返回结果:{result}");
            }
        }

        static void TestTcpJsonRpcParser()
        {
            JsonRpcClient jsonRpcClient = new JsonRpcClient();

            var config = new JsonRpcClientConfig();
            config.ProtocolType = JsonRpcProtocolType.Tcp;
            config.RemoteIPHost = new RRQMSocket.IPHost("127.0.0.1:7705");

            jsonRpcClient.Setup(config);

            jsonRpcClient.Connect();
            Console.WriteLine("连接成功");

            while (true)
            {
                string result = jsonRpcClient.Invoke<string>("TestJsonRpc", InvokeOption.WaitInvoke, Console.ReadLine());
                Console.WriteLine($"返回结果:{result}");
            }
        }
    }
}
