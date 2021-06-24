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
public interface IMyOperation
{
 System.String SayHello (System.Int32 a,InvokeOption invokeOption = null);
Task<System.String> SayHelloAsync (System.Int32 a,InvokeOption invokeOption = null);
 System.Int32 Sum (System.Int32 a,System.Int32 b,InvokeOption invokeOption = null);
Task<System.Int32> SumAsync (System.Int32 a,System.Int32 b,InvokeOption invokeOption = null);
}
public class MyOperation :IMyOperation
{
public MyOperation(IRPCClient client)
{
this.Client=client;
}
public IRPCClient Client{get;private set; }
public System.String SayHello (System.Int32 a,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
System.String returnData=Client.Invoke<System.String>("SayHello",invokeOption, parameters);
return returnData;
}
public  async Task<System.String> SayHelloAsync (System.Int32 a,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return SayHello(a,invokeOption);});
}
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
public  async Task<System.Int32> SumAsync (System.Int32 a,System.Int32 b,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Sum(a,b,invokeOption);});
}
}
}
