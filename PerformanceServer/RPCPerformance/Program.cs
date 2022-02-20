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
using System;

namespace RPCPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("0.All");
            Console.WriteLine("1.RRQMRPC-TCP");
            Console.WriteLine("2.NewLifeRPC");
            Console.WriteLine("3.BeetleXRPC");
            switch (Console.ReadLine())
            {
                case "0":
                    {
                        RRQMRPCTCP.Start();
                        NewLifeRPC.Start();
                        BeetleXRPC.Start();
                        break;
                    }
                case "1":
                    {
                        RRQMRPCTCP.Start();
                        break;
                    }
                case "2":
                    {
                        NewLifeRPC.Start();
                        break;
                    }
                case "3":
                    {
                        BeetleXRPC.Start();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }
    }
}
