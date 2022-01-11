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
namespace RRQMProxy
{
public interface IServer:IRemoteServer
{
 System.String TestJsonRpc (System.String str,InvokeOption invokeOption = null);
Task<System.String> TestJsonRpcAsync (System.String str,InvokeOption invokeOption = null);

}
public class Server :IServer
{
public Server(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.String TestJsonRpc (System.String str,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{str};
System.String returnData=Client.Invoke<System.String>("TestJsonRpc",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> TestJsonRpcAsync (System.String str,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestJsonRpc(str,invokeOption);});
}

}
}
