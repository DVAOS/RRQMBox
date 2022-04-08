using RRQMCore.ByteManager;
using RRQMSocket;
using RRQMSocket.Plugins;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace UdpServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpSession udpSession = new UdpSession();
            udpSession.SetDataHandlingAdapter(new UdpPackageAdapter());
            udpSession.Received += (remote, byteBlock, requestInfo) =>
            {
                udpSession.Send(remote, byteBlock);
                Console.WriteLine($"收到：{byteBlock.Len}");
            };
            udpSession.Setup(new RRQMConfig()
                .SetBufferLength(1024 * 1024)
                 .SetBindIPHost(new IPHost(7789))
                 .UsePlugin())
                 .Start();

            Console.WriteLine("等待接收");
            Console.ReadKey();
        }
    }

}
