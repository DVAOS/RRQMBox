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
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using Xunit;

namespace RRQMSocketXUnitTest.RPC.Udp
{
    public class TestRRQMUdpRpc
    {
        [Theory]
        [InlineData("127.0.0.1:7797", 8848)]
        public void ShouldSuccessfulCallService(string ipHost, int port)
        {
            UdpRpc client = new UdpRpc();
            var config = new UdpRpcClientConfig();
            config.RemoteIPHost = new IPHost(ipHost);
            config.BindIPHost = new IPHost(port);
           
            client.Setup(config);
            client.Start();
            MethodItem[] methodItems = client.DiscoveryService("RPC");

            Assert.NotNull(methodItems);
            Assert.True(methodItems.Length > 0);

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

            if (new IPHost(ipHost).Port != 7799)
            {
                remoteTest.Test12();
            }

            remoteTest.Test13();
            remoteTest.Test14();
            remoteTest.Test15();
            remoteTest.Test16();
            remoteTest.Test17();
            remoteTest.Test18();
            remoteTest.Test22();
        }
    }
}