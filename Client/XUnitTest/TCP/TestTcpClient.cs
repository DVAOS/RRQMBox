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
using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Threading;
using Xunit;

namespace RRQMSocketXUnitTest.TCP
{
    public class TestTcpClient
    {
        [Fact]
        public void ShouldCanConnectAndReceive()
        {
            int waitTime = 100;
            SimpleTcpClient client = new SimpleTcpClient();

            bool connected = false;
            int disconnectCount = 0;
            client.Connected += (client, e) =>
            {
                connected = true;
            };
            client.Disconnected += (client, e) =>
            {
                disconnectCount++;
                connected = false;
            };

            int receivedCount = 0;
            client.Received += (tcpClient, arg1, arg2) =>
            {
                receivedCount++;
            };

            var config = new TcpClientConfig();
            config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7789"))//远程IPHost
                .SetValue(TcpClientConfig.SeparateThreadSendProperty, false);//独立线程发送;

            client.Setup(config);//载入配置
            client.Connect();//连接
            Thread.Sleep(waitTime);

            Assert.True(client.Online);
            Assert.Equal("127.0.0.1", client.IP);
            Assert.Equal(7789, client.Port);
            Assert.True(connected);

            client.Send(BitConverter.GetBytes(1));
            Thread.Sleep(waitTime);
            Assert.Equal(1, receivedCount);

            client.Send(BitConverter.GetBytes(2));
            Thread.Sleep(waitTime);
            Assert.Equal(2, receivedCount);

            client.Disconnect();
            Thread.Sleep(waitTime);
            Assert.True(!client.Online);
            Assert.True(!connected);
            Assert.Equal(1, disconnectCount);

            client.Connect();
            Thread.Sleep(waitTime);
            Assert.True(client.Online);
            Assert.Equal("127.0.0.1", client.IP);
            Assert.Equal(7789, client.Port);
            Assert.True(connected);

            client.Send(BitConverter.GetBytes(3));
            Thread.Sleep(waitTime);
            Assert.Equal(3, receivedCount);

            client.Send(BitConverter.GetBytes(4));
            Thread.Sleep(waitTime);
            Assert.Equal(4, receivedCount);

            client.Dispose();
            Thread.Sleep(waitTime);
            Assert.True(!client.Online);
            Assert.True(!connected);
            Assert.Equal(2, disconnectCount);

            Assert.ThrowsAny<Exception>(() =>
            {
                client.Connect();
            });

            Thread.Sleep(1000);
            Assert.Equal(2, disconnectCount);
        }
    }
}