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
using RpcArgsClassLib;
using RRQMCore.XREF.Newtonsoft.Json.Linq;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace XUnitService
{
    public enum MyEnum
    {
        T1 = 0,
        T2 = 100,
        T3 = 200
    }

    public class Args
    {
        public int P1 { get; set; }
        public double P2 { get; set; }
        public string P3 { get; set; }
    }

    public class Class01
    {
        public int Age { get; set; } = 1;
        public string Name { get; set; }
    }

    public class Class02
    {
        public int Age { get; set; }
        public List<int> list { get; set; }
        public string Name { get; set; }
        public int[] nums { get; set; }
    }

    public class Class03 : Class02
    {
        public int Length { get; set; }
    }

    public class Class04
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
        public int P3 { get; set; }
    }

    public struct StructArgs
    {
        public int P1 { get; set; }
    }

    [Route("/[controller]/[action]")]
    public class Server : ControllerBase
    {
        public static bool isStart;

        public static Action<string> ShowMsgMethod;

        private int a;

        public Server()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += this.Timer_Elapsed;
            timer.Start();
        }

        [XmlRpc(MethodName = "Xml_Test01_Performance")]
        [JsonRpc(MethodName = "Json_Test01_Performance")]
        [Route]
        [RRQMRPC]
        [Description("性能测试")]
        public void Test01_Performance()
        {
            a++;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        [Description("测试异步字符串")]
        public async Task<string> Test02_TaskString(string msg)
        {
            return await Task.Run(() =>
            {
                return msg;
            });
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public ProxyClass1 Test03_GetProxyClass()
        {
            return new ProxyClass1() { P1 = 10, P2 = new ProxyClass2() { P1 = 100, P2 = new ProxyClass3() { P1 = 1000 } } };
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public int Test04_In32DefaultValue(int a = 100)
        {
            return a;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public void Test05_NoneReturnNoneParameter()
        {
        }

        [RRQMRPC]
        public void Test06_OutParameters(out string name, out int age, out string occupation)
        {
            name = "若汝棋茗";
            age = 18;
            occupation = "搬砖工程师";
        }

        [RRQMRPC]
        public void Test07_OutStringParameter(out string name)
        {
            name = "若汝棋茗";
        }

        [RRQMRPC]
        public void Test08_RefStringParameter(ref string name)
        {
            name = name + "ref";
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public bool Test09_Boolean(bool b)
        {
            return b;
        }

        [XmlRpc]
        [JsonRpc]
        [RRQMRPC]
        public string Test10_StringDefaultNullValue(string s = null)
        {
            return s;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public string Test11_StringDefaultValue(string s = "RRQM")
        {
            return s;
        }

        [JsonRpc]
        [Route]
        [RRQMRPC]
        public Dictionary<int, string> Test12_Dictionary(int length)
        {
            Dictionary<int, string> valuePairs = new Dictionary<int, string>();
            for (int i = 0; i < length; i++)
            {
                valuePairs.Add(i, i.ToString());
            }

            return valuePairs;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public async Task Test13_Task()
        {
            await Task.Run(() =>
            {
                ShowMsg("TestTaskAsync");
            });
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public List<Class01> Test14_ListClass01(int length)
        {
            List<Class01> list = new List<Class01>();
            for (int i = 0; i < length; i++)
            {
                list.Add(new Class01() { Age = i });
            }
            return list;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public Args Test15_ReturnArgs()
        {
            return new Args() { P1 = 10, P2 = 10.0, P3 = "RRQM" };
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public Class04 Test16_ReturnClass4(int a, string b, int c = 10)
        {
            return new Class04() { P1 = a, P2 = b, P3 = c };
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public double Test17_DoubleDefaultValue(double a = 3.1415926)
        {
            return a;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public Class01 Test18_Class1(Class01 class01)
        {
            return class01;
        }

        [RRQMRPC(Async = true)]//此处必须使用异步，要么在方法里面使用Task.Run
        public string Test19_CallBack(string id,int age)
        {
            //先在RPC服务器中找到TcpRpc解析器。
            TcpRpcParser tcpRpcParser = ((TcpRpcParser)this.RPCService.RPCParsers["tcpRPCParser"]);

            //然后检索出id对应的RpcSocketClient
            if (tcpRpcParser.TryGetSocketClient(id, out RpcSocketClient socketClient))
            {
                //最后调用CallBack
                string msg = socketClient.Invoke<string>("SayHello", InvokeOption.WaitInvoke, age);
                ShowMsg($"TestCallBack，mes={msg}");
                return msg;
            }
            return null;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public string Test20_XmlRpc(string param, int a, double b, Args[] args)
        {
            return "RRQM";
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public JObject Test21_JsonRpcReturnJObject()
        {
            JObject jobj = new JObject();
            jobj.Add("P1", "P1");
            jobj.Add("P2", "P2");
            jobj.Add("P3", "P3");
            return jobj;
        }

        [JsonRpc]
        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public int Test22_IncludeCaller(ICallContext serverCallContext, int a)
        {
            if (serverCallContext is JsonRpcServerCallContext jsonRpcServerCallContext)
            {
            }
            return a;
        }

        private int invokeCount;

        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public int Test23_InvokeType(ICallContext serverCallContext)
        {
            return invokeCount++;
        }

        [RRQMRPC]
        public int Test25_TestStruct(StructArgs structArgs)
        {
            return structArgs.P1;
        }

        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public int Test26_TestCancellationToken(ICallContext serverCallContext)
        {
            int i = 0;
            serverCallContext.TokenSource.Token.Register(() =>
            {
                this.ShowMsg($"任务已取消，i={i}");
            });

            for (; i < 500; i++)
            {
                if (serverCallContext.TokenSource.Token.IsCancellationRequested)
                {
                    return 10;
                }
                Thread.Sleep(20);
            }

            return 1;
        }
        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public void Test27_TestCallBackFromCallContext(ICallContext serverCallContext)
        {
            if (serverCallContext.Caller is RpcSocketClient socketClient)
            {
                Task.Run(() =>
                {
                    //直接调用CallBack
                    string msg = socketClient.Invoke<string>("SayHello", InvokeOption.WaitInvoke, 10);
                    ShowMsg($"TestCallBack，mes={msg}");
                });
            }
        }

        /// <summary>
        /// "测试从RPC创建通道，从而实现流数据的传输"
        /// </summary>
        /// <param name="serverCallContext"></param>
        /// <param name="channelID"></param>
        [Description("测试从RPC创建通道，从而实现流数据的传输")]
        [RRQMRPC(MethodFlags.IncludeCallContext)]
        public void Test28_TestChannel(ICallContext serverCallContext, int channelID)
        {
            if (serverCallContext.Caller is RpcSocketClient socketClient)
            {
                if (socketClient.TrySubscribeChannel(channelID, out Channel channel))
                {
                    for (int i = 0; i < 1024; i++)
                    {
                        channel.Write(new byte[1024]);
                    }
                }
            }
        }

        private void ShowMsg(string msg)
        {
            ShowMsgMethod?.Invoke(msg);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isStart)
            {
                this.ShowMsg($"PerformanceTest,处理{a}次");
                a = 0;
            }
        }
    }
}