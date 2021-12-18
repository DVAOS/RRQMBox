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
using RRQMSocket;
using System;
using System.Net;
using System.Text;

namespace UdpSessionDemo
{
    class Program
    {
        static void Main(string[] args)
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

            Console.ReadKey();
        }

        static void TestUdpPerformance()
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();

            EndPoint endPoint = new IPHost("127.0.0.1:7790").EndPoint;
            udpSession.Received += (remote, byteBlock) =>
            {
                udpSession.Send(remote, byteBlock);
            };
            UdpSessionConfig config = new UdpSessionConfig();
            config.BindIPHost =new IPHost(7789);
            udpSession.Setup(config);
            udpSession.Start();

            Console.WriteLine("等待接收");
        }

        static void TestUdpSession()
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();
            udpSession.Received += (remote, byteBlock) =>
            {
                string ss = remote.ToString();
                udpSession.Send(remote, byteBlock);

                Console.WriteLine(ss);
                //Console.WriteLine($"收到：{Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len)}");
            };
            UdpSessionConfig config = new UdpSessionConfig();
            config.BindIPHost = new IPHost(7789);
            udpSession.Setup(config);
            udpSession.Start();

            Console.ReadKey();
        }

    }
}
