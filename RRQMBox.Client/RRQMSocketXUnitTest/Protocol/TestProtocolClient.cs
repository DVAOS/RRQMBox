using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace RRQMSocketXUnitTest.Protocol
{
    public class TestProtocolClient
    {
        [Fact]
        public void ShouldCanReceiveAndResetID()
        {
            int waitTime = 100;
            SimpleProtocolClient client = new SimpleProtocolClient();

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
            string newID = null;
            client.Received += (short? arg1, ByteBlock arg2) =>
            {
                if (arg1 == 10)
                {
                    newID = Encoding.UTF8.GetString(arg2.Buffer, 2, arg2.Len - 2);
                }
                else
                {
                    receivedCount++;
                }

            };

            var config = new TokenClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7793");
            config.VerifyToken = "XUnitTest";
            config.VerifyTimeout = 3;

            client.Setup(config);//载入配置
            client.Connect();//连接
            Thread.Sleep(waitTime);

            Assert.True(client.Online);
            Assert.Equal("127.0.0.1", client.IP);
            Assert.Equal(7793, client.Port);
            Assert.True(connected);

            string id = Guid.NewGuid().ToString();
            client.ResetID(id);
            Assert.Equal(id, client.ID);
            client.Send(10);
            Thread.Sleep(waitTime);

            Assert.Equal(newID, client.ID);

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
            Assert.Equal(7793, client.Port);
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
