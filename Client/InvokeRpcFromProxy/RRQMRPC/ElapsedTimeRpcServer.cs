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
using System;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using RRQMCore.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RRQMRPC.RRQMTest
{
public interface IElapsedTimeRpcServer:IRemoteServer
{
///<summary>
///测试可取消的调用
///</summary>
 System.Boolean DelayInvoke (System.Int32 tick,InvokeOption invokeOption = null);
///<summary>
///测试可取消的调用
///</summary>
Task<System.Boolean> DelayInvokeAsync (System.Int32 tick,InvokeOption invokeOption = null);
}
public class ElapsedTimeRpcServer :IElapsedTimeRpcServer
{
public ElapsedTimeRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.Boolean DelayInvoke (System.Int32 tick,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{tick};
System.Boolean returnData=Client.Invoke<System.Boolean>("DelayInvoke",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Boolean> DelayInvokeAsync (System.Int32 tick,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return DelayInvoke(tick,invokeOption);});
}
}
}
