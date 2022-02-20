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
using RRQMClient.FileService;
using RRQMClient.Protocol;
using RRQMClient.RPC;
using RRQMClient.Ssl;
using RRQMClient.TCP;
using RRQMClient.Token;
using RRQMClient.UDP;
using RRQMClient.WebSocket;
using System;

namespace RRQMClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.TCP客户端");
            Console.WriteLine("2.UDP客户端");
            Console.WriteLine("3.SslTCP客户端");
            Console.WriteLine("4.Token客户端");
            Console.WriteLine("5.Protocol客户端");
            Console.WriteLine("6.WebSocket客户端");
            Console.WriteLine("7.RRQM RPC客户端");
            Console.WriteLine("8.RRQM 反向RPC客户端");
            Console.WriteLine("9.文件客户端");
            var input = Console.ReadLine();
            Console.Clear();
            switch (input)
            {
                case "1":
                    {
                        TCPDemo.Start();
                        break;
                    }
                case "2":
                    {
                        UDPDemo.Start();
                        break;
                    }
                case "3":
                    {
                        SslTCP.Start();
                        break;
                    }
                case "4":
                    {
                        TokenDemo.Start();
                        break;
                    }
                case "5":
                    {
                        ProtocolDemo.Start();
                        break;
                    }
                case "6":
                    {
                        WebSocketDemo.Start();
                        break;
                    }
                case "7":
                    {
                        RPCDemo.Start();
                        break;
                    }
                case "8":
                    {
                        ReverseRPCDemo.Start();
                        break;
                    }
                case "9":
                    {
                        FileClientDemo.Start();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }
    }
}
