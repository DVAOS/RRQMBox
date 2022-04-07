using RRQMCore.XREF.Newtonsoft.Json.Linq;
using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using System;

namespace JsonRpcConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RpcService rpcService = new RpcService();

            //添加解析器，解析器根据传输协议，序列化方式的不同，调用RPC服务
            rpcService.AddRpcParser("tcpJsonRpcParser ", CreateTcpJsonRpcParser());
            rpcService.AddRpcParser("httpJsonRpcParser ", CreateHTTPJsonRpcParser());

            //注册当前程序集的所有服务
            rpcService.RegisterAllServer();

            //分享代理，代理文件可通过RRQMTool远程获取。
            //rpcService.ShareProxy(new IPHost(8848));

            //或者直接本地导出代理文件。
            //RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.RRQMRPC, "RPC");
            //string codeString = CodeGenerator.ConvertToCode("RRQMProxy", proxyInfo.Codes);

            JsonRpcClientInvokeByTcp();
            JsonRpcClientInvokeByHttp();

            Console.ReadKey();
        }

        static void JsonRpcClientInvokeByHttp()
        {
            JsonRpcClient jsonRpcClient = new JsonRpcClient(JRPT.Http);
            jsonRpcClient.Setup("http://127.0.0.1:7706/jsonrpc");
            jsonRpcClient.Connect();
            Console.WriteLine("连接成功");
            string result = jsonRpcClient.Invoke<string>("TestJsonRpc", InvokeOption.WaitInvoke, "RRQM");
            Console.WriteLine($"Http返回结果:{result}");

            JObject obj = new JObject();
            obj.Add("A","A");
            obj.Add("B",10);
            obj.Add("C",100.1);
            JObject newObj = jsonRpcClient.Invoke<JObject>("TestJObject", InvokeOption.WaitInvoke, obj);
            Console.WriteLine($"Http返回结果:{newObj}");
        }

        static void JsonRpcClientInvokeByTcp()
        {
            JsonRpcClient jsonRpcClient = new JsonRpcClient(JRPT.Tcp);
            jsonRpcClient.Setup("127.0.0.1:7705");
            jsonRpcClient.Connect();
            Console.WriteLine("连接成功");
            string result = jsonRpcClient.Invoke<string>("TestJsonRpc", InvokeOption.WaitInvoke, "RRQM");
            Console.WriteLine($"Tcp返回结果:{result}");

            JObject obj = new JObject();
            obj.Add("A", "A");
            obj.Add("B", 10);
            obj.Add("C", 100.1);
            JObject newObj = jsonRpcClient.Invoke<JObject>("TestJObject", InvokeOption.WaitInvoke, obj);
            Console.WriteLine($"Tcp返回结果:{newObj}");
        }

        static IRpcParser CreateTcpJsonRpcParser()
        {
            TcpService service = new TcpService();
            service.Connecting += (client, e) =>
            {
                e.DataHandlingAdapter = new TerminatorPackageAdapter(client.MaxPackageSize, "\r\n");
            };

            service.Setup(new RRQMConfig()
                .UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(7705) }))
                .Start();

            return service.AddPlugin<JsonRpcParserPlugin>()
                 .SetProxyToken("RPC");
        }

        static IRpcParser CreateHTTPJsonRpcParser()
        {
            HttpService service = new HttpService();

            service.Setup(new RRQMConfig().UsePlugin()
                .SetListenIPHosts(new IPHost[] { new IPHost(7706) }))
                .Start();

            return service.AddPlugin<JsonRpcParserPlugin>()
                 .SetProxyToken("RPC")
                 .SetJsonRpcUrl("/jsonRpc");
        }
    }

    public class Server : ServerProvider
    {
        [JsonRpc]
        public string TestJsonRpc(string str)
        {
            return "RRQM" + str;
        }

        [JsonRpc]
        public JObject TestJObject(JObject obj)
        {
            return obj;
        }
    }
}
