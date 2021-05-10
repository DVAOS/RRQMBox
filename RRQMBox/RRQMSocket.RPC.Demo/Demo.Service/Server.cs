//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using RRQMSocket.RPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Demo.Service
{
    /*
    若汝棋茗
    */

    public class Server : ServerProvider
    {
        private int a;

        [RRQMRPCMethod]
        public void TestNullReturnNullParameter()
        {
            if (++a % 1000 == 0)
            {
                Console.WriteLine($"TestNullReturnNullParameter,a={a}");
            }
        }

        [RRQMRPCMethod]
        public string TestStringReturnNullParameter()
        {
            Console.WriteLine("TestStringReturnNullParameter");
            return "若汝棋茗";
        }

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
        public void TestClass1(Test01 test01)
        {
        }

        [RRQMRPCMethod]
        public Test02 TestClass1AndClass2(Test01 test01)
        {
            Test02 test02 = new Test02();
            return test02;
        }

        [RRQMRPCMethod]
        public void TestClass3(Test03 test03)
        {
        }

        [RRQMRPCMethod]
        public void TestGetSocketClient(string iDToken)
        {
            ISocketClient socketClient = ((TcpRPCParser)this.RPCService.RPCParsers["TcpParser"]).SocketClients[iDToken];
            socketClient.Send(Encoding.UTF8.GetBytes("若汝棋茗"));
        }

        [RRQMRPCMethod]
        public void TestSleep()
        {
            Thread.Sleep(10 * 1000);
        }

        //[RRQMRPCMethod]
        //public List<int> TestReturnList()
        //{
        //    List<int> list = new List<int>();
        //    list.Add(1);
        //    list.Add(2);
        //    list.Add(3);
        //    list.Add(4);
        //    return list;
        //}

    }
}