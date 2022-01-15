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
using HPSocket;
using HPSocket.Tcp;
using RRQMSocket;
using System;
using System.Text;

namespace TcpServicePerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.HPSocket服务");
            Console.WriteLine("2.SuperSocket服务");
            Console.WriteLine("3.RRQMSocket服务");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        HPSocketDemo.Start();
                        break;
                    } 
                case "2":
                    {
                        SuperSocketDemo.Start();
                        break;
                    }
                case "3":
                    {
                        RRQMSocketDemo.Start();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        private static HandleResult TcpServer_OnReceive(HPSocket.IServer sender, IntPtr connId, byte[] data)
        {
            //sender.Send(connId,data,data.Length);
            return HandleResult.Ok;
        }
    }
}
