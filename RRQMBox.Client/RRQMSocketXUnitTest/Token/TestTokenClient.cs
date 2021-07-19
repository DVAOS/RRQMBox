using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace RRQMSocketXUnitTest.Token
{
    public class TestTokenClient
    {
        [Fact]
        public void ShouldCanConnectAndReceive()
        {
            int waitTime = 100;
            SimpleTokenClient client = new SimpleTokenClient();

            bool connected = false;
            int disconnectCount = 0;
            client.ConnectedService += (object sender, MesEventArgs e) =>
            {
                connected = true;
            };
            client.DisconnectedService += (object sender, MesEventArgs e) =>
            {
                disconnectCount++;
                connected = false;
            };

            int receivedCount = 0;
            client.Received += (ByteBlock arg1, object arg2) =>
            {
                receivedCount++;
            };

            var config = new TokenClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7792");
            config.VerifyToken = "XUnitTest";
            config.VerifyTimeout = 3;

            client.Setup(config);//载入配置
            client.Connect();//连接
            Thread.Sleep(waitTime);

            Assert.True(client.Online);
            Assert.Equal("127.0.0.1", client.IP);
            Assert.Equal(7792, client.Port);
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
            Assert.Equal(7792, client.Port);
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

            Thread.Sleep(waitTime);
            Assert.Equal(2, disconnectCount);
        }

        [Fact]
        public void ShouldCanNotConnect()
        {
            SimpleTokenClient client = new SimpleTokenClient();
            var config = new TcpClientConfig();
            config.SetValue(TcpClientConfig.RemoteIPHostProperty, new IPHost("127.0.0.1:7792"))//连接地址
                .SetValue(TokenClientConfig.VerifyTokenProperty, "Error")//连接验证令箭
                .SetValue(TokenClientConfig.VerifyTimeoutProperty, 3);//验证等待时间

            client.Setup(config);
            Assert.Throws<RRQMTokenVerifyException>(() =>
            {
                client.Connect();
            });
        }
    }
}
