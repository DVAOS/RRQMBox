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
using System;
using System.Collections.Generic;
using System.Text;
using RRQMRPC.RRQMTest;
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;

namespace Demo.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ReadKey();

            BinarySerialize();
            UDPBinarySerialize();
            XmlSerialize();
            Console.ReadKey();
        }

        private static void BinarySerialize()
        {
            //序列化默认为二进制

            RPCClient client = new RPCClient();
            client.ReceivedByteBlock += Client_ReceivedByteBlock;

            //开启反向RPC，先注册
            client.RegistService(new CallBackServer());
            client.OpenCallBackRPCServer();

            TypeInitializeDic pairs = new TypeInitializeDic();
            pairs.Add("List<RRQMRPC.RRQMTest.Test01>", typeof(List<Test01>));
            pairs.Add("Dictionary<System.Int32,System.String>", typeof(Dictionary<int, string>));

            //无法找到的类型，通过TypeInitializeDic显式指定

            client.InitializedRPC(new IPHost("127.0.0.1:7789"), typeDic: pairs);
            Console.WriteLine();
            Console.WriteLine("二进制连接成功");

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.CanFeedback);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            remoteTest.Test11(client.ID);
            remoteTest.Test12();
            remoteTest.Test13();
            remoteTest.Test14();
            remoteTest.Test15(client.ID);//调用服务，然后让服务再回调RPC

            Console.WriteLine("二进制测试完成");
            Console.WriteLine();
        }

        private static void UDPBinarySerialize()
        {
            //UDP序列化默认为二进制

            UdpRPCClient client = new UdpRPCClient();
            client.Bind(8848, 1);
            TypeInitializeDic pairs = new TypeInitializeDic();
            pairs.Add("List<RRQMRPC.RRQMTest.Test01>", typeof(List<Test01>));
            pairs.Add("Dictionary<System.Int32,System.String>", typeof(Dictionary<int, string>));

            client.InitializedRPC(new IPHost("127.0.0.1:7790"), typeDic: pairs);
            Console.WriteLine();
            Console.WriteLine("二进制连接成功");

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.NoFeedback);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            Console.WriteLine("UDP二进制测试完成");
            Console.WriteLine();
        }

        private static void Client_ReceivedByteBlock(object sender, RRQMCore.ByteManager.ByteBlock e)
        {
            Console.WriteLine($"收到独立消息：{Encoding.UTF8.GetString(e.Buffer, 0, (int)e.Length)}");
        }

        private static void XmlSerialize()
        {
            RPCClient client = new RPCClient();
            client.SerializeConverter = new XmlSerializeConverter();

            TypeInitializeDic pairs = new TypeInitializeDic();
            pairs.Add("List<RRQMRPC.RRQMTest.Test01>", typeof(List<Test01>));
            pairs.Add("Dictionary<System.Int32,System.String>", typeof(Dictionary<int, string>));

            client.InitializedRPC(new IPHost("127.0.0.1:7791"), typeDic: pairs);
            Console.WriteLine();
            Console.WriteLine("Xml连接成功");

            RemoteTest remoteTest = new RemoteTest(client);

            remoteTest.Test01(InvokeOption.CanFeedback);
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            remoteTest.Test12();
            remoteTest.Test14();

            Console.WriteLine("Xml测试完成");
            Console.WriteLine();
        }
    }
}