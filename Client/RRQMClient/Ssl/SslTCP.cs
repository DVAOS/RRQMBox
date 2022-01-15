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
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.Ssl
{
    public static class SslTCP
    {
        public static void Start()
        {
            Console.WriteLine("1.BIO简单SslTCP客户端");
            Console.WriteLine("2.Select简单SslTCP客户端");//客户端Select模型和BIO一样
            switch (Console.ReadLine())
            {
                case "1":
                case "2":
                    {
                        StartSimpleTcpClient(ReceiveType.BIO);
                        break;
                    }
              
                default:
                    break;
            }
        }
        static void StartSimpleTcpClient(ReceiveType receiveType)
        {
            SimpleTcpClient tcpClient = new SimpleTcpClient();

            tcpClient.Connected += (client, e) =>
            {
                Console.WriteLine(e.Message);
                //成功连接到服务器
            };
            tcpClient.Received += (client, byteBlock, obj) =>
            {
                //从服务器收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"已从服务器接收到信息：{mes}");
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
                Console.WriteLine(e.Message);
            };

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.ReceiveType = receiveType;
            config.SslOption = new ClientSslOption()
            {
                ClientCertificates = new X509CertificateCollection() { new X509Certificate2("RRQMSocket.pfx", "RRQMSocket") },
                SslProtocols = SslProtocols.Tls12,
                TargetHost = "127.0.0.1",
                CertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; }
            };

            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tcpClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }
    }
}
