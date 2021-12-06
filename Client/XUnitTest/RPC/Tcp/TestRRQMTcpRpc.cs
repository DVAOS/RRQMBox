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
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RRQMSocketXUnitTest.RPC.Tcp
{
    public class TestRRQMTcpRpc
    {
        [Fact]
        public void ShouldBeAbleToDiscoveryService()
        {
            TcpRpcClient client = new TcpRpcClient();
            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7794");
            config.ProxyToken = "RPC";

            client.Setup(config);
            client.Connect("123RPC");//此步骤可以省略
            MethodItem[] methodItems = client.DiscoveryService();

            Assert.NotNull(methodItems);
            Assert.True(methodItems.Length > 0);
        }

        [Fact]
        public void ShouldFailedToDiscoveryService()
        {
            TcpRpcClient client = new TcpRpcClient();
            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7794");
            config.ProxyToken = "error";

            client.Setup(config);
            client.Connect("123RPC");//此步骤可以省略
            MethodItem[] methodItems = client.DiscoveryService();

            Assert.NotNull(methodItems);
            Assert.True(methodItems.Length == 0);
        }

        [Theory]
        [InlineData(SerializationType.RRQMBinary)]
        [InlineData(SerializationType.Json)]
        [InlineData(SerializationType.Xml)]
        public void ShouldSuccessfulCallService(SerializationType serializationType)
        {
            TcpRpcClient client = new TcpRpcClient();
            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7794");
            config.ProxyToken = "RPC";

            client.Setup(config);
            client.Connect("123RPC");//此步骤可以省略
            MethodItem[] methodItems = client.DiscoveryService();

            Assert.NotNull(methodItems);
            Assert.True(methodItems.Length > 0);

            //此处是修改的默认调用配置参数
            //可以自己构建为每个调用设置调用
            InvokeOption.WaitInvoke.SerializationType = serializationType;

            RemoteTest remoteTest = new RemoteTest(client);
            remoteTest.Test01();
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
            remoteTest.Test22();
            remoteTest.Test25();
        }

        [Fact]
        public void ShouldSuccessfulCallGlobalInstance()
        {
            for (int i = 0; i < 5; i++)
            {
                TcpRpcClient client = new TcpRpcClient();
                var config = new TcpRpcClientConfig();
                config.RemoteIPHost = new IPHost("127.0.0.1:7794");
                config.ProxyToken = "RPC";

                client.Setup(config);
                client.Connect("123RPC");//此步骤可以省略
                client.DiscoveryService();

                RemoteTest remoteTest = new RemoteTest(client);
                int value = remoteTest.Test23(RRQMSocket.RPC.InvokeType.GlobalInstance);
                Assert.Equal(i, value);
                client.Dispose();
            }

        }

        [Fact]
        public void ShouldSuccessfulCallCustomInstance()
        {
            for (int i = 0; i < 5; i++)
            {
                TcpRpcClient client = new TcpRpcClient();
                var config = new TcpRpcClientConfig();
                config.RemoteIPHost = new IPHost("127.0.0.1:7794");
                config.ProxyToken = "RPC";

                client.Setup(config);
                client.Connect("123RPC");//此步骤可以省略
                client.DiscoveryService();

                RemoteTest remoteTest = new RemoteTest(client);
                for (int j = 0; j < 10; j++)
                {
                    int value = remoteTest.Test23(RRQMSocket.RPC.InvokeType.CustomInstance);
                    Assert.Equal(j, value);
                }
               
                client.Dispose();
            }

        }

        [Fact]
        public void ShouldSuccessfulCallNewInstance()
        {
            for (int i = 0; i < 5; i++)
            {
                TcpRpcClient client = new TcpRpcClient();
                var config = new TcpRpcClientConfig();
                config.RemoteIPHost = new IPHost("127.0.0.1:7794");
                config.ProxyToken = "RPC";

                client.Setup(config);
                client.Connect("123RPC");//此步骤可以省略
                client.DiscoveryService();

                RemoteTest remoteTest = new RemoteTest(client);
                for (int j = 0; j < 10; j++)
                {
                    int value = remoteTest.Test23(RRQMSocket.RPC.InvokeType.NewInstance);
                    Assert.Equal(0, value);
                }

                client.Dispose();
            }

        }

        [Fact]
        public void ShouldSuccessfulCallCancellationToken()
        {
            TcpRpcClient client = new TcpRpcClient();
            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7794");
            config.ProxyToken = "RPC";

            client.Setup(config);
            client.Connect("123RPC");//此步骤可以省略
            client.DiscoveryService();

            RemoteTest remoteTest = new RemoteTest(client);
            remoteTest.Test26();
        }
    }
}
