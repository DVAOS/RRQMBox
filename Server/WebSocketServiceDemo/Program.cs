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
using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.WebSocket;
using RRQMSocket.WebSocket.Helper;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleWSService wSService = new SimpleWSService();
            wSService.Received += WSService_Received;
            wSService.Connected += WSService_Connected;
            wSService.Setup(7789).Start();
            Console.WriteLine("服务器已启动");
            Console.ReadKey();
        }

        private static void WSService_Connected(SimpleWSSocketClient client, MesEventArgs e)
        {
            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 5000, (loop) =>
            {
                if (!client.Ping())
                {
                    loop.Dispose();
                }
            });

            loopAction.RunAsync();
        }

        private static void WSService_Received(SimpleWSSocketClient client, WSDataFrame dataFrame)
        {
            switch (dataFrame.Opcode)
            {
                case WSDataType.Cont:
                    Console.WriteLine($"收到中间数据，长度为：{dataFrame.PayloadLength}");
                    break;
                case WSDataType.Text:
                    Console.WriteLine(dataFrame.GetMessage());
                    break;
                case WSDataType.Binary:
                    if (dataFrame.FIN)
                    {
                        Console.WriteLine($"收到二进制数据，长度为：{dataFrame.PayloadLength}");
                    }
                    else
                    {
                        Console.WriteLine($"收到未结束的二进制数据，长度为：{dataFrame.PayloadLength}");
                    }
                    break;
                case WSDataType.Close:
                    break;
                case WSDataType.Ping:
                    break;
                case WSDataType.Pong:
                    break;
                default:
                    break;
            }

            byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            client.SubpackageSend(data, 0, data.Length, 4);
        }
    }
}
