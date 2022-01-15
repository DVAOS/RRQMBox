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
public interface IServer
{
IRpcClient Client{get;}
///<summary>
///性能测试
///</summary>
  void Test01_Performance (InvokeOption invokeOption = null);
///<summary>
///性能测试
///</summary>
void Test01_PerformanceAsync (InvokeOption invokeOption = null);
///<summary>
///测试异步字符串
///</summary>
 String Test02_TaskString (System.String msg,InvokeOption invokeOption = null);
///<summary>
///测试异步字符串
///</summary>
Task<String> Test02_TaskStringAsync (System.String msg,InvokeOption invokeOption = null);
///<summary>
///</summary>
 RRQMRPC.RRQMTest.ProxyClass1 Test03_GetProxyClass (InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<RRQMRPC.RRQMTest.ProxyClass1> Test03_GetProxyClassAsync (InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Int32 Test04_In32DefaultValue (System.Int32 a=100,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Int32> Test04_In32DefaultValueAsync (System.Int32 a=100,InvokeOption invokeOption = null);
///<summary>
///</summary>
  void Test05_NoneReturnNoneParameter (InvokeOption invokeOption = null);
///<summary>
///</summary>
void Test05_NoneReturnNoneParameterAsync (InvokeOption invokeOption = null);
///<summary>
///</summary>
  void Test06_OutParameters (out System.String name,out System.Int32 age,out System.String occupation,InvokeOption invokeOption = null);
///<summary>
///</summary>
  void Test07_OutStringParameter (out System.String name,InvokeOption invokeOption = null);
///<summary>
///</summary>
  void Test08_RefStringParameter (ref System.String name,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Boolean Test09_Boolean (System.Boolean b,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Boolean> Test09_BooleanAsync (System.Boolean b,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.String Test10_StringDefaultNullValue (System.String s=null,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.String> Test10_StringDefaultNullValueAsync (System.String s=null,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.String Test11_StringDefaultValue (System.String s="RRQM",InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.String> Test11_StringDefaultValueAsync (System.String s="RRQM",InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Collections.Generic.Dictionary<System.Int32,System.String> Test12_Dictionary (System.Int32 length,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Collections.Generic.Dictionary<System.Int32,System.String>> Test12_DictionaryAsync (System.Int32 length,InvokeOption invokeOption = null);
///<summary>
///</summary>
  void Test13_Task (InvokeOption invokeOption = null);
///<summary>
///</summary>
void Test13_TaskAsync (InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Collections.Generic.List<RRQMRPC.RRQMTest.Class01> Test14_ListClass01 (System.Int32 length,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Collections.Generic.List<RRQMRPC.RRQMTest.Class01>> Test14_ListClass01Async (System.Int32 length,InvokeOption invokeOption = null);
///<summary>
///</summary>
 RRQMRPC.RRQMTest.Args Test15_ReturnArgs (InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<RRQMRPC.RRQMTest.Args> Test15_ReturnArgsAsync (InvokeOption invokeOption = null);
///<summary>
///</summary>
 RRQMRPC.RRQMTest.Class04 Test16_ReturnClass4 (System.Int32 a,System.String b,System.Int32 c=10,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<RRQMRPC.RRQMTest.Class04> Test16_ReturnClass4Async (System.Int32 a,System.String b,System.Int32 c=10,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Double Test17_DoubleDefaultValue (System.Double a=3.1415926,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Double> Test17_DoubleDefaultValueAsync (System.Double a=3.1415926,InvokeOption invokeOption = null);
///<summary>
///</summary>
 RRQMRPC.RRQMTest.Class01 Test18_Class1 (RRQMRPC.RRQMTest.Class01 class01,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<RRQMRPC.RRQMTest.Class01> Test18_Class1Async (RRQMRPC.RRQMTest.Class01 class01,InvokeOption invokeOption = null);
///<summary>
///</summary>
  void Test19_CallBack (System.String id,InvokeOption invokeOption = null);
///<summary>
///</summary>
void Test19_CallBackAsync (System.String id,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.String Test20_XmlRpc (System.String param,System.Int32 a,System.Double b,RRQMRPC.RRQMTest.Args[] args,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.String> Test20_XmlRpcAsync (System.String param,System.Int32 a,System.Double b,RRQMRPC.RRQMTest.Args[] args,InvokeOption invokeOption = null);
///<summary>
///</summary>
 RRQMCore.XREF.Newtonsoft.Json.Linq.JObject Test21_JsonRpcReturnJObject (InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<RRQMCore.XREF.Newtonsoft.Json.Linq.JObject> Test21_JsonRpcReturnJObjectAsync (InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Int32 Test22_IncludeCaller (System.Int32 a,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Int32> Test22_IncludeCallerAsync (System.Int32 a,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Int32 Test23_InvokeType (InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Int32> Test23_InvokeTypeAsync (InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Int32 Test25_TestStruct (RRQMRPC.RRQMTest.StructArgs structArgs,InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Int32> Test25_TestStructAsync (RRQMRPC.RRQMTest.StructArgs structArgs,InvokeOption invokeOption = null);
///<summary>
///</summary>
 System.Int32 Test26_TestCancellationToken (InvokeOption invokeOption = null);
///<summary>
///</summary>
Task<System.Int32> Test26_TestCancellationTokenAsync (InvokeOption invokeOption = null);
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
public  void Test01_Performance (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Test01_Performance",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
public  async void Test01_PerformanceAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
Test01_Performance(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public String Test02_TaskString (System.String msg,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{msg};
String returnData=Client.Invoke<String>("Test02_TaskString",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<String> Test02_TaskStringAsync (System.String msg,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test02_TaskString(msg,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public RRQMRPC.RRQMTest.ProxyClass1 Test03_GetProxyClass (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
RRQMRPC.RRQMTest.ProxyClass1 returnData=Client.Invoke<RRQMRPC.RRQMTest.ProxyClass1>("Test03_GetProxyClass",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<RRQMRPC.RRQMTest.ProxyClass1> Test03_GetProxyClassAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test03_GetProxyClass(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 Test04_In32DefaultValue (System.Int32 a=100,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
System.Int32 returnData=Client.Invoke<System.Int32>("Test04_In32DefaultValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> Test04_In32DefaultValueAsync (System.Int32 a=100,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test04_In32DefaultValue(a,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public  void Test05_NoneReturnNoneParameter (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Test05_NoneReturnNoneParameter",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
public  async void Test05_NoneReturnNoneParameterAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
Test05_NoneReturnNoneParameter(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public  void Test06_OutParameters (out System.String name,out System.Int32 age,out System.String occupation,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{default(System.String),default(System.Int32),default(System.String)};
Type[] types = new Type[]{typeof(System.String),typeof(System.Int32),typeof(System.String)};
Client.Invoke("Test06_OutParameters",invokeOption,ref parameters,types);
if(parameters!=null)
{
name=(System.String)parameters[0];
age=(System.Int32)parameters[1];
occupation=(System.String)parameters[2];
}
else
{
name=default(System.String);
age=default(System.Int32);
occupation=default(System.String);
}
}
///<summary>
///<inheritdoc/>
///</summary>
public  void Test07_OutStringParameter (out System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{default(System.String)};
Type[] types = new Type[]{typeof(System.String)};
Client.Invoke("Test07_OutStringParameter",invokeOption,ref parameters,types);
if(parameters!=null)
{
name=(System.String)parameters[0];
}
else
{
name=default(System.String);
}
}
///<summary>
///<inheritdoc/>
///</summary>
public  void Test08_RefStringParameter (ref System.String name,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{name};
Type[] types = new Type[]{typeof(System.String)};
Client.Invoke("Test08_RefStringParameter",invokeOption,ref parameters,types);
if(parameters!=null)
{
name=(System.String)parameters[0];
}
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Boolean Test09_Boolean (System.Boolean b,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{b};
System.Boolean returnData=Client.Invoke<System.Boolean>("Test09_Boolean",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Boolean> Test09_BooleanAsync (System.Boolean b,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test09_Boolean(b,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.String Test10_StringDefaultNullValue (System.String s=null,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{s};
System.String returnData=Client.Invoke<System.String>("Test10_StringDefaultNullValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> Test10_StringDefaultNullValueAsync (System.String s=null,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test10_StringDefaultNullValue(s,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.String Test11_StringDefaultValue (System.String s="RRQM",InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{s};
System.String returnData=Client.Invoke<System.String>("Test11_StringDefaultValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> Test11_StringDefaultValueAsync (System.String s="RRQM",InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test11_StringDefaultValue(s,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Collections.Generic.Dictionary<System.Int32,System.String> Test12_Dictionary (System.Int32 length,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
System.Collections.Generic.Dictionary<System.Int32,System.String> returnData=Client.Invoke<System.Collections.Generic.Dictionary<System.Int32,System.String>>("Test12_Dictionary",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Collections.Generic.Dictionary<System.Int32,System.String>> Test12_DictionaryAsync (System.Int32 length,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test12_Dictionary(length,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public  void Test13_Task (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Test13_Task",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
public  async void Test13_TaskAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
Test13_Task(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Collections.Generic.List<RRQMRPC.RRQMTest.Class01> Test14_ListClass01 (System.Int32 length,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
System.Collections.Generic.List<RRQMRPC.RRQMTest.Class01> returnData=Client.Invoke<System.Collections.Generic.List<RRQMRPC.RRQMTest.Class01>>("Test14_ListClass01",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Collections.Generic.List<RRQMRPC.RRQMTest.Class01>> Test14_ListClass01Async (System.Int32 length,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test14_ListClass01(length,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public RRQMRPC.RRQMTest.Args Test15_ReturnArgs (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
RRQMRPC.RRQMTest.Args returnData=Client.Invoke<RRQMRPC.RRQMTest.Args>("Test15_ReturnArgs",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<RRQMRPC.RRQMTest.Args> Test15_ReturnArgsAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test15_ReturnArgs(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public RRQMRPC.RRQMTest.Class04 Test16_ReturnClass4 (System.Int32 a,System.String b,System.Int32 c=10,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b,c};
RRQMRPC.RRQMTest.Class04 returnData=Client.Invoke<RRQMRPC.RRQMTest.Class04>("Test16_ReturnClass4",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<RRQMRPC.RRQMTest.Class04> Test16_ReturnClass4Async (System.Int32 a,System.String b,System.Int32 c=10,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test16_ReturnClass4(a,b,c,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Double Test17_DoubleDefaultValue (System.Double a=3.1415926,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
System.Double returnData=Client.Invoke<System.Double>("Test17_DoubleDefaultValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Double> Test17_DoubleDefaultValueAsync (System.Double a=3.1415926,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test17_DoubleDefaultValue(a,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public RRQMRPC.RRQMTest.Class01 Test18_Class1 (RRQMRPC.RRQMTest.Class01 class01,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{class01};
RRQMRPC.RRQMTest.Class01 returnData=Client.Invoke<RRQMRPC.RRQMTest.Class01>("Test18_Class1",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<RRQMRPC.RRQMTest.Class01> Test18_Class1Async (RRQMRPC.RRQMTest.Class01 class01,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test18_Class1(class01,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public  void Test19_CallBack (System.String id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id};
Client.Invoke("Test19_CallBack",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
public  async void Test19_CallBackAsync (System.String id,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
await Task.Run(() =>{
Test19_CallBack(id,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.String Test20_XmlRpc (System.String param,System.Int32 a,System.Double b,RRQMRPC.RRQMTest.Args[] args,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{param,a,b,args};
System.String returnData=Client.Invoke<System.String>("Test20_XmlRpc",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.String> Test20_XmlRpcAsync (System.String param,System.Int32 a,System.Double b,RRQMRPC.RRQMTest.Args[] args,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test20_XmlRpc(param,a,b,args,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public RRQMCore.XREF.Newtonsoft.Json.Linq.JObject Test21_JsonRpcReturnJObject (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
RRQMCore.XREF.Newtonsoft.Json.Linq.JObject returnData=Client.Invoke<RRQMCore.XREF.Newtonsoft.Json.Linq.JObject>("Test21_JsonRpcReturnJObject",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<RRQMCore.XREF.Newtonsoft.Json.Linq.JObject> Test21_JsonRpcReturnJObjectAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test21_JsonRpcReturnJObject(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 Test22_IncludeCaller (System.Int32 a,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
System.Int32 returnData=Client.Invoke<System.Int32>("Test22_IncludeCaller",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> Test22_IncludeCallerAsync (System.Int32 a,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test22_IncludeCaller(a,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 Test23_InvokeType (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.Int32 returnData=Client.Invoke<System.Int32>("Test23_InvokeType",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> Test23_InvokeTypeAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test23_InvokeType(invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 Test25_TestStruct (RRQMRPC.RRQMTest.StructArgs structArgs,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{structArgs};
System.Int32 returnData=Client.Invoke<System.Int32>("Test25_TestStruct",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> Test25_TestStructAsync (RRQMRPC.RRQMTest.StructArgs structArgs,InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test25_TestStruct(structArgs,invokeOption);});
}
///<summary>
///<inheritdoc/>
///</summary>
public System.Int32 Test26_TestCancellationToken (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("IRPCClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.Int32 returnData=Client.Invoke<System.Int32>("Test26_TestCancellationToken",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
public  async Task<System.Int32> Test26_TestCancellationTokenAsync (InvokeOption invokeOption = null)
{
if(Client==null)
{
throw new RRQMRPCException("RPCClient为空，请先初始化或者进行赋值");
}
return await Task.Run(() =>{
return Test26_TestCancellationToken(invokeOption);});
}
}
}
