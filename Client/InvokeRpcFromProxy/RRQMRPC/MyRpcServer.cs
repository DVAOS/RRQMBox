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
public interface IMyRpcServer:IRemoteServer
{
///<summary>
///测试同步调用
///</summary>
 System.String TestOne (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试同步调用
///</summary>
Task<System.String> TestOneAsync (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试TestTwo
///</summary>
 System.String TestTwo (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试TestTwo
///</summary>
Task<System.String> TestTwoAsync (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试重载调用
///</summary>
 System.String TestOne_Name (System.Int32 id,System.String name,InvokeOption invokeOption = null);
///<summary>
///测试重载调用
///</summary>
Task<System.String> TestOne_NameAsync (System.Int32 id,System.String name,InvokeOption invokeOption = null);
///<summary>
///测试Out
///</summary>
  void TestOut (out System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试Ref
///</summary>
  void TestRef (ref System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试异步
///</summary>
 String AsyncTestOne (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///测试异步
///</summary>
Task<String> AsyncTestOneAsync (System.Int32 id,InvokeOption invokeOption = null);
}
public class MyRpcServer :IMyRpcServer
{
public MyRpcServer(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public System.String TestOne (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
System.String returnData=Client.Invoke<System.String>("TestOne",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> TestOneAsync (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestOne(id,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.String TestTwo (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
System.String returnData=Client.Invoke<System.String>("TestTwo",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> TestTwoAsync (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestTwo(id,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.String TestOne_Name (System.Int32 id,System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id,name};
System.String returnData=Client.Invoke<System.String>("TestOne_Name",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> TestOne_NameAsync (System.Int32 id,System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestOne_Name(id,name,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public  void TestOut (out System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{default(System.Int32)};
Type[] types = new Type[]{typeof(System.Int32)};
Client.Invoke("TestOut",invokeOption,ref parameters,types);
if(parameters!=null)
{
id=(System.Int32)parameters[0];
}
else
{
id=default(System.Int32);
}
}
///<summary>
///<inheritdoc/>
///</summary>
public  void TestRef (ref System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
Type[] types = new Type[]{typeof(System.Int32)};
Client.Invoke("TestRef",invokeOption,ref parameters,types);
if(parameters!=null)
{
id=(System.Int32)parameters[0];
}
}
///<summary>
///<inheritdoc/>
///</summary>
public String AsyncTestOne (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
String returnData=Client.Invoke<String>("AsyncTestOne",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<String> AsyncTestOneAsync (System.Int32 id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return AsyncTestOne(id,invokeOption);});
}
}
}
