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
public interface IPerformanceRpcServer:IRemoteServer
{
///<summary>
///测试性能
///</summary>
 System.String Performance (InvokeOption invokeOption = null);
///<summary>
///测试性能
///</summary>
Task<System.String> PerformanceAsync (InvokeOption invokeOption = null);
///<summary>
///测试并发性能
///</summary>
 Int32 ConPerformance (System.Int32 num,InvokeOption invokeOption = null);
///<summary>
///测试并发性能
///</summary>
Task<Int32> ConPerformanceAsync (System.Int32 num,InvokeOption invokeOption = null);
}
public class PerformanceRpcServer :IPerformanceRpcServer
{
public PerformanceRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.String Performance (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.String returnData=Client.Invoke<System.String>("Performance",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> PerformanceAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Performance(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public Int32 ConPerformance (System.Int32 num,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{num};
Int32 returnData=Client.Invoke<Int32>("ConPerformance",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<Int32> ConPerformanceAsync (System.Int32 num,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return ConPerformance(num,invokeOption);});
}
}
}
