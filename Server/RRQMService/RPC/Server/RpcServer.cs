//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
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

    [RRQMProxy("MyArgs")]
    public class Args
    {

    }
}
