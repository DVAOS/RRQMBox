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
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPCServiceDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //实例化RPCService
            RPCService rpcService = new RPCService();

            //添加解析器，解析器根据传输协议，序列化方式的不同，调用RPC服务
            rpcService.AddRPCParser("qosTcpRpcParser", CreateRRQMTcpParser(7794));

            //注册服务
            rpcService.RegisterAllServer();

            //注册当前程序集的所有服务
            //rpcService.RegisterAllServer();
            Console.WriteLine("RPC服务已启动");

            Console.WriteLine("按任意键显示代理代码");
            Console.ReadKey();

            if (rpcService.TryGetRPCParser("qosTcpRpcParser", out IRPCParser parser))
            {
                if (parser is TcpRpcParser tcpRpcParser)
                {
                    CellCode[] cellCodes = tcpRpcParser.Codes;
                    foreach (var item in cellCodes)
                    {
                        Console.WriteLine(item.Code);

                        File.WriteAllText(item.Name + ".cs", item.Code);
                    }

                    tcpRpcParser.CompilerProxy();
                }
            }

            Console.ReadKey();
        }

        private static IRPCParser CreateRRQMTcpParser(int port)
        {
            QosTcpRpcParser qosTcpRpcParser = new QosTcpRpcParser();

            //声明配置
            var config = new TcpRpcParserConfig();

            //继承TcpService配置
            config.ListenIPHosts = new IPHost[] { new IPHost(port) };//同时监听两个地址
            //继承TokenService配置
            config.VerifyToken = "123RPC";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //继承ProcotolService配置
            config.CanResetID = true;//是否重新设置ID

            //继承TcpRpcParser配置，以实现RPC交互
            config.NameSpace = "RRQMTest";
            config.RPCVersion = new Version(1, 0, 0, 0);//定义此次发布的RPC版本。
            config.ProxyToken = "RPC";//代理令箭，当客户端获取代理文件,或服务时需验证令箭

            //载入配置
            qosTcpRpcParser.Setup(config);

            //启动服务
            qosTcpRpcParser.Start();

            Console.WriteLine($"TCP解析器添加完成，端口号：{port}，VerifyToken={qosTcpRpcParser.VerifyToken}，ProxyToken={qosTcpRpcParser.ProxyToken}");
            return qosTcpRpcParser;
        }
    }

    internal class QosTcpRpcParser : TcpRpcParser
    {
        /// <summary>
        /// 在获取代理时筛选，
        /// 仅筛选代理代码功能，并不能决定服务能不能调用
        /// </summary>
        /// <param name="proxyToken"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public override RpcProxyInfo GetProxyInfo(string proxyToken, ICaller caller)
        {
            RpcProxyInfo rpcProxy = base.GetProxyInfo(proxyToken, caller);
            if (proxyToken.StartsWith("RPC"))
            {
                RpcProxyInfo proxyInfo = new RpcProxyInfo()
                {
                    AssemblyName = this.NameSpace + ".dll",
                    Status = 1,//1表示成功，2表示失败
                    Version = this.RPCVersion.ToString()
                };

                string ser = proxyToken.Replace("RPC", string.Empty);

                proxyInfo.Codes = new List<CellCode>(this.Codes.Where(a => a.CodeType == CodeType.ClassArgs || a.Name.Contains(ser)));

                return proxyInfo;
            }
            else
            {
                return new RpcProxyInfo() { Status = 2, Message = "你不配拥有代理文件" };//1表示成功，2表示失败
            }
        }

        /// <summary>
        /// 在客户端发现服务时调用，
        /// 决定该客户端能不能调用某个服务（或服务函数）
        /// </summary>
        /// <param name="proxyToken"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public override List<MethodItem> GetRegisteredMethodItems(string proxyToken, ICaller caller)
        {
            if (proxyToken.StartsWith("RPC"))
            {
                string ser = proxyToken.Replace("RPC", string.Empty);

                //全部服务
                List<MethodItem> methodItems = this.MethodStore.GetAllMethodItem();

                return new List<MethodItem>(methodItems.Where(m => m.ServerName.Contains(ser)));
            }
            else
            {
                return new List<MethodItem>();
            }
        }
    }

    public class MyRpcServer : ServerProvider
    {
        [Description("测试同步调用")]
        [RRQMRPC]
        public string TestOne(int id)//同步服务
        {
            return $"若汝棋茗,id={id}";
        }

        [Description("测试TestTwo")]
        [RRQMRPC]
        public string TestTwo(int id)//同步服务
        {
            return $"若汝棋茗,id={id}";
        }

        [Description("测试重载调用")]
        [RRQMRPC("TestOne_Name")]//在重载服务时需要重新设定服务唯一键
        public string TestOne(int id, string name)
        {
            return $"若汝棋茗,Name={name},id={id}";
        }

        [Description("测试Out")]
        [RRQMRPC]
        public void TestOut(out int id)
        {
            id = 10;
        }

        [Description("测试Ref")]
        [RRQMRPC]
        public void TestRef(ref int id)
        {
            id += 1;
        }

        [Description("测试异步")]
        [RRQMRPC]
        public Task<string> AsyncTestOne(int id)//异步服务,尽量不要用Async结尾，不然生成的异步代码方法将出现两个Async
        {
            return Task.Run(() =>
            {
                return $"若汝棋茗,id={id}";
            });
        }
    }

    public class PerformanceRpcServer : ServerProvider
    {
        [Description("测试性能")]
        [RRQMRPC]
        public string Performance()//同步服务
        {
            return "若汝棋茗";
        }

        [Description("测试并发性能")]
        [RRQMRPC]
        public Task<int> ConPerformance(int num)
        {
            return Task.FromResult(++num);
        }
    }

    public class ElapsedTimeRpcServer : ServerProvider
    {
        [Description("测试可取消的调用")]
        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public bool DelayInvoke(IServerCallContext serverCallContext, int tick)//同步服务
        {
            for (int i = 0; i < tick; i++)
            {
                Thread.Sleep(100);
                if (serverCallContext.TokenSource.IsCancellationRequested)
                {
                    Console.WriteLine("客户端已经取消该任务！");
                    return false;//实际上在取消时，客户端得不到该值
                }
            }
            return true;
        }
    }

    public class InstanceRpcServer : ServerProvider
    {
        public int Count { get; set; }

        [Description("测试调用实例")]
        [RRQMRPC]
        public int Increment()//同步服务
        {
            return ++Count;
        }
    }

    public class GetCallerRpcServer : ServerProvider
    {
        [Description("测试调用上下文")]
        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public string GetCallerID(IServerCallContext callContext)
        {
            if (callContext.Caller is RpcSocketClient socketClient)
            {
                return socketClient.ID;
            }
            return null;
        }
    }
}