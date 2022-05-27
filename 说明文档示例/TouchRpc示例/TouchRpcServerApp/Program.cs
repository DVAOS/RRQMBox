using RRQMCore;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.TouchRpc;
using System;
using System.ComponentModel;
using System.Linq;
using RRQMSocket.Http;

namespace TouchRpcServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RpcStore rpcStore = new RpcStore();
            rpcStore.ProxyUrl = "/proxy";//代理url
            rpcStore.OnRequestProxy = (request) =>//此处做请求验证，保证代理文件安全。
            {
                if (request.TryGetQuery("token",out string value)&&value == "123")
                {
                    return true;
                }
                return false;
            };

            rpcStore.AddRpcParser("tcpTouchRpcParser", CreateTcpParser());
            rpcStore.AddRpcParser("httpTouchRpcParser", CreateHttpParser());
            rpcStore.AddRpcParser("udpTouchRpcParser", CreateUdpParser());

            //注册当前程序集的所有服务
            rpcStore.RegisterServer<MyRpcServer>();

            //分享代理，代理文件可通过RRQMTool，或者浏览器获取，远程获取。
            //http://127.0.0.1:8848/proxy?proxy=all&token=123
             rpcStore.ShareProxy(new IPHost(8848));

            //或者直接本地导出代理文件。
            ServerCellCode[] codes = rpcStore.GetProxyInfo(RpcStore.ProxyAttributeMap.Values.ToArray());
            string codeString = CodeGenerator.ConvertToCode("RRQMProxy", codes);
            Console.WriteLine(codeString);

            Console.WriteLine("服务器已启动");
            Console.ReadKey();
        }

        /// <summary>
        /// 解析器是实际的逻辑服务器，此处是<see cref="TcpTouchRpcService"/>。
        /// </summary>
        /// <returns></returns>
        static IRpcParser CreateTcpParser()
        {
          return  new RRQMConfig()//配置     
                 .SetListenIPHosts(new IPHost[] { new IPHost(7789) })
                 .SetMaxCount(10000)
                 .SetThreadCount(100)
                 .SetVerifyToken("TouchRpc")
                 .BuildWithTcpTouchRpcService();//此处build相当于new TcpTouchRpcService，然后Setup，然后Start。
        }

        /// <summary>
        /// 此处是<see cref="HttpTouchRpcService"/>。
        /// </summary>
        /// <returns></returns>
        static IRpcParser CreateHttpParser()
        {
            return new RRQMConfig()//配置     
                   .SetListenIPHosts(new IPHost[] { new IPHost(7790) })
                   .SetMaxCount(10000)
                   .SetThreadCount(100)
                   .SetVerifyToken("TouchRpc")
                   .BuildWithHttpTouchRpcService();//此处build相当于new HttpTouchRpcService，然后Setup，然后Start。
        }

        /// <summary>
        /// 此处是<see cref="UdpTouchRpc"/>。
        /// </summary>
        /// <returns></returns>
        static IRpcParser CreateUdpParser()
        {
            RRQMConfig config = new RRQMConfig();
           return config.SetBindIPHost(7791)
                .SetBufferLength(1024 * 10)
                .SetThreadCount(10)
                .BuildWithUdpTouchRpc();
        }
    }

    public class MyRpcServer : ServerProvider
    {
        [Description("登录")]
        [TouchRpc(MethodFlags = MethodFlags.IncludeCallContext)]//使用调用上才文
        public bool Login(ICallContext callContext,string account,string password)
        {
            if (callContext.Caller is TcpTouchRpcSocketClient)
            {
                Console.WriteLine("TcpTouchRpc请求");
            }
            else if (callContext.Caller is HttpTouchRpcSocketClient)
            {
                Console.WriteLine("HttpTouchRpc请求");
            }
            else if (callContext.Caller is UdpCaller)
            {
                Console.WriteLine("UdpTouchRpc请求");
            }

            if (account=="123"&&password=="abc")
            {
                return true;
            }

            return false;
        }
    }
}
