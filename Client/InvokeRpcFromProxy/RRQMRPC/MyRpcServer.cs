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
public interface IMyRpcServer
{
IRpcClient Client{get;}
///<summary>
///</summary>
 System.String TestOne (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.String> TestOneAsync (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.String TestOne_Name (System.Int32 id,System.String name,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.String> TestOne_NameAsync (System.Int32 id,System.String name,InvokeOption invokeOption = null);
///<summary>
///</summary>
 String AsyncTestOne (System.Int32 id,InvokeOption invokeOption = null);
///<summary>
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
