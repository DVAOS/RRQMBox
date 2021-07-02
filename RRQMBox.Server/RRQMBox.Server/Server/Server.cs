//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using RRQMSocket.RPC.XmlRpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RRQMBox.Server
{
    [Route("/[controller]/[action]")]
    public class Server : ControllerBase
    {
        public Server()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += this.Timer_Elapsed;
            timer.Start();
        }

        public static bool isStart;

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isStart)
            {
                this.ShowMsg($"PerformanceTest,处理{a}次");
                a = 0;
            }
        }

        private void ShowMsg(string msg)
        {
            ShowMsgMethod.Invoke(msg);
        }

        public static Action<string> ShowMsgMethod;

        private int a;

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public void PerformanceTest()
        {
            a++;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public void TestNullReturnNullParameter()
        {
            ShowMsg($"TestNullReturnNullParameter,a={a}");
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public string TestStringReturnNullParameter()
        {
            ShowMsg("TestStringReturnNullParameter");
            return "若汝棋茗";
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public int TestIntReturnNullParameter()
        {
            ShowMsg("TestIntReturnNullParameter");
            return 10;
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public void TestNullReturnStringParameter(string name)
        {
            ShowMsg($"TestNullReturnStringParameter,String:{name}");
        }

        [RRQMRPC]
        public void TestNullReturnOutStringParameter(out string name)
        {
            ShowMsg($"TestNullReturnOutStringParameter");
            name = "若汝棋茗";
        }

        [RRQMRPC]
        public string TestStringReturnOutStringParameter(out string name)
        {
            ShowMsg($"TestStringReturnOutStringParameter");
            name = "若汝棋茗";
            return name;
        }

        [RRQMRPC]
        public void TestNullReturnRefStringParameter(ref string name)
        {
            ShowMsg($"TestStringReturnOutStringParameter,String:{name}");
            name = "若汝棋茗";
        }

        [RRQMRPC]
        public void TestNullReturnOutParameters(out string name, out int age, out string occupation)
        {
            ShowMsg($"TestNullReturnOutParameters");
            name = "若汝棋茗";
            age = 23;
            occupation = "搬砖工程师";
        }

        [XmlRpc]
        [JsonRpc]
        [RRQMRPC]
        public Test02 TestClass1AndClass2(Test01 test01)
        {
            Test02 test02 = new Test02();
            return test02;
        }

        [RRQMRPC]
        public void TestGetSocketClient(string iDToken)
        {
            ISocketClient socketClient = ((TcpRPCParser)this.RPCService.RPCParsers["TcpParser"]).SocketClients[iDToken];
            socketClient.Send(Encoding.UTF8.GetBytes("若汝棋茗"));
        }

        [RRQMRPC]
        public void TestCallBack(string id)
        {
            Task.Run(() =>
            {
                try
                {
                    //先判断SocketClient是否还在线，然后回调。
                    //TcpRPCParser tcpRPCParser = ((TcpRPCParser)this.RPCService.RPCParsers["TcpParser"]);
                    //if (tcpRPCParser.Service.SocketClients.TryGetSocketClient(iDToken, out RPCSocketClient socketClient))
                    //{
                    //    string msg = socketClient.CallBack<string>(1000, invokeOption, 10);
                    //}

                    //或者这样直接调
                    string mes = ((TcpRPCParser)this.RPCService.RPCParsers["TcpParser"]).CallBack<string>(id, 1000, InvokeOption.WaitInvoke, 10);

                    ShowMsg($"TestCallBack，mes={mes}");
                }
                catch (Exception ex)
                {
                    ShowMsg($"TestCallBack调用异常,信息：{ex.Message}");
                }
            });
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public async Task<string> TestAsync()
        {
            return await Task.Run(() =>
            {
                ShowMsg("TestAsync");
                return "若汝棋茗";
            });
        }

        [XmlRpc]
        [JsonRpc]
        [Route]
        [RRQMRPC]
        public List<Test01> TestReturnList()
        {
            List<Test01> list = new List<Test01>();
            list.Add(new Test01() { Age = 1 });
            list.Add(new Test01() { Age = 2 });
            list.Add(new Test01() { Age = 3 });
            return list;
        }

        [JsonRpc]
        [RRQMRPC]
        public Dictionary<int, string> TestReturnDic()
        {
            Dictionary<int, string> valuePairs = new Dictionary<int, string>();
            valuePairs.Add(0, "0");
            valuePairs.Add(1, "1");
            valuePairs.Add(2, "2");
            valuePairs.Add(3, "3");
            return valuePairs;
        }

        [XmlRpc]
        [JsonRpc]
        [RRQMRPC]
        public void TestStringDefaultNullValue(string s = null)
        {
        }

        [XmlRpc]
        [JsonRpc]
        [RRQMRPC]
        public void TestStringDefaultValue(string s = "123123123")
        {
        }

        [XmlRpc]
        [JsonRpc]
        [RRQMRPC]
        public void TestValueDefaultValue(int a = 1234)
        {
        }

        [XmlRpc]
        [JsonRpc]
        [RRQMRPC]
        public void TestDoubleValueDefaultValue(double a = 1234.021)
        {
        }

        [XmlRpc]
        public string TestXmlRpc(string param, int a, double b, Args[] args)
        {
            ShowMsg("TestXmlRpc");
            return "若汝棋茗";
        }

        [XmlRpc]
        [JsonRpc]
        public string TestJsonRpc(int a)
        {
            ShowMsg("TestJsonRpc");
            return "若汝棋茗";
        }
    }

    public class Args
    {
        public int P1 { get; set; }
        public double P2 { get; set; }
        public string P3 { get; set; }
    }

    public class Test01
    {
        public int Age { get; set; } = 1;
        public string Name { get; set; }
    }

    public class Test02
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public List<int> list { get; set; }
        public int[] nums { get; set; }
    }

    public class Test03 : Test02
    {
        public int Length { get; set; }
    }

    public enum MyEnum
    {
        T1 = 0,
        T2 = 100,
        T3 = 200
    }
}