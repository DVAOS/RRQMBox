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
public class Server
{
public Server(IRPCClient client)
{
this.Client=client;
}
public IRPCClient Client{get;private set; }
public  void TestNullReturnNullParameter (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.RPCInvoke("TestNullReturnNullParameter",ref parameters,invokeOption);
if(parameters!=null)
{
}
}
public  async void TestNullReturnNullParameterAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
TestNullReturnNullParameter(invokeOption);});
}
public System.String TestStringReturnNullParameter (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.String returnData=Client.RPCInvoke<System.String>("TestStringReturnNullParameter",ref parameters,invokeOption);
if(parameters!=null)
{
}
return returnData;
}
public  async Task<System.String> TestStringReturnNullParameterAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestStringReturnNullParameter(invokeOption);});
}
public  void TestNullReturnStringParameter (System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{name};
Client.RPCInvoke("TestNullReturnStringParameter",ref parameters,invokeOption);
name=default(System.String);
if(parameters!=null)
{
name=(System.String)parameters[0];
}
}
public  async void TestNullReturnStringParameterAsync (System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
TestNullReturnStringParameter(name,invokeOption);});
}
public  void TestNullReturnOutStringParameter (out System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{default(System.String)};
Client.RPCInvoke("TestNullReturnOutStringParameter",ref parameters,invokeOption);
name=default(System.String);
if(parameters!=null)
{
name=(System.String)parameters[0];
}
}
public System.String TestStringReturnOutStringParameter (out System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{default(System.String)};
System.String returnData=Client.RPCInvoke<System.String>("TestStringReturnOutStringParameter",ref parameters,invokeOption);
name=default(System.String);
if(parameters!=null)
{
name=(System.String)parameters[0];
}
return returnData;
}
public  void TestNullReturnRefStringParameter (ref System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{name};
Client.RPCInvoke("TestNullReturnRefStringParameter",ref parameters,invokeOption);
name=default(System.String);
if(parameters!=null)
{
name=(System.String)parameters[0];
}
}
public  void TestNullReturnOutParameters (out System.String name,out System.Int32 age,out System.String occupation,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{default(System.String),default(System.Int32),default(System.String)};
Client.RPCInvoke("TestNullReturnOutParameters",ref parameters,invokeOption);
name=default(System.String);
age=default(System.Int32);
occupation=default(System.String);
if(parameters!=null)
{
name=(System.String)parameters[0];
age=(System.Int32)parameters[1];
occupation=(System.String)parameters[2];
}
}
public  void TestClass1 (RRQMRPC.RRQMTest.Test01 test01,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{test01};
Client.RPCInvoke("TestClass1",ref parameters,invokeOption);
test01=default(RRQMRPC.RRQMTest.Test01);
if(parameters!=null)
{
test01=(RRQMRPC.RRQMTest.Test01)parameters[0];
}
}
public  async void TestClass1Async (RRQMRPC.RRQMTest.Test01 test01,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
TestClass1(test01,invokeOption);});
}
public RRQMRPC.RRQMTest.Test02 TestClass1AndClass2 (RRQMRPC.RRQMTest.Test01 test01,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{test01};
RRQMRPC.RRQMTest.Test02 returnData=Client.RPCInvoke<RRQMRPC.RRQMTest.Test02>("TestClass1AndClass2",ref parameters,invokeOption);
test01=default(RRQMRPC.RRQMTest.Test01);
if(parameters!=null)
{
test01=(RRQMRPC.RRQMTest.Test01)parameters[0];
}
return returnData;
}
public  async Task<RRQMRPC.RRQMTest.Test02> TestClass1AndClass2Async (RRQMRPC.RRQMTest.Test01 test01,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return TestClass1AndClass2(test01,invokeOption);});
}
public  void TestClass3 (RRQMRPC.RRQMTest.Test03 test03,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{test03};
Client.RPCInvoke("TestClass3",ref parameters,invokeOption);
test03=default(RRQMRPC.RRQMTest.Test03);
if(parameters!=null)
{
test03=(RRQMRPC.RRQMTest.Test03)parameters[0];
}
}
public  async void TestClass3Async (RRQMRPC.RRQMTest.Test03 test03,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
TestClass3(test03,invokeOption);});
}
public  void TestGetSocketClient (System.String iDToken,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{iDToken};
Client.RPCInvoke("TestGetSocketClient",ref parameters,invokeOption);
iDToken=default(System.String);
if(parameters!=null)
{
iDToken=(System.String)parameters[0];
}
}
public  async void TestGetSocketClientAsync (System.String iDToken,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
TestGetSocketClient(iDToken,invokeOption);});
}
public  void TestSleep (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.RPCInvoke("TestSleep",ref parameters,invokeOption);
if(parameters!=null)
{
}
}
public  async void TestSleepAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
TestSleep(invokeOption);});
}
}
}
