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
using RRQMCore.Serialization;
using RRQMProxy;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace RRQMSocketXUnitTest.RPC
{
    public class RemoteTest
    {
        private IXUnitTestServer server;

        public RemoteTest(IRpcClient client)
        {
            server = new XUnitTestServer(client);
        }

        public void Test01(string invokeType)
        {
            if (invokeType == "rrqm")
            {
                server.Test01_Performance();
            }
            else if (invokeType == "xml")
            {
                server.Xml_Test01_Performance();
            }
            else if (invokeType == "json")
            {
                server.Json_Test01_Performance();
            }
        }

        public void Test02()
        {
            for (int i = 0; i < 10; i++)
            {
                string returnValue = server.Test02_TaskString(i.ToString());
                Assert.Equal(i.ToString(), returnValue);
            }
        }

        public void Test03()
        {
            ProxyClass1 proxyClass1 = server.Test03_GetProxyClass();
            Assert.Equal(10, proxyClass1.P1);
            Assert.Equal(100, proxyClass1.P2.P1);
            Assert.Equal(1000, proxyClass1.P2.P2.P1);
        }

        public void Test04()
        {
            int value = server.Test04_In32DefaultValue();
            Assert.Equal(100, value);

            value = server.Test04_In32DefaultValue(int.MaxValue);
            Assert.Equal(int.MaxValue, value);
        }

        public void Test05()
        {
            server.Test05_NoneReturnNoneParameter();
        }

        public void Test06()
        {
            string name;
            int age;
            string occupation;
            server.Test06_OutParameters(out name, out age, out occupation);
            Assert.Equal("若汝棋茗", name);
            Assert.Equal(18, age);
            Assert.Equal("搬砖工程师", occupation);
        }

        public void Test07()
        {
            string name;
            server.Test07_OutStringParameter(out name);
            Assert.Equal("若汝棋茗", name);
        }

        public void Test08()
        {
            string name = "若汝棋茗";
            server.Test08_RefStringParameter(ref name);
            Assert.Equal("若汝棋茗ref", name);
        }

        public void Test09()
        {
            Assert.True(server.Test09_Boolean(true));
            Assert.False(server.Test09_Boolean(false));
        }

        public void Test10()
        {
            Assert.Null(server.Test10_StringDefaultNullValue());
        }

        public void Test11()
        {
            Assert.Equal("RRQM", server.Test11_StringDefaultValue());
        }

        public void Test12()
        {
            for (int i = 1; i < 10; i++)
            {
                Dictionary<int, string> dic = server.Test12_Dictionary(i);
                Assert.Equal(i, dic.Count);
                for (int j = 0; j < dic.Count; j++)
                {
                    Assert.True(dic.ContainsKey(j));
                    Assert.Equal(j.ToString(), dic[j]);
                }
            }
        }

        public void Test13()
        {
            server.Test13_Task();
        }

        public void Test14()
        {
            for (int i = 1; i < 10; i++)
            {
                List<Class01> list = server.Test14_ListClass01(i);
                Assert.True(list.Count == i);
                for (int j = 0; j < list.Count; j++)
                {
                    Assert.Equal(j, list[j].Age);
                }
            }
        }

        public void Test15()
        {
            Args args = server.Test15_ReturnArgs();
            Assert.NotNull(args);
            Assert.Equal(10, args.P1);
            Assert.Equal(10.0, args.P2);
            Assert.Equal("RRQM", args.P3);
        }

        public void Test16()
        {
            Class04 class04 = server.Test16_ReturnClass4(10, "RRQM");
            Assert.NotNull(class04);
            Assert.Equal(10, class04.P1);
            Assert.Equal("RRQM", class04.P2);
            Assert.Equal(10, class04.P3);
        }

        public void Test17()
        {
            Assert.Equal(3.1415926, server.Test17_DoubleDefaultValue());
            Assert.Equal(10.0, server.Test17_DoubleDefaultValue(10.0));
        }

        public void Test18()
        {
            Class01 class01 = server.Test18_Class1(new Class01() { Age = 10, Name = "RRQM" });
            Assert.NotNull(class01);
            Assert.Equal(10, class01.Age);
            Assert.Equal("RRQM", class01.Name);
        }

        public void Test19(string id)
        {
            for (int i = 0; i < 10; i++)
            {
                string msg = server.Test19_CallBack(id, i);
                Assert.Equal($"我今年{i}岁了",msg);
            }
           
        }

        public void Test22()
        {
            int value = server.Test22_IncludeCaller(10);
            Assert.Equal(10, value);
        }

        public int Test23(InvokeType invokeType)
        {
            InvokeOption invokeOption = new InvokeOption();
            invokeOption.InvokeType = invokeType;
            invokeOption.SerializationType = SerializationType.Json;
            invokeOption.FeedbackType = FeedbackType.WaitInvoke;

            return server.Test23_InvokeType(invokeOption);
        }

        public void Test25()
        {
            int result = server.Test25_TestStruct(new StructArgs() { P1 = 10 });
            Assert.Equal(10, result);
        }

        public void Test26()
        {
            InvokeOption invokeOption1 = new InvokeOption()
            {
                InvokeType = InvokeType.GlobalInstance,
                FeedbackType = FeedbackType.WaitInvoke,
                SerializationType = SerializationType.RRQMBinary,
                Timeout = 20 * 1000
            };

            int result = server.Test26_TestCancellationToken(invokeOption1);
            Assert.Equal(1, result);

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            InvokeOption invokeOption2 = new InvokeOption()
            {
                CancellationToken = tokenSource.Token,
                InvokeType = InvokeType.GlobalInstance,
                FeedbackType = FeedbackType.WaitInvoke,
                SerializationType = SerializationType.RRQMBinary,
                Timeout = 20 * 1000
            };

            RRQMCore.Run.EasyAction.DelayRun(2, () =>
             {
                 tokenSource.Cancel();
             });
            int result2 = server.Test26_TestCancellationToken(invokeOption2);
            Assert.Equal(0, result2);
        }
    }
}