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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketLib
{
    public class RRQMIOCPMode
    {
        public void Start(int port)
        {
            //用列表存下已连接的客户端，模拟RRQM的SocketClients集合。
            List<SimulationSocketClient> simulations = new List<SimulationSocketClient>();
            Task.Run(() =>
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(10);
                while (true)
                {
                    Socket newSocket = socket.Accept();//同步接收，这个无所谓

                    SimulationSocketClient simulationSocketClient = new SimulationSocketClient() { Socket = newSocket };
                    simulations.Add(simulationSocketClient);
                    simulationSocketClient.BeginReceive();
                }
            });
        }
    }

    class SimulationSocketClient
    {
        public Socket Socket { get; set; }

        public void BeginReceive()
        {
            eventArgs = new SocketAsyncEventArgs();
            eventArgs.Completed += EventArgs_Completed;

            byte[] buffer = new byte[1024 * 64];
            eventArgs.SetBuffer(buffer, 0, buffer.Length);

            if (!Socket.ReceiveAsync(eventArgs))
            {
                ProcessReceived(eventArgs);
            }
        }
        SocketAsyncEventArgs eventArgs;


        private void EventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceived(e);
            }
        }
        private void ProcessReceived(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                //在这里处理数据，此处不做任何处理，直接进行下次接收。
                if (!Socket.ReceiveAsync(e))
                {
                    ProcessReceived(e);
                }
            }
        }
    }
}
