//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.Serialization;
using RRQMProxy;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RRQMSocketXUnitTest.RPC.Tcp
{
    public class TestRRQMTcpRpc
    {
        [Fact]
        public void ShouldBeAbleToDiscoveryService()
        {
            TcpRpcClient client = new TcpRpcClient();

            client.Setup("127.0.0.1:7794");
            client.Connect("123RPC");
            MethodItem[] methodItems = client.DiscoveryService("RPC");
            Assert.NotNull(methodItems);
            Assert.True(methodItems.Length > 0);
        }

        [Fact]
        public void ShouldFailedToDiscoveryService()
        {
            TcpRpcClient client = new TcpRpcClient();

            client.Setup("127.0.0.1:7794");
            client.Connect("123RPC");

            Assert.ThrowsAny<Exception>(() =>
            {
                client.DiscoveryService("error");
            });


        }

        [Theory]
        [InlineData(SerializationType.RRQMBinary)]
        [InlineData(SerializationType.Json)]
        [InlineData(SerializationType.Xml)]
        public void ShouldSuccessfulCallService(SerializationType serializationType)
        {
            TcpRpcClient client = new TcpRpcClient();

            RpcService service = new RpcService();
            service.AddRpcParser("client", client);
            service.RegisterServer<CallbackServer>();

            client.Setup("127.0.0.1:7794");
            client.Connect("123RPC");
            MethodItem[] methodItems = client.DiscoveryService("RPC");

            Assert.NotNull(methodItems);
            Assert.True(methodItems.Length > 0);

            //此处是修改的默认调用配置参数
            //可以自己构建为每个调用设置调用
            InvokeOption.WaitInvoke.SerializationType = serializationType;

            RemoteTest remoteTest = new RemoteTest(client);
            remoteTest.Test01("rrqm");
            remoteTest.Test02();
            remoteTest.Test03();
            remoteTest.Test04();
            remoteTest.Test05();
            remoteTest.Test06();
            remoteTest.Test07();
            remoteTest.Test08();
            remoteTest.Test09();
            remoteTest.Test10();
            remoteTest.Test11();

            if (serializationType != SerializationType.Xml)
            {
                remoteTest.Test12();//xml不支持序列化字典
            }

            remoteTest.Test13();
            remoteTest.Test14();
            remoteTest.Test15();
            remoteTest.Test16();
            remoteTest.Test17();
            remoteTest.Test18();
            remoteTest.Test19(client.ID);
            remoteTest.Test22();
            remoteTest.Test25();
        }

        [Fact]
        public void ShouldSuccessfulCallCancellationToken()
        {
            TcpRpcClient client = new TcpRpcClient();
            client.Setup("127.0.0.1:7794");
            client.Connect("123RPC");
            client.DiscoveryService("RPC");

            RemoteTest remoteTest = new RemoteTest(client);
            remoteTest.Test26();
        }

        [Fact]
        public void ShouldCreateChannelAndReadWrite()
        {
            TcpRpcClient client = new TcpRpcClient();
            client.Setup("127.0.0.1:7794");
            client.Connect("123RPC");
            MethodItem[] methodItems = client.DiscoveryService("RPC");

            XUnitTestController server = new XUnitTestController(client);

            Channel channel = client.CreateChannel();
            int length = 0;
            Task.Run(() =>
            {
                while (channel.MoveNext())
                {
                    length += channel.GetCurrent().Length;
                }
            });
            server.Test28_TestChannel(channel.ID);
            Thread.Sleep(1000 * 2);
            Assert.Equal(1024 * 1024, length);
        }
    }

    public class CallbackServer : RRQMSocket.RPC.ServerProvider
    {
        [RRQMRPCCallBack()]
        public string SayHello(int age)
        {
            return $"我今年{age}岁了";
        }
    }
}