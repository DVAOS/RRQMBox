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
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.Ssl
{
    public static class SslTCP
    {
        public static void Start()
        {
            Console.WriteLine("1.BIO简单SslTCP服务器");
            Console.WriteLine("2.Select简单SslTCP服务器");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        StartSslTcpService(ReceiveType.BIO);
                        break;
                    }
                case "2":
                    {
                        StartSslTcpService(ReceiveType.Select);
                        break;
                    }
                default:
                    break;
            }
        }
        static void StartSslTcpService(ReceiveType receiveType)
        {
            TcpService service = new TcpService();

            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.Name}连接"); };

            service.Disconnected += (client, e) => { Console.WriteLine($"客户端{client.Name}断开连接，原因：{e.Message}"); };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"已从{client.Name}接收到信息：{mes}");//Name即IP+Port
                client.Send(byteBlock);
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = receiveType;
            config.SslOption = new ServiceSslOption() { Certificate = new X509Certificate2("RRQMSocket.pfx", "RRQMSocket"), SslProtocols = SslProtocols.Tls12 };
            config.ReceiveType = ReceiveType.Select;
            //载入配置
            service.Setup(config);

            //启动
            service.Start();


            Console.WriteLine($"Ssl {receiveType}服务器启动成功");
        }
    }
}
