using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using System;

namespace GetProxyFromTcpParser
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
                client.Connect();
                RpcProxyInfo proxyInfo = client.GetProxyInfo();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
