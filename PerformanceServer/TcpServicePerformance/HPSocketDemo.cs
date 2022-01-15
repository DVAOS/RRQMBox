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
using HPSocket.Tcp;
using RRQMCore.Run;
using System;
using System.Collections.Generic;
using System.Text;

namespace TcpServicePerformance
{
    public static class HPSocketDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.测试连接");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TestConnect();
                        break;
                    }
                default:
                    break;
            }
           
        }

        static void TestConnect()
        {
            TcpServer tcpServer = new TcpServer();
            tcpServer.MaxConnectionCount = 10000;
            tcpServer.SocketListenQueue = 1000;
            tcpServer.Port = 7789;
            tcpServer.Start();
            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                Console.WriteLine($"HPSocket在线客户端数量：{tcpServer.ConnectionCount}");
            });

            loopAction.RunAsync();
        }
    }
}
