using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.UDP
{
    public static class UDPDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.简单udp测试");
            Console.WriteLine("2.udp性能测试");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestUdpSession();
                        break;
                    }
                case "2":
                    {
                        TestUdpPerformance();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void TestUdpPerformance()
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();

            udpSession.Received += (remote, byteBlock) =>
            {
                udpSession.Send(remote, byteBlock);
            };
            UdpSessionConfig config = new UdpSessionConfig();
            config.BindIPHost = new IPHost(7789);
            udpSession.Setup(config);
            udpSession.Start();

            Console.WriteLine("等待接收");
        }

        private static void TestUdpSession()
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();
            udpSession.Received += (remote, byteBlock) =>
            {
                udpSession.Send(remote, byteBlock);
                Console.WriteLine($"收到：{Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len)}");
            };
            UdpSessionConfig config = new UdpSessionConfig();
            config.BindIPHost = new IPHost(7789);
            udpSession.Setup(config);
            udpSession.Start();
            Console.WriteLine("等待接收");
        }
    }
}
