using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMService.RPC.Server
{
    public class RpcServer : ServerProvider
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
        [RRQMRPC(async: true)]//设置Task执行
        public string Performance()//同步服务
        {
            return "若汝棋茗";
        }

        [Description("测试并发性能")]
        [RRQMRPC(async: true)]
        public int ConPerformance(int num)
        {
            return ++num;
        }

        [Description("测试并发性能2")]
        [RRQMRPC(async: true)]
        public int ConPerformance2(int num)
        {
            return ++num;
        }
    }

    public class ElapsedTimeRpcServer : ServerProvider
    {
        [Description("测试可取消的调用")]
        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public bool DelayInvoke(ICallContext serverCallContext, int tick)//同步服务
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
        public string GetCallerID(ICallContext callContext)
        {
            if (callContext.Caller is RpcSocketClient socketClient)
            {
                return socketClient.ID;
            }
            return null;
        }
    }
}
