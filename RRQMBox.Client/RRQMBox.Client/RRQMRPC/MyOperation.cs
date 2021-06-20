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
 RRQMRPC.RRQMTest.FileArgs SayHello (RRQMRPC.RRQMTest.FileArgs a,InvokeOption invokeOption = null);
Task<RRQMRPC.RRQMTest.FileArgs> SayHelloAsync (RRQMRPC.RRQMTest.FileArgs a,InvokeOption invokeOption = null);
}
public class MyOperation :IMyOperation
{
public MyOperation(IRPCClient client)
{
this.Client=client;
}
public IRPCClient Client{get;private set; }
public RRQMRPC.RRQMTest.FileArgs SayHello (RRQMRPC.RRQMTest.FileArgs a,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
RRQMRPC.RRQMTest.FileArgs returnData=Client.Invoke<RRQMRPC.RRQMTest.FileArgs>("SayHello",invokeOption, parameters);
return returnData;
}
public  async Task<RRQMRPC.RRQMTest.FileArgs> SayHelloAsync (RRQMRPC.RRQMTest.FileArgs a,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return SayHello(a,invokeOption);});
}
}
}
