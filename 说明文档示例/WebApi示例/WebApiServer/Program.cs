using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.RPC;
using RRQMSocket.RPC.WebApi;
using System;

namespace WebApiServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RpcService rpcService = new RpcService();
            rpcService.AddRpcParser("webApiParser", CreateWebApiParser());
            rpcService.RegisterServer<ServerController>();//注册服务

            Console.WriteLine("以下连接用于测试webApi");
            Console.WriteLine($"使用：http://127.0.0.1:7789/Server/Sum?a=10&b=20");

            //生成的WebApi的本地代理文件。
            //RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.WebApi, "RPC");
            //string code = CodeGenerator.ConvertToCode("RRQM", proxyInfo.Codes);

            rpcService.ShareProxy(new IPHost(8848));//分享远程代理

           

            Console.ReadKey();
        }

        

        private static IRpcParser CreateWebApiParser()
        {
            HttpService service = new HttpService();

            service.Setup(new RRQMConfig()
                .UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(7789) }))
                .Start();

            return service.AddPlugin<WebApiParserPlugin>()
                 .SetProxyToken("RPC")
                 .SetApiDataConverter(new JsonDataConverter());
        }
    }

    [Route("/[controller]/[action]")]
    public class ServerController : ServerProvider
    {
        [HttpGet]
        public int Sum(int a, int b)
        {
            return a + b;
        }

        [HttpPost]
        public int TestPost(MyClass myClass)
        {
            return myClass.A + myClass.B;
        }
    }

    public class MyClass
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}
