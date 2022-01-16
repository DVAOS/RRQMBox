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

namespace RPCPerformanceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.RRQMRPC-TCP");
            Console.WriteLine("2.NewLifeRPC");
            Console.WriteLine("3.BeetleXRPC");
            Console.WriteLine("4.Grpc");
            var input = Console.ReadLine();
            Console.Clear();
            switch (input)
            {
                case "1":
                    {
                        RRQMRPCTCP.Start(100000);
                        break;
                    }
                case "2":
                    {
                        NewLifeRPC.Start(100000);
                        break;
                    } 
                case "3":
                    {
                        BeetleXRPC.Start(100000);
                        break;
                    }
                case "4":
                    {
                        GrpcDemoClient.Start(100000);
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }
    }
}
