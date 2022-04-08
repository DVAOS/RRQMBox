using RRQMSocket;
using System;
using System.Net;
using System.Text;

namespace UdpClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpSession udpSession = new UdpSession();
            udpSession.SetDataHandlingAdapter(new UdpPackageAdapter());
            udpSession.Received += (remote, byteBlock, requestInfo) =>
            {
                Console.WriteLine($"收到：{Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len)}");
            };
            udpSession.Setup(new RRQMConfig()
                .SetBufferLength(1024*1024)
                 .SetBindIPHost(new IPHost(7788))
                 .SetRemoteIPHost(new IPHost("127.0.0.1:7789")))
                 .Start();

            while (true)
            {
                Console.ReadKey();
                udpSession.Send(new byte[1024*100]);
            }
        }
    }
}
