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
using RRQMSocket.RPC.RRQMRPC;
using RRQMSocket.RPC.WebApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Service
{
    /*
    若汝棋茗
    */
    [Route("/[controller]/[action]")]
    public class Server : ControllerBase
    {
        private int a;

        [Route]
        [RRQMRPCMethod]
        public void TestNullReturnNullParameter()
        {
            if (++a % 1000 == 0)
            {
                Console.WriteLine($"TestNullReturnNullParameter,a={a}");
            }
        }

        [Route]
        [RRQMRPCMethod]
        public string TestStringReturnNullParameter()
        {
            Console.WriteLine("TestStringReturnNullParameter");
            return "若汝棋茗";
        }

        [Route]
        [RRQMRPCMethod]
        public void TestNullReturnStringParameter(string name)
        {
            Console.WriteLine($"TestNullReturnStringParameter,String:{name}");
        }

        [RRQMRPCMethod]
        public void TestNullReturnOutStringParameter(out string name)
        {
            Console.WriteLine($"TestNullReturnOutStringParameter");
            name = "若汝棋茗";
        }

        [RRQMRPCMethod]
        public string TestStringReturnOutStringParameter(out string name)
        {
            Console.WriteLine($"TestStringReturnOutStringParameter");
            name = "若汝棋茗";
            return name;
        }

        [RRQMRPCMethod]
        public void TestNullReturnRefStringParameter(ref string name)
        {
            Console.WriteLine($"TestStringReturnOutStringParameter,String:{name}");
            name = "若汝棋茗";
        }

        [RRQMRPCMethod]
        public void TestNullReturnOutParameters(out string name, out int age, out string occupation)
        {
            Console.WriteLine($"TestNullReturnOutParameters");
            name = "若汝棋茗";
            age = 23;
            occupation = "搬砖工程师";
        }

        [RRQMRPCMethod]
        public Test02 TestClass1AndClass2(Test01 test01)
        {
            Test02 test02 = new Test02();
            return test02;
        }

        [RRQMRPCMethod]
        public void TestGetSocketClient(string iDToken)
        {
            ISocketClient socketClient = ((TcpRPCParser)this.RPCService.RPCParsers["TcpParser"]).Service.SocketClients[iDToken];
            socketClient.Send(Encoding.UTF8.GetBytes("若汝棋茗"));
        }
        [RRQMRPCMethod]
        public void TestCallBack(string iDToken)
        {
            Task.Run(() =>
            {
                InvokeOption invokeOption = new InvokeOption();
                invokeOption.Feedback = true;
                invokeOption.WaitTime = 100;
                try
                {
                    string mes = ((TcpRPCParser)this.RPCService.RPCParsers["TcpParser"]).CallBack<string>(iDToken, 1000, invokeOption, 10);

                    Console.WriteLine($"TestCallBack，mes={mes}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"TestCallBack调用异常,信息：{ex.Message}");
                }

            });

        }

        [Route]
        [RRQMRPCMethod]
        public async Task<string> TestAsync()
        {
            return await Task.Run(() =>
            {
                Console.WriteLine(nameof(TestAsync));
                return "若汝棋茗";
            });
        }

        [Route]
        [RRQMRPCMethod]
        public List<Test01> TestReturnList()
        {
            List<Test01> list = new List<Test01>();
            list.Add(new Test01() { Age = 1 });
            list.Add(new Test01() { Age = 2 });
            list.Add(new Test01() { Age = 3 });
            return list;
        }

        [RRQMRPCMethod]
        public Dictionary<int, string> TestReturnDic()
        {
            Dictionary<int, string> valuePairs = new Dictionary<int, string>();
            valuePairs.Add(0, "0");
            valuePairs.Add(1, "1");
            valuePairs.Add(2, "2");
            valuePairs.Add(3, "3");
            return valuePairs;
        }

        [RRQMRPCMethod]
        public void TestStringDefaultNullValue(string s = null)
        {

        }
        [RRQMRPCMethod]
        public void TestStringDefaultValue(string s = "123123123")
        {

        }
        [RRQMRPCMethod]
        public void TestValueDefaultValue(int a = 1234)
        {

        }

        [RRQMRPCMethod]
        public void TestDoubleValueDefaultValue(double a = 1234.021)
        {

        }
    }
}