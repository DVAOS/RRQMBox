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
using RRQMCore;
using RRQMCore.ByteManager;
using RRQMCore.Dependency;
using RRQMCore.Run;
using RRQMSocket;
using RRQMSocket.Plugins;
using System;
using System.Text;

namespace RRQMService.Token
{
    public static class TokenDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.简单Token服务器");
            Console.WriteLine("2.多租户Token服务器");
            Console.WriteLine("3.测试Token服务器连接性能");
            Console.WriteLine("4.测试扩展");
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
                case "4":
                    {
                        StartPlugTokenService();
                        break;
                    }
                default:
                    break;
            }
        }

        static void StartPlugTokenService()
        {
            TokenService service = new TokenService();
            service.AddPlugin<MyTokenPlug>();

            service.Setup(new RRQMConfig()
                .SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })
                .SetVerifyToken("Token"))
                .Start();

            Console.WriteLine($"测试Token插件服务器启动成功");
        }


        static void StartConnectPerformanceTokenService()
        {
            TokenService service = new TokenService();

            service.Setup(new RRQMConfig()
               .SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })
               .SetVerifyToken("Token"))
               .Start();

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
            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.IP}:{client.Port}连接"); };
            service.Disconnected += (client, e) => { Console.WriteLine($"客户端{client.IP}:{client.Port}断开连接，原因：{e.Message}"); };
            service.Received += (client, byteBlock, requestInfo) =>
            {
                //从客户端收到信息
                string mes = byteBlock.ToString();
                Console.WriteLine($"已从{client.IP}:{client.Port}接收到信息：{mes}");//Name即IP+Port
                client.Send(byteBlock);
            };

            service.Setup(new RRQMConfig()
                .SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })
                .SetVerifyToken("Token"))
                .Start();

            Console.WriteLine($"Token服务器启动成功,请使用'{service.VerifyToken}'连接");
        }

        private static void CreateQosTokenService()
        {
            TokenService<QosTokenSocketClient> service = new TokenService<QosTokenSocketClient>();
            service.Connected += (client, e) => { Console.WriteLine($"客户端{client.IP}:{client.Port}连接，类型：{client.ClientType}"); };

            service.Disconnected += (client, e) => { Console.WriteLine($"客户端{client.IP}:{client.Port}断开连接，原因：{e.Message}"); };

            service.Setup(new RRQMConfig()
               .SetListenIPHosts(new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) })
               .SetVerifyToken("Token"))
               .Start();

            Console.WriteLine($"Token服务器启动成功,请使用'{service.VerifyToken}'连接为主人，以'T'开头认证为游客。其他类型客户端请使用ASCII发送“me”连接为非正常客户端");
        }
    }

    class MyTokenPlug : TokenPluginBase
    {
        public static readonly DependencyProperty ClientTypeProperty =
            DependencyProperty.Register("ClientType", typeof(ClientType), typeof(MyTokenPlug), ClientType.Owner);

        protected override void OnReceivedData(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            base.OnReceivedData(client, e);
        }

        protected override void OnHandleTokenData(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            base.OnHandleTokenData(client, e);
        }

        protected override void OnConnected(ITcpClientBase client, RRQMEventArgs e)
        {
            Console.WriteLine($"从{this.GetType().Name}收到连接，类型为:{client.GetValue<ClientType>(ClientTypeProperty)}");
        }

        protected override void OnAbnormalVerify(ITcpClientBase client, ReceivedDataEventArgs e)
        {
            if (e.ByteBlock.ToString() == "1")
            {
                e.AddOperation(RRQMCore.Operation.Permit);
                e.AddOperation(RRQMCore.Operation.Handled);

                client.SetValue(ClientTypeProperty, ClientType.AbnormalClient);//注入属性
            }
            base.OnAbnormalVerify(client, e);
        }
    }


    public class QosTokenSocketClient : TokenSocketClient
    {
        /// <summary>
        /// 客户端类型
        /// </summary>
        public ClientType ClientType { get; set; }


        protected override void HandleTokenData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            //此处处理数据，相当于Received事件。
            //从客户端收到信息
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            Console.WriteLine($"已从{this.IP}:{this.Port}接收到信息：{mes}");//Name即IP+Port
            this.Send(byteBlock);
            base.HandleTokenData(byteBlock, requestInfo);
        }

        protected override void OnVerifyToken(VerifyOptionEventArgs e)
        {
            e.AddOperation(RRQMCore.Operation.Handled);//此处表示，该消息已被处理

            if (e.Token == this.VerifyToken)
            {
                this.ClientType = ClientType.Owner;
                e.AddOperation(RRQMCore.Operation.Permit);//如果是配置中的Token，直接允许连接
            }
            else if (e.Token.StartsWith("T"))//以T为标识示例，标识为游客
            {
                e.AddOperation(RRQMCore.Operation.Permit);
                this.ClientType = ClientType.Guest;
            }
            else
            {
                e.RemoveOperation(RRQMCore.Operation.Permit);
                e.Message = "啥也不是";
            }

            base.OnVerifyToken(e);
        }

        protected override void OnAbnormalVerify(ReceivedDataEventArgs e)
        {
            if (e.ByteBlock.ToString() == "me")
            {
                this.ClientType = ClientType.AbnormalClient;
                e.AddOperation(RRQMCore.Operation.Permit);
                e.AddOperation(RRQMCore.Operation.Handled);
            }
            base.OnAbnormalVerify(e);
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
