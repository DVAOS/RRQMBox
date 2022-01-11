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
                        FileServiceDemo.Start();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }
    }
}
