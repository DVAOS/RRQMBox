//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.ByteManager;
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace TcpServicePerformance
{
    public static class RRQMSocketDemo
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
            BytePool.SetBlockSize(1024, 1024 * 1024 * 10);//重新指定内存池最大、最小值分配。
            TcpService service = new TcpService();
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789")};
            config.MaxCount = 10000;
            config.BufferLength = 1024;
            //config.BufferLength = 1024*64;//此处设置在测试流量时生效
            config.Backlog = 1000;
            service.Setup(config);
            service.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                Console.WriteLine($"RRQMSocket在线客户端数量：{service.SocketClients.Count},(按任意键清空内存池)");
            });

            loopAction.RunAsync();

            
            while (true)
            {
                Console.ReadKey();
                BytePool.Clear();
                Console.WriteLine("GC");
            }
           
        }
    }
}
