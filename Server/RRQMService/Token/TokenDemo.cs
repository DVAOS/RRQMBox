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
using RRQMCore.ByteManager;
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.Token
{
    public static class TokenDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.简单Token服务器");
            Console.WriteLine("2.多租户Token服务器");
            Console.WriteLine("3.测试Token服务器连接性能");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateSimpleTokenService();
                        break;
                    }
                case "2":
                    {
                        CreateQosTokenService();
                        break;
                    }
                case "3":
                    {
                        StartConnectPerformanceTokenService();
                        break;
                    }
                default:
                    break;
            }
        }

        static void StartConnectPerformanceTokenService()
        {
            TokenService service = new TokenService();

            //声明配置
            var config = new TokenServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址

            //继承TokenService配置
            config.VerifyToken = "Token";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时
            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                Console.WriteLine($"在线客户端数量：{service.SocketClients.Count}");
            });

            loopAction.RunAsync();

            Console.WriteLine($"测试Token连接服务器启动成功");
        }

        private static void CreateSimpleTokenService()
        {
            TokenService service = new TokenService();
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
            var config = new TokenServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址

            //继承TokenService配置
            config.VerifyToken = "Token";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //载入配置
            service.Setup(config);

            //启动
            service.Start();
            Console.WriteLine($"Token服务器启动成功,请使用'{service.VerifyToken}'连接");
        }

        private static void CreateQosTokenService()
        {
            TokenService<QosTokenSocketClient> service = new TokenService<QosTokenSocketClient>();
            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.Name}连接，类型：{client.ClientType}"); };

            service.Disconnected += (client, e) => { Console.WriteLine($"客户端{client.Name}断开连接，原因：{e.Message}"); };

            //声明配置
            var config = new TokenServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址

            //继承TokenService配置
            config.VerifyToken = "Token";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //载入配置
            service.Setup(config);

            //启动
            service.Start();
            Console.WriteLine($"Token服务器启动成功,请使用'{service.VerifyToken}'连接为主人，以'T'开头认证为游客。其他类型客户端请使用ASCII发送“me”连接为非正常客户端");
        }
    }

    public class QosTokenSocketClient : TokenSocketClient
    {
        /// <summary>
        /// 客户端类型
        /// </summary>
        public ClientType ClientType { get; set; }


        protected override void HandleTokenReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //此处处理数据，相当于Received事件。
            //从客户端收到信息
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            Console.WriteLine($"已从{this.Name}接收到信息：{mes}");//Name即IP+Port
            this.Send(byteBlock);
        }

        protected override void OnVerifyToken(VerifyOption verifyOption)
        {
            if (verifyOption.Token == this.VerifyToken)
            {
                this.ClientType = ClientType.Owner;
                verifyOption.Accept = true;//如果是配置中的Token，直接允许连接
            }
            else if (verifyOption.Token.StartsWith("T"))//以T为标识示例，标识为游客
            {
                verifyOption.Accept = true;
                this.ClientType =  ClientType.Guest;
            }
            else
            {
                verifyOption.Accept = false;
                verifyOption.ErrorMessage = "啥也不是";
            }
        }

        /// <summary>
        /// 此处处理非常规连接，一般用于接收其他类型的TCP客户端。
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        /// <returns></returns>
        protected override bool OnAbnormalVerify(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (Encoding.ASCII.GetString(byteBlock.Buffer,0,byteBlock.Len) == "me")
            {
                this.ClientType = ClientType.AbnormalClient;
                return true;
            }
            return false;
        }
    }

    public enum ClientType
    {
        /// <summary>
        /// 主人
        /// </summary>
        Owner,

        /// <summary>
        /// 访客
        /// </summary>
        Guest,

        /// <summary>
        /// 其他类型客户端
        /// </summary>
        AbnormalClient
    }
}
