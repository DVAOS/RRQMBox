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
using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Net;
using Xunit;

namespace RRQMSocketXUnitTest.UDP
{
    public class TestUdpService
    {
        [Fact]
        public void ShouldShowProperties()
        {
            UdpSession udpSession = new UdpSession();
            Assert.Equal(ServerState.None, udpSession.ServerState);

            var config = new RRQMConfig();//UDP配置
            config.SetRemoteIPHost(new IPHost("127.0.0.1:10086"))
                .SetBindIPHost(new IPHost("127.0.0.1:10087"))
                .SetServerName("RRQMUdpServer")
                .SetBufferLength(2048);

            udpSession.Setup(config);//加载配置
            udpSession.Start();//启动

            Assert.Equal(ServerState.Running, udpSession.ServerState);
            Assert.Equal("RRQMUdpServer", udpSession.ServerName);
            Assert.Equal(2048, udpSession.BufferLength);
            Assert.Equal("127.0.0.1:10086", udpSession.RemoteIPHost.ToString());

            udpSession.Stop();
            Assert.Equal(ServerState.Stopped, udpSession.ServerState);

            udpSession.Start();
            Assert.Equal(ServerState.Running, udpSession.ServerState);
            Assert.Equal("RRQMUdpServer", udpSession.ServerName);
            Assert.Equal(2048, udpSession.BufferLength);
            Assert.Equal("127.0.0.1:10086", udpSession.RemoteIPHost.ToString());

            udpSession.Dispose();
            Assert.Equal(ServerState.Disposed, udpSession.ServerState);

            Assert.ThrowsAny<Exception>(() =>
            {
                udpSession.Start();
            });
        }

        [Fact]
        public void ShouldCanSendAndReceive()
        {
            UdpSession udpSession = new UdpSession();

            int count = 0;
            udpSession.Received += (endpoint, byteBlock,requestInfo) =>
            {
                count++;
                Assert.Equal(count, BitConverter.ToInt32(byteBlock.Buffer, 0));
            };

            udpSession.Setup(new RRQMConfig()//加载配置
                .SetRemoteIPHost(new IPHost("127.0.0.1:7790"))
                .SetBindIPHost(new IPHost("127.0.0.1:7791")))
                .Start();

            udpSession.Send(BitConverter.GetBytes(1));

            byte[] data_2 = BitConverter.GetBytes(2);
            udpSession.Send(data_2, 0, data_2.Length);

            ByteBlock byteBlock = BytePool.GetByteBlock(4);
            byteBlock.Write(BitConverter.GetBytes(3));
            udpSession.Send(byteBlock);
            byteBlock.Dispose();

            udpSession.SendAsync(BitConverter.GetBytes(4));
            byte[] data_5 = BitConverter.GetBytes(5);
            udpSession.Send(new IPHost($"127.0.0.1:7790").EndPoint, data_5, 0, data_5.Length);
        }
    }
}