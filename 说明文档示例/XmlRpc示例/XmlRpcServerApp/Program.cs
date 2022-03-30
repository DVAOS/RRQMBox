using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.Http.Plugins;
using RRQMSocket.RPC;
using RRQMSocket.RPC.XmlRpc;
using System;

namespace XmlRpcServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RpcService rpcService = new RpcService();

            //添加解析器，解析器根据传输协议，序列化方式的不同，调用RPC服务
            rpcService.AddRpcParser("xmlRpcParser ", CreateXmlRpcRpcParser());

            //注册当前程序集的所有服务
            rpcService.RegisterAllServer();

            //分享代理，代理文件可通过RRQMTool远程获取。
            rpcService.ShareProxy(new IPHost(8848));

            //或者直接本地导出代理文件。
            //RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.RRQMRPC, "RPC");
            //string codeString = CodeGenerator.ConvertToCode("RRQMProxy", proxyInfo.Codes);

            Console.WriteLine("服务器已启动");
            Console.ReadKey();

        }

        static IRpcParser CreateXmlRpcRpcParser()
        {
            HttpService service = new HttpService();

            service.Setup(new RRQMConfig().UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(7706) }))
                .Start();

            service.AddPlugin<MyPlugin>();

            return service.AddPlugin<XmlRpcParserPlugin>()
                 .SetProxyToken("RPC")
                 .SetXmlRpcUrl("/xmlRpc");
        }
    }

    public class MyPlugin : HttpPluginBase
    {
        protected override void OnPost(ITcpClientBase client, HttpContextEventArgs e)
        {
            string s = e.Request.GetBody();
            base.OnPost(client, e);
        }
    }

    public class Server : ServerProvider
    {
        [XmlRpc]
        public int Sum(int a, int b)
        {
            return a + b;
        }

        [XmlRpc]
        public int TestClass(MyClass myClass)
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
