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
            SimpleTokenService service = new SimpleTokenService();

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
            SimpleTokenService service = new SimpleTokenService();
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
            TokenService<QosTokenSocketClient> service =new  TokenService<QosTokenSocketClient>();
            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.Name}连接，{(client.Guest?"游客":"主人")}"); };

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
            Console.WriteLine($"Token服务器启动成功,请使用'{service.VerifyToken}'连接为主人，以'T'开头认证为游客。");
        }
    }

    public class QosTokenSocketClient : TokenSocketClient
    {
        /// <summary>
        /// 是否为访客
        /// </summary>
        public bool Guest { get; set; }


        protected override void HandleTokenReceivedData(ByteBlock byteBlock, object obj)
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
                verifyOption.Accept = true;//如果是配置中的Token，直接允许连接
            }
            else if (verifyOption.Token.StartsWith("T"))//以T为标识示例，标识为游客
            {
                verifyOption.Accept = true;
                this.Guest = true;
            }
            else
            {
                verifyOption.Accept = false;
                verifyOption.ErrorMessage = "啥也不是";
            }
        }
    }
}
