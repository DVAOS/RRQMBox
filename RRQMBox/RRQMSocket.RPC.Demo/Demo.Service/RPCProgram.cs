//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using RRQMSocket.RPC;
using System;

namespace Demo.Service
{
    internal class RPCProgram
    {
        private static void Main(string[] args)
        {
            RPCService rpcService = new RPCService();
            rpcService.ProxyToken = "123TT";
            rpcService.RegistAllService();

            TcpRPCParser tcpRPCParser = new TcpRPCParser();
            tcpRPCParser.SerializeConverter = new BinarySerializeConverter();
            BindSetting tcpSetting = new BindSetting();
            tcpSetting.IP = "127.0.0.1";
            tcpSetting.Port = 7789;
            tcpSetting.MultithreadThreadCount = 10;
            tcpRPCParser.Bind(tcpSetting);
            Console.WriteLine("TCP解析器添加完成");

            UdpRPCParser udpRPCParser = new UdpRPCParser();
            udpRPCParser.SerializeConverter = new BinarySerializeConverter();
            BindSetting udpSetting = new BindSetting();
            udpSetting.IP = "127.0.0.1";
            udpSetting.Port = 7790;
            udpSetting.MultithreadThreadCount = 10;
            udpRPCParser.Bind(udpSetting);
            Console.WriteLine("UDP解析器添加完成");

            TcpRPCParser tcpXmlRPCParser = new TcpRPCParser();
            tcpXmlRPCParser.SerializeConverter = new XmlSerializeConverter();
            BindSetting tcpXmlSetting = new BindSetting();
            tcpXmlSetting.IP = "127.0.0.1";
            tcpXmlSetting.Port = 7791;
            tcpXmlSetting.MultithreadThreadCount = 10;
            tcpXmlRPCParser.Bind(tcpXmlSetting);
            Console.WriteLine("TCPXml解析器添加完成");

            rpcService.AddRPCParser("TcpParser", tcpRPCParser);
            rpcService.AddRPCParser("UdpParser", udpRPCParser);
            rpcService.AddRPCParser("tcpXmlRPCParser", tcpXmlRPCParser);

            RPCServerSetting setting = new RPCServerSetting();
            setting.NameSpace = "RRQMTest";
           
            rpcService.OpenRPCServer(setting);

            Console.WriteLine("RPC启动完成");
            Console.ReadKey();
        }
    }
}