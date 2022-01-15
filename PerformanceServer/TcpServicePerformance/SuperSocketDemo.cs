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
using RRQMCore.Run;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServicePerformance
{
    public static class SuperSocketDemo
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
            AppServer appServer = new AppServer();
            var config = new SuperSocket.SocketBase.Config.ServerConfig()
            {
                ClearIdleSession = true, //60秒执行一次清理90秒没数据传送的连接
                ClearIdleSessionInterval = 60,
                IdleSessionTimeOut = 90,
                MaxRequestLength = 1024*64, //最大包长度
                Ip = "Any",
                Port = 7789,
                ListenBacklog = 1000,
                MaxConnectionNumber = 10000,
            };

            appServer.Setup(config);
            appServer.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                Console.WriteLine($"SuperSocket在线客户端数量：{appServer.SessionCount}");
            });

            loopAction.RunAsync();
        }
    }
}
