//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System.Threading.Tasks;

namespace RRQMRPC.RRQMTest
{
    public interface IMyOperation
    {
        RRQMRPC.RRQMTest.FileArgs SayHello(RRQMRPC.RRQMTest.FileArgs a, InvokeOption invokeOption = null);

        Task<RRQMRPC.RRQMTest.FileArgs> SayHelloAsync(RRQMRPC.RRQMTest.FileArgs a, InvokeOption invokeOption = null);
    }

    public class MyOperation : IMyOperation
    {
        public MyOperation(IRPCClient client)
        {
            this.Client = client;
        }

        public IRPCClient Client { get; private set; }

        public RRQMRPC.RRQMTest.FileArgs SayHello(RRQMRPC.RRQMTest.FileArgs a, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
            }
            object[] parameters = new object[] { a };
            RRQMRPC.RRQMTest.FileArgs returnData = Client.Invoke<RRQMRPC.RRQMTest.FileArgs>("SayHello", invokeOption, parameters);
            return returnData;
        }

        public async Task<RRQMRPC.RRQMTest.FileArgs> SayHelloAsync(RRQMRPC.RRQMTest.FileArgs a, InvokeOption invokeOption = null)
        {
            if (Client == null)
            {
                throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
            }
            return await Task.Run(() =>
            {
                return SayHello(a, invokeOption);
            });
        }
    }
}