//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Demo.TestTcpService
{
    class ServiceProgram
    {
        static void Main(string[] args)
        {
            TcpService<MyTcpSocketClient> service = new TcpService<MyTcpSocketClient>();
            service.ClientConnected += Service_ClientConnected;
            service.IsCheckClientAlive = true;
          
            service.Bind(7789,10);

            /*
             * Ipv6
              service.Bind(AddressFamily.InterNetworkV6,new IPEndPoint(IPAddress.IPv6Any,7789),1);
             */
            Console.WriteLine("绑定成功");
            Console.ReadKey();
        }

        private static void Service_ClientConnected(object sender, RRQMSocket.MesEventArgs e)
        {
            MyTcpSocketClient tcpSocketClient = (MyTcpSocketClient)sender;
            Console.WriteLine($"客户端连接,Name={tcpSocketClient.Name},ID={tcpSocketClient.ID}");
        }
    }

   
}
