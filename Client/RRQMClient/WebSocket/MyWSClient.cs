using RRQMSocket.WebSocket;
using RRQMSocket.WebSocket.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.WebSocket
{
    class MyWSClient : WSClient
    {
        protected override void HandleWSDataFrame(WSDataFrame dataFrame)
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
        }

        protected override void OnPing()
        {
            Console.WriteLine("Ping");
            this.Pong();
        }
    }
}
