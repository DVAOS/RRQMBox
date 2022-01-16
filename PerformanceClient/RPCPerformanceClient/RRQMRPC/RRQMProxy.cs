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
public interface ITestController:IRemoteServer
{
 GetAddResponse GetAdd (GetAddRequest request,InvokeOption invokeOption = null);
Task<GetAddResponse> GetAddAsync (GetAddRequest request,InvokeOption invokeOption = null);

 System.Int32 Sum (System.Int32 a,System.Int32 b,InvokeOption invokeOption = null);
Task<System.Int32> SumAsync (System.Int32 a,System.Int32 b,InvokeOption invokeOption = null);

 System.Byte[] GetBytes (System.Int32 length,InvokeOption invokeOption = null);
Task<System.Byte[]> GetBytesAsync (System.Int32 length,InvokeOption invokeOption = null);

 System.String GetBigString (InvokeOption invokeOption = null);
Task<System.String> GetBigStringAsync (InvokeOption invokeOption = null);

}
public class TestController :ITestController
{
public TestController(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
public GetAddResponse GetAdd (GetAddRequest request,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{request};
GetAddResponse returnData=Client.Invoke<GetAddResponse>("GetAdd",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<GetAddResponse> GetAddAsync (GetAddRequest request,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return GetAdd(request,invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 Sum (System.Int32 a,System.Int32 b,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
System.Int32 returnData=Client.Invoke<System.Int32>("Sum",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> SumAsync (System.Int32 a,System.Int32 b,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Sum(a,b,invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public System.Byte[] GetBytes (System.Int32 length,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
System.Byte[] returnData=Client.Invoke<System.Byte[]>("GetBytes",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Byte[]> GetBytesAsync (System.Int32 length,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return GetBytes(length,invokeOption);});
}

///<summary>
///<inheritdoc/>
///</summary>
public System.String GetBigString (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.String returnData=Client.Invoke<System.String>("GetBigString",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> GetBigStringAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return GetBigString(invokeOption);});
}

}

public class GetAddResponse
{
public System.Int32 Result{get;set;}
}


public class GetAddRequest
{
public System.Int32 A{get;set;}
public System.Int32 B{get;set;}
}

}
