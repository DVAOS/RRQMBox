using System;
using RRQMCore;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
namespace RRQMProxy
{
public interface IXUnitTestController:IRemoteServer
{
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 MySum (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> MySumAsync (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default);

///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test01_Performance (IInvokeOption invokeOption = default);
///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task Test01_PerformanceAsync (IInvokeOption invokeOption = default);

///<summary>
///测试异步字符串
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.String Test02_TaskString (System.String msg,IInvokeOption invokeOption = default);
///<summary>
///测试异步字符串
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.String> Test02_TaskStringAsync (System.String msg,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
ProxyClass1 Test03_GetProxyClass (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<ProxyClass1> Test03_GetProxyClassAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 Test04_In32DefaultValue (System.Int32 a=100,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> Test04_In32DefaultValueAsync (System.Int32 a=100,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test05_NoneReturnNoneParameter (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task Test05_NoneReturnNoneParameterAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test06_OutParameters (out System.String name,out System.Int32 age,out System.String occupation,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test07_OutStringParameter (out System.String name,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test08_RefStringParameter (ref System.String name,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Boolean Test09_Boolean (System.Boolean b,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Boolean> Test09_BooleanAsync (System.Boolean b,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.String Test10_StringDefaultNullValue (System.String s=null,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.String> Test10_StringDefaultNullValueAsync (System.String s=null,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.String Test11_StringDefaultValue (System.String s="RRQM",IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.String> Test11_StringDefaultValueAsync (System.String s="RRQM",IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Collections.Generic.Dictionary<System.Int32,System.String> Test12_Dictionary (System.Int32 length,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Collections.Generic.Dictionary<System.Int32,System.String>> Test12_DictionaryAsync (System.Int32 length,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test13_Task (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task Test13_TaskAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Collections.Generic.List<Class01> Test14_ListClass01 (System.Int32 length,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Collections.Generic.List<Class01>> Test14_ListClass01Async (System.Int32 length,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Args Test15_ReturnArgs (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<Args> Test15_ReturnArgsAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Class04 Test16_ReturnClass4 (System.Int32 a,System.String b,System.Int32 c=10,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<Class04> Test16_ReturnClass4Async (System.Int32 a,System.String b,System.Int32 c=10,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Double Test17_DoubleDefaultValue (System.Double a=3.1415926,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Double> Test17_DoubleDefaultValueAsync (System.Double a=3.1415926,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Class01 Test18_Class1 (Class01 class01,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<Class01> Test18_Class1Async (Class01 class01,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.String Test19_CallBack (System.String id,System.Int32 age,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.String> Test19_CallBackAsync (System.String id,System.Int32 age,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.String Test20_XmlRpc (System.String param,System.Int32 a,System.Double b,Args[] args,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.String> Test20_XmlRpcAsync (System.String param,System.Int32 a,System.Double b,Args[] args,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
RRQMCore.XREF.Newtonsoft.Json.Linq.JObject Test21_JsonRpcReturnJObject (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<RRQMCore.XREF.Newtonsoft.Json.Linq.JObject> Test21_JsonRpcReturnJObjectAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 Test22_IncludeCaller (System.Int32 a,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> Test22_IncludeCallerAsync (System.Int32 a,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 Test23_InvokeType (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> Test23_InvokeTypeAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 Test25_TestStruct (StructArgs structArgs,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> Test25_TestStructAsync (StructArgs structArgs,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 Test26_TestCancellationToken (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> Test26_TestCancellationTokenAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test27_TestCallBackFromCallContext (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task Test27_TestCallBackFromCallContextAsync (IInvokeOption invokeOption = default);

///<summary>
///测试从RPC创建通道，从而实现流数据的传输
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Test28_TestChannel (System.Int32 channelID,IInvokeOption invokeOption = default);
///<summary>
///测试从RPC创建通道，从而实现流数据的传输
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task Test28_TestChannelAsync (System.Int32 channelID,IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 Sum (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> SumAsync (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default);

///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Json_Test01_Performance (IInvokeOption invokeOption = default);
///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task Json_Test01_PerformanceAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Int32 HttpGetSum (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Int32> HttpGetSumAsync (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default);

///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void HttpGetPerformance (IInvokeOption invokeOption = default);
///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task HttpGetPerformanceAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
ProxyClass1 HttpGetGetProxyClass (IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<ProxyClass1> HttpGetGetProxyClassAsync (IInvokeOption invokeOption = default);

///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
System.Collections.Generic.List<Class01> HttpGetGetListClass01 (System.Int32 length,IInvokeOption invokeOption = default);
///<summary>
///无注释信息
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task<System.Collections.Generic.List<Class01>> HttpGetGetListClass01Async (System.Int32 length,IInvokeOption invokeOption = default);

///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
 void Xml_Test01_Performance (IInvokeOption invokeOption = default);
///<summary>
///性能测试
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
Task Xml_Test01_PerformanceAsync (IInvokeOption invokeOption = default);

}
public class XUnitTestController :IXUnitTestController
{
public XUnitTestController(IRpcClient client)
{
this.Client=client;
}
public IRpcClient Client{get;private set; }
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 MySum (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
System.Int32 returnData=Client.Invoke<System.Int32>("MySum",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> MySumAsync (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
return Client.InvokeAsync<System.Int32>("MySum",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test01_Performance (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Test01_Performance",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task Test01_PerformanceAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync("Test01_Performance",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.String Test02_TaskString (System.String msg,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{msg};
System.String returnData=Client.Invoke<System.String>("Test02_TaskString",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.String> Test02_TaskStringAsync (System.String msg,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{msg};
return Client.InvokeAsync<System.String>("Test02_TaskString",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public ProxyClass1 Test03_GetProxyClass (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
ProxyClass1 returnData=Client.Invoke<ProxyClass1>("Test03_GetProxyClass",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<ProxyClass1> Test03_GetProxyClassAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync<ProxyClass1>("Test03_GetProxyClass",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 Test04_In32DefaultValue (System.Int32 a=100,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
System.Int32 returnData=Client.Invoke<System.Int32>("Test04_In32DefaultValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> Test04_In32DefaultValueAsync (System.Int32 a=100,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
return Client.InvokeAsync<System.Int32>("Test04_In32DefaultValue",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test05_NoneReturnNoneParameter (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Test05_NoneReturnNoneParameter",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task Test05_NoneReturnNoneParameterAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync("Test05_NoneReturnNoneParameter",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test06_OutParameters (out System.String name,out System.Int32 age,out System.String occupation,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
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
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test07_OutStringParameter (out System.String name,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
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
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test08_RefStringParameter (ref System.String name,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
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
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Boolean Test09_Boolean (System.Boolean b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{b};
System.Boolean returnData=Client.Invoke<System.Boolean>("Test09_Boolean",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Boolean> Test09_BooleanAsync (System.Boolean b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{b};
return Client.InvokeAsync<System.Boolean>("Test09_Boolean",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.String Test10_StringDefaultNullValue (System.String s=null,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{s};
System.String returnData=Client.Invoke<System.String>("Test10_StringDefaultNullValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.String> Test10_StringDefaultNullValueAsync (System.String s=null,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{s};
return Client.InvokeAsync<System.String>("Test10_StringDefaultNullValue",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.String Test11_StringDefaultValue (System.String s="RRQM",IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{s};
System.String returnData=Client.Invoke<System.String>("Test11_StringDefaultValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.String> Test11_StringDefaultValueAsync (System.String s="RRQM",IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{s};
return Client.InvokeAsync<System.String>("Test11_StringDefaultValue",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Collections.Generic.Dictionary<System.Int32,System.String> Test12_Dictionary (System.Int32 length,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
System.Collections.Generic.Dictionary<System.Int32,System.String> returnData=Client.Invoke<System.Collections.Generic.Dictionary<System.Int32,System.String>>("Test12_Dictionary",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Collections.Generic.Dictionary<System.Int32,System.String>> Test12_DictionaryAsync (System.Int32 length,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
return Client.InvokeAsync<System.Collections.Generic.Dictionary<System.Int32,System.String>>("Test12_Dictionary",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test13_Task (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Test13_Task",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task Test13_TaskAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync("Test13_Task",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Collections.Generic.List<Class01> Test14_ListClass01 (System.Int32 length,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
System.Collections.Generic.List<Class01> returnData=Client.Invoke<System.Collections.Generic.List<Class01>>("Test14_ListClass01",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Collections.Generic.List<Class01>> Test14_ListClass01Async (System.Int32 length,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
return Client.InvokeAsync<System.Collections.Generic.List<Class01>>("Test14_ListClass01",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Args Test15_ReturnArgs (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Args returnData=Client.Invoke<Args>("Test15_ReturnArgs",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<Args> Test15_ReturnArgsAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync<Args>("Test15_ReturnArgs",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Class04 Test16_ReturnClass4 (System.Int32 a,System.String b,System.Int32 c=10,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b,c};
Class04 returnData=Client.Invoke<Class04>("Test16_ReturnClass4",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<Class04> Test16_ReturnClass4Async (System.Int32 a,System.String b,System.Int32 c=10,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b,c};
return Client.InvokeAsync<Class04>("Test16_ReturnClass4",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Double Test17_DoubleDefaultValue (System.Double a=3.1415926,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
System.Double returnData=Client.Invoke<System.Double>("Test17_DoubleDefaultValue",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Double> Test17_DoubleDefaultValueAsync (System.Double a=3.1415926,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
return Client.InvokeAsync<System.Double>("Test17_DoubleDefaultValue",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Class01 Test18_Class1 (Class01 class01,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{class01};
Class01 returnData=Client.Invoke<Class01>("Test18_Class1",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<Class01> Test18_Class1Async (Class01 class01,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{class01};
return Client.InvokeAsync<Class01>("Test18_Class1",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.String Test19_CallBack (System.String id,System.Int32 age,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id,age};
System.String returnData=Client.Invoke<System.String>("Test19_CallBack",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.String> Test19_CallBackAsync (System.String id,System.Int32 age,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{id,age};
return Client.InvokeAsync<System.String>("Test19_CallBack",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.String Test20_XmlRpc (System.String param,System.Int32 a,System.Double b,Args[] args,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{param,a,b,args};
System.String returnData=Client.Invoke<System.String>("Test20_XmlRpc",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.String> Test20_XmlRpcAsync (System.String param,System.Int32 a,System.Double b,Args[] args,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{param,a,b,args};
return Client.InvokeAsync<System.String>("Test20_XmlRpc",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public RRQMCore.XREF.Newtonsoft.Json.Linq.JObject Test21_JsonRpcReturnJObject (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
RRQMCore.XREF.Newtonsoft.Json.Linq.JObject returnData=Client.Invoke<RRQMCore.XREF.Newtonsoft.Json.Linq.JObject>("Test21_JsonRpcReturnJObject",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<RRQMCore.XREF.Newtonsoft.Json.Linq.JObject> Test21_JsonRpcReturnJObjectAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync<RRQMCore.XREF.Newtonsoft.Json.Linq.JObject>("Test21_JsonRpcReturnJObject",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 Test22_IncludeCaller (System.Int32 a,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
System.Int32 returnData=Client.Invoke<System.Int32>("Test22_IncludeCaller",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> Test22_IncludeCallerAsync (System.Int32 a,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a};
return Client.InvokeAsync<System.Int32>("Test22_IncludeCaller",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 Test23_InvokeType (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.Int32 returnData=Client.Invoke<System.Int32>("Test23_InvokeType",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> Test23_InvokeTypeAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync<System.Int32>("Test23_InvokeType",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 Test25_TestStruct (StructArgs structArgs,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{structArgs};
System.Int32 returnData=Client.Invoke<System.Int32>("Test25_TestStruct",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> Test25_TestStructAsync (StructArgs structArgs,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{structArgs};
return Client.InvokeAsync<System.Int32>("Test25_TestStruct",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 Test26_TestCancellationToken (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
System.Int32 returnData=Client.Invoke<System.Int32>("Test26_TestCancellationToken",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> Test26_TestCancellationTokenAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync<System.Int32>("Test26_TestCancellationToken",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test27_TestCallBackFromCallContext (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Test27_TestCallBackFromCallContext",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task Test27_TestCallBackFromCallContextAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync("Test27_TestCallBackFromCallContext",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Test28_TestChannel (System.Int32 channelID,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{channelID};
Client.Invoke("Test28_TestChannel",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task Test28_TestChannelAsync (System.Int32 channelID,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{channelID};
return Client.InvokeAsync("Test28_TestChannel",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 Sum (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
System.Int32 returnData=Client.Invoke<System.Int32>("Sum",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> SumAsync (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
return Client.InvokeAsync<System.Int32>("Sum",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Json_Test01_Performance (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Json_Test01_Performance",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task Json_Test01_PerformanceAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync("Json_Test01_Performance",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Int32 HttpGetSum (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
System.Int32 returnData=Client.Invoke<System.Int32>("GET:/XUnitTest/HttpGetSum?a={0}&b={1}",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Int32> HttpGetSumAsync (System.Int32 a,System.Int32 b,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{a,b};
return Client.InvokeAsync<System.Int32>("GET:/XUnitTest/HttpGetSum?a={0}&b={1}",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void HttpGetPerformance (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("GET:/XUnitTest/HttpGetPerformance",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task HttpGetPerformanceAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync("GET:/XUnitTest/HttpGetPerformance",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public ProxyClass1 HttpGetGetProxyClass (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
ProxyClass1 returnData=Client.Invoke<ProxyClass1>("GET:/XUnitTest/HttpGetGetProxyClass",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<ProxyClass1> HttpGetGetProxyClassAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync<ProxyClass1>("GET:/XUnitTest/HttpGetGetProxyClass",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public System.Collections.Generic.List<Class01> HttpGetGetListClass01 (System.Int32 length,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
System.Collections.Generic.List<Class01> returnData=Client.Invoke<System.Collections.Generic.List<Class01>>("GET:/XUnitTest/HttpGetGetListClass01?length={0}",invokeOption, parameters);
return returnData;
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task<System.Collections.Generic.List<Class01>> HttpGetGetListClass01Async (System.Int32 length,IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{length};
return Client.InvokeAsync<System.Collections.Generic.List<Class01>>("GET:/XUnitTest/HttpGetGetListClass01?length={0}",invokeOption, parameters);
}

///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public  void Xml_Test01_Performance (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
Client.Invoke("Xml_Test01_Performance",invokeOption, parameters);
}
///<summary>
///<inheritdoc/>
///</summary>
/// <exception cref="TimeoutException">调用超时</exception>
/// <exception cref="RpcSerializationException">序列化异常</exception>
/// <exception cref="RRQMRpcInvokeException">Rpc异常</exception>
/// <exception cref="RRQMException">其他异常</exception>
public Task Xml_Test01_PerformanceAsync (IInvokeOption invokeOption = default)
{
if(Client==null)
{
throw new RpcException("IRpcClient为空，请先初始化或者进行赋值");
}
object[] parameters = new object[]{};
return Client.InvokeAsync("Xml_Test01_Performance",invokeOption, parameters);
}

}

public class ProxyClass3
{
public System.Int32 P1{get;set;}
}


public class ProxyClass2
{
public System.Int32 P1{get;set;}
public ProxyClass3 P2{get;set;}
}


public class ProxyClass1
{
public System.Int32 P1{get;set;}
public ProxyClass2 P2{get;set;}
}


public class Class01
{
public System.Int32 Age{get;set;}
public System.String Name{get;set;}
}


public class Args
{
public System.Int32 P1{get;set;}
public System.Double P2{get;set;}
public System.String P3{get;set;}
}


public class Class04
{
public System.Int32 P1{get;set;}
public System.String P2{get;set;}
public System.Int32 P3{get;set;}
}


public struct StructArgs
{
public System.Int32 P1{get;set;}
}

}
