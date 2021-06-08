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
using System;
using System.Collections.Generic;
using RRQMCore.Diagnostics;
using RRQMRPC.RRQMTest;
using RRQMSocket.RPC.RRQMRPC;

namespace RRQMBox.Client.RPCTest
{
    public class RemoteTest
    {
        public RemoteTest(IRPCClient client)
        {
            server = new Server(client);
        }
        private void ShowMsg(string msg)
        {
            ShowMsgMethod.Invoke(msg);
        }
        public static Action<string> ShowMsgMethod;

        private Server server;

        /// <summary>
        /// 测试无返回值无参数
        /// </summary>
        public void Test01(InvokeOption invokeOption)
        {
            server.TestNullReturnNullParameter(invokeOption);

            ShowMsg("Test01=>测试完成");
        }

        /// <summary>
        /// 测试无返回值多个Out类型参数
        /// </summary>
        public void Test02()
        {
            string name;
            int age;
            string occupation;
            server.TestNullReturnOutParameters(out name, out age, out occupation);
            ShowMsg($"Test02=>TestNullReturnOutParameters完成,name={name},age={age},occupation={occupation}");
        }

        /// <summary>
        /// 测试无返回值单个out类型参数
        /// </summary>
        public void Test03()
        {
            string name;
            server.TestNullReturnOutStringParameter(out name);
            ShowMsg($"Test03=>TestNullReturnOutStringParameter完成,name={name}");
        }

        /// <summary>
        /// 测试无返回值，ref类型参数
        /// </summary>
        public void Test04()
        {
            string name = "张三";
            server.TestNullReturnRefStringParameter(ref name);
            ShowMsg($"Test04=>TestNullReturnRefStringParameter完成,name={name}");
        }

        /// <summary>
        /// 测试无返回值，String类型参数
        /// </summary>
        public void Test05()
        {
            string name = "张三";
            server.TestNullReturnStringParameter(name);
            ShowMsg($"Test05=>TestNullReturnStringParameter完成");
        }

        /// <summary>
        /// 测试String返回值，无参数
        /// </summary>
        public void Test06()
        {
            string s = server.TestStringReturnNullParameter();
            ShowMsg($"Test06=>TestStringReturnNullParameter完成,Return={s}");
        }

        /// <summary>
        /// 测试String返回值，out 类型参数
        /// </summary>
        public void Test07()
        {
            string name;
            string s = server.TestStringReturnOutStringParameter(out name);
            ShowMsg($"Test07=>TestStringReturnOutStringParameter完成,Return={s},name={name}");
        }

        /// <summary>
        /// 测试预设
        /// </summary>
        public void Test08()
        {
            server.TestDoubleValueDefaultValue();
            ShowMsg($"Test08=>TestDoubleValueDefaultValue完成");
        }

        public void Test09()
        {
            Test01 test01 = new Test01() { Age = 10, Name = "若汝棋茗" };
            Test02 test02 = server.TestClass1AndClass2(test01);
            ShowMsg($"Test09=>TestClass1AndClass2完成");
        }

        public void Test11(string iDToken)
        {
            server.TestGetSocketClient(iDToken);
            ShowMsg($"Test11=>TestGetSocketClient完成");
        }

        public void Test12()
        {
            List<Test01> tests = server.TestReturnList();
            ShowMsg($"Test12=>TestReturnList完成,长度={tests.Count}");
        }

        public void Test13()
        {
            Dictionary<int, string> tests = server.TestReturnDic();
            ShowMsg($"Test13=>TestReturnDic完成,长度={tests.Count}");
        }

        public void Test14()
        {
            string mes = server.TestAsync();
            ShowMsg($"Test14=>TestAsync完成,mes={mes}");
        }

        public void Test15(string iDToekn)
        {
            server.TestCallBack(iDToekn);
            ShowMsg($"Test15=>TestCallBack完成");
        }
    }
}