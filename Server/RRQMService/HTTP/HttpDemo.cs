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
using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.Http.Plugins;
using RRQMSocket.WebSocket;
using RRQMSocket.WebSocket.Plugins;
using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace RRQMService.HTTP
{
    public class HttpDemo
    {
        public static void Start()
        {
            var service = new HttpService();

            service.AddPlugin<MyHttpPlug>();
            service.AddPlugin<HttpStaticPagePlugin>().
               AddFolder("../../../../../api");//添加静态页面

            service.AddPlugin<WebSocketServerPlugin>().//添加WS解析
               SetTimeout(10 * 1000).
               SetWSUrl("/ws").
               SetCallback(WSCallback);

            service.AddPlugin<MyWebSocketPlugin>();//添加WS事务触发。
            
            service.AddPlugin<MyWSCommandLinePlugin>();//添加WS命令行事务。

            var config = new RRQMConfig();
            config.UsePlugin()
                .SetReceiveType(ReceiveType.Auto)
                //.SetServiceSslOption(new ServiceSslOption()
                //{
                //    Certificate = new X509Certificate2("RRQMSocket.pfx", "RRQMSocket"),
                //    SslProtocols = SslProtocols.Tls12
                //})
                .SetListenIPHosts(new IPHost[] { new IPHost(7789) });

            service.Setup(config).Start();
            Console.WriteLine("Http服务器已启动");
            Console.WriteLine("浏览器访问：http://127.0.0.1:7789/index.html");
            Console.WriteLine("WS访问：ws://127.0.0.1:7789/ws");
            Console.ReadKey();
        }

        static void WSCallback(ITcpClientBase client, WSDataFrameEventArgs e)
        {
            switch (e.DataFrame.Opcode)
            {
                case WSDataType.Cont:
                    Console.WriteLine($"收到中间数据，长度为：{e.DataFrame.PayloadLength}");
                    break;
                case WSDataType.Text:
                    Console.WriteLine(e.DataFrame.ToText());
                    break;
                case WSDataType.Binary:
                    if (e.DataFrame.FIN)
                    {
                        Console.WriteLine($"收到二进制数据，长度为：{e.DataFrame.PayloadLength}");
                    }
                    else
                    {
                        Console.WriteLine($"收到未结束的二进制数据，长度为：{e.DataFrame.PayloadLength}");
                    }
                    break;
                case WSDataType.Close:
                    {
                        Console.WriteLine("远程请求断开");
                        client.Close("断开");
                    }

                    break;
                case WSDataType.Ping:
                    break;
                case WSDataType.Pong:
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 命令行插件。
    /// 声明的方法必须以"Command"结尾，支持json字符串，参数之间空格隔开。
    /// </summary>
    class MyWSCommandLinePlugin : WSCommandLinePlugin
    {
        public int AddCommand(int a, int b)
        {
            return a + b;
        }

        public SumClass SumCommand(SumClass sumClass)
        {
            sumClass.Sum = sumClass.A + sumClass.B;
            return sumClass;
        }
    }

    class SumClass
    {
        public int A { get; set; }
        public int B { get; set; }
        public int Sum { get; set; }

    }

    /// <summary>
    /// 支持GET、Post、Put，Delete，或者其他
    /// </summary>
    class MyHttpPlug : HttpPluginBase
    {
        protected override void OnGet(ITcpClientBase client, HttpContextEventArgs e)
        {
            Console.WriteLine(e.Request.ToString());
            HttpResponse httpResponse = new HttpResponse();
            httpResponse.FileNotFind();
            e.Response = httpResponse;

            e.Handled = true;
            base.OnGet(client, e);
        }

        protected override void OnReceivedOtherHttpRequest(ITcpClientBase client, HttpContextEventArgs e)
        {
            Console.WriteLine(e.Request.ToString());
            base.OnReceivedOtherHttpRequest(client, e);
        }
    }

    class MyWebSocketPlugin : WebSocketPluginBase
    {
        protected override void OnConnected(ITcpClientBase client, RRQMEventArgs e)
        {
            Console.WriteLine("TCP连接");
            base.OnConnected(client, e);
        }

        protected override void OnHandshaking(ITcpClientBase client, HttpContextEventArgs e)
        {
            Console.WriteLine("WebSocket正在连接");
            //e.IsPermitOperation = false;表示拒绝
            base.OnHandshaking(client, e);
        }

        protected override void OnHandshaked(ITcpClientBase client, HttpContextEventArgs e)
        {
            Console.WriteLine("WebSocket成功连接");
            base.OnHandshaked(client, e);
        }

        protected override void OnDisconnected(ITcpClientBase client, ClientDisconnectedEventArgs e)
        {
            Console.WriteLine("TCP断开连接");
            base.OnDisconnected(client, e);
        }
    }
}
