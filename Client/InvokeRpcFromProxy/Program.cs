using RRQMRPC.RRQMTest;
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using System;

namespace InvokeRpcFromProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpRpcClient client = new TcpRpcClient();

            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7794");
            config.VerifyToken = "123RPC";
            config.ProxyToken = "RPC";

            client.Setup(config);

            try
            {
                //1.先连接
                client.Connect();
                Console.WriteLine("连接成功");

                //2.然后发现服务
                MethodItem[] methodItems = client.DiscoveryService();
                Console.WriteLine("服务发现成功");

                foreach (var item in methodItems)
                {
                    Console.WriteLine($"服务{item.ServerName}中的‘{item.Method}’可以调用");
                }

                Console.WriteLine("按任意键调用TestOne");
                Console.ReadKey();

                //3.实例化服务代理，传入IRpcClient

                MyRpcServer myRpcServer = new MyRpcServer(client);

                //4.通过MyRpcServer代理类直接调用
                string returnString = myRpcServer.TestOne(10);

                Console.WriteLine($"调用成功，结果={returnString}");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
