using RRQMService.FileServiceN;
using RRQMService.JsonRpc;
using RRQMService.NAT;
using RRQMService.Protocol;
using RRQMService.RPC;
using RRQMService.Ssl;
using RRQMService.TCP;
using RRQMService.Token;
using RRQMService.UDP;
using RRQMService.WebSocket;
using RRQMService.XUnitTest;
using System;

namespace RRQMService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.TCP服务器");
            Console.WriteLine("2.UDP服务器");
            Console.WriteLine("3.SslTCP服务器");
            Console.WriteLine("4.Token服务器");
            Console.WriteLine("5.Protocol服务器");
            Console.WriteLine("6.WebSocket服务器");
            Console.WriteLine("7.RRQM RPC服务器");
            Console.WriteLine("8.RRQM 反向RPC服务器");
            Console.WriteLine("9.NAT 地址转换服务器");
            Console.WriteLine("10.文件服务器");
            Console.WriteLine("11.JsonRpc服务器");
            Console.WriteLine("12.XUnitTest服务器");
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
                        NATDemo.Start();
                        break;
                    }
                case "10":
                    {
                        FileServiceDemo.Start();
                        break;
                    } 
                case "11":
                    {
                        JsonRpcDemo.Start();
                        break;
                    }
                case "12":
                    {
                        XUnitTestDemo.Start();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }
    }
}
