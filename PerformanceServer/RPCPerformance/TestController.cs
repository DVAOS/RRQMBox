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
using EventNext;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPCPerformance
{
    public interface ITestTaskController
    {
        Task<int> Sum(int a, int b);

        Task<byte[]> GetBytes(int length);

        Task<string> GetBigString();
    }

    /// <summary>
    /// BeetleXRPC仅支持Task返回。
    /// NewLifeRPC仅支持常规参数返回。
    /// 只有RRQM全兼容。哎！！还得写两个服务类。
    /// </summary>
    [Service(typeof(ITestTaskController))]
    public class TestTaskController : ITestTaskController
    {
        [RRQMRPC]
        public Task<int> Sum(int a, int b)
        {
            return Task.FromResult(a + b);
        } 

        [RRQMRPC]
        public Task<byte[]> GetBytes(int length)
        {
            return Task.FromResult(new byte[length]) ;
        }

        [RRQMRPC]
        public Task<string> GetBigString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                stringBuilder.Append("RRQM");
            }
            return Task.FromResult(stringBuilder.ToString()) ;
        }
    }


    public class TestController : ServerProvider
    {
        [RRQMRPC]
        public int Sum(int a, int b) => a + b;

        [RRQMRPC]
        public byte[] GetBytes(int length)
        {
            return new byte[length];
        }

        [RRQMRPC]
        public string GetBigString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                stringBuilder.Append("RRQM");
            }
            return stringBuilder.ToString();
        }
    }

}
