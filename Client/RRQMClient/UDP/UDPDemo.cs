using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMClient.UDP
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

            int receivedCount = 0;
            udpSession.Received += (remote, byteBlock) =>
            {
                receivedCount++;
            };

            UdpSessionConfig config = new UdpSessionConfig();
            config.BindIPHost = new IPHost(7790);
            udpSession.Setup(config);
            udpSession.Start();

            //EndPoint endPoint = new IPHost("127.0.0.1:7789").EndPoint;
            EndPoint endPoint = new IPHost("127.0.0.1:7789").EndPoint;

            int testCount = 10000;
            while (true)
            {
                Console.WriteLine("按任意键开始测试");
                Console.ReadKey();
                for (int i = 0; i < testCount; i++)
                {
                    udpSession.Send(endPoint, BitConverter.GetBytes(i));
                }

                Thread.Sleep(1000);

                Console.WriteLine($"已发送{testCount}条记录，收到：{receivedCount}条");
                receivedCount = 0;
            }
        }

        private static void TestUdpSession()
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();
            udpSession.Received += (remote, byteBlock) =>
            {
                Console.WriteLine($"收到：{Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len)}");
            };
            UdpSessionConfig config = new UdpSessionConfig();
            config.BindIPHost = new IPHost(7788);
            udpSession.Setup(config);
            udpSession.Start();

            while (true)
            {
                udpSession.Send(new IPHost("127.0.0.1:7789").EndPoint, Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }
    }
}
