using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace RRQMSocketXUnitTest.UDP
{
    public class TestUdpService
    {
        [Fact]
        public void ShouldShowProperties()
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();
            Assert.Equal(ServerState.None, udpSession.ServerState);

            var config = new UdpSessionConfig();//UDP配置
            config.SetValue(UdpSessionConfig.DefaultRemotePointProperty, new IPHost("127.0.0.1:10086").EndPoint)//设置默认终结点，用于发送
            .SetValue(UdpSessionConfig.UseBindProperty, true)//是否执行绑定，一般作为接收端时需要绑定
            .SetValue(UdpSessionConfig.ListenIPHostsProperty, new IPHost[] { new IPHost("127.0.0.1:10087") })//绑定的IPHost，udp只能绑定一个地址。
            .SetValue(UdpSessionConfig.BufferLengthProperty,2048)//设置缓存
            .SetValue(UdpSessionConfig.ServerNameProperty,"RRQMUdpServer");//设置服务名称


            udpSession.Setup(config);//加载配置
            udpSession.Start();//启动

            Assert.Equal(ServerState.Running, udpSession.ServerState);
            Assert.Equal("RRQMUdpServer",udpSession.ServerName);
            Assert.Equal(2048,udpSession.BufferLength);
            Assert.Equal("127.0.0.1:10086", udpSession.DefaultRemotePoint.ToString());
            Assert.Equal("127.0.0.1:10087", udpSession.Name);

            udpSession.Stop();
            Assert.Equal(ServerState.Stopped, udpSession.ServerState);

            udpSession.Start();
            Assert.Equal(ServerState.Running, udpSession.ServerState);
            Assert.Equal("RRQMUdpServer", udpSession.ServerName);
            Assert.Equal(2048, udpSession.BufferLength);
            Assert.Equal("127.0.0.1:10086", udpSession.DefaultRemotePoint.ToString());
            Assert.Equal("127.0.0.1:10087", udpSession.Name);

            udpSession.Dispose();
            Assert.Equal(ServerState.Disposed, udpSession.ServerState);

            Assert.ThrowsAny<Exception>(()=> 
            {
                udpSession.Start();
            });
        }

        [Fact]
        public void ShouldCanSendAndReceive()
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();

            int count = 0 ;
            udpSession.Received += (EndPoint endpoint, ByteBlock e) =>
            {
                count++;
                Assert.Equal(count,BitConverter.ToInt32(e.Buffer,0));
            };
            var config = new UdpSessionConfig();//UDP配置
            config.SetValue(UdpSessionConfig.DefaultRemotePointProperty, new IPHost($"127.0.0.1:7790").EndPoint)//设置默认终结点，用于发送
            .SetValue(UdpSessionConfig.UseBindProperty, true)//是否执行绑定，一般作为接收端时需要绑定
            .SetValue(UdpSessionConfig.ListenIPHostsProperty, new IPHost[] { new IPHost($"127.0.0.1:7791") })//绑定的IPHost，udp只能绑定一个地址。
            .SetValue(UdpSessionConfig.BufferLengthProperty, 2048)//设置缓存
            .SetValue(UdpSessionConfig.ServerNameProperty, "RRQMUdpServer");//设置服务名称

            udpSession.Setup(config);//加载配置
            udpSession.Start();//启动

            udpSession.Send(BitConverter.GetBytes(1));

            byte[] data_2 = BitConverter.GetBytes(2);
            udpSession.Send(data_2,0,data_2.Length);

            ByteBlock byteBlock = BytePool.Default.GetByteBlock(4);
            byteBlock.Write(BitConverter.GetBytes(3));
            udpSession.Send(byteBlock);
            byteBlock.Dispose();

            udpSession.SendAsync(BitConverter.GetBytes(4));
            byte[] data_5 = BitConverter.GetBytes(5);
            udpSession.SendTo(data_5,0,data_5.Length, new IPHost($"127.0.0.1:7790").EndPoint);

        }
    }
}
