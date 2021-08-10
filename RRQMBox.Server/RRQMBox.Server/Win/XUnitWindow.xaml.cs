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
using RRQMCore.ByteManager;
using RRQMSkin.Windows;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// XUnitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class XUnitWindow : RRQMWindow
    {
        public XUnitWindow()
        {
            InitializeComponent();
        }

        private void ShowMsg(string msg)
        {
            this.UIInvoke(() =>
            {
                this.msgBox.AppendText($"{msg}\r\n");
            });
        }

        private void UIInvoke(Action action)
        {
            this.Dispatcher.Invoke(() =>
            {
                action.Invoke();
            });
        }

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            this.CreateTcpService(7789);
            this.CreateUdpService(7790, 7791);
            this.CreateTokenService(7792);
            this.CreateProtocolService(7793);
        }

        private void CreateProtocolService(int port)
        {
            SimpleProtocolService service = new SimpleProtocolService();
            service.Received += (SimpleProtocolSocketClient arg1, short? arg2, ByteBlock arg3) =>
            {
                ShowMsg($"ProtocolService收到数据，协议为：{arg2}，数据长度为：{arg3.Len - 2}");
                if (arg2 == 10)
                {
                    arg1.Send(10, Encoding.UTF8.GetBytes(arg1.ID));
                }
                else
                {
                    arg1.Send(arg3.Buffer, 2, arg3.Len - 2);
                }

            };


            //属性设置
            var config = new ServiceConfig();
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(port) })
                .SetValue(ServiceConfig.ThreadCountProperty, 1)//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServiceConfig.VerifyTokenProperty, "XUnitTest")//设置验证令箭
                .SetValue(TokenServiceConfig.VerifyTimeoutProperty, 3);//验证超时时间为3秒

            //方法
            service.Setup(config);
            service.Start();
            ShowMsg($"ProtocolService已启动,端口：{port}");
        }

        private void CreateTokenService(int port)
        {
            SimpleTokenService service = new SimpleTokenService();
            service.Received += (SimpleSocketClient arg1, ByteBlock arg2, object arg3) =>
            {
                arg1.Send(arg2);
            };

            //属性设置
            var config = new ServiceConfig();
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(port) })
                .SetValue(ServiceConfig.ThreadCountProperty, 1)//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServiceConfig.VerifyTokenProperty, "XUnitTest")//设置验证令箭
                .SetValue(TokenServiceConfig.VerifyTimeoutProperty, 3);//验证超时时间为3秒

            //方法
            service.Setup(config);
            service.Start();
            ShowMsg($"TokenService已启动,端口：{port}");
        }

        private void CreateUdpService(int bindPort, int targetPort)
        {
            SimpleUdpSession udpSession = new SimpleUdpSession();
            udpSession.Received += (EndPoint endpoint, ByteBlock e) =>
             {
                 udpSession.Send(e);//将接收到的数据发送至默认终端
             };
            var config = new UdpSessionConfig();//UDP配置
            config.SetValue(UdpSessionConfig.DefaultRemotePointProperty, new IPHost($"127.0.0.1:{targetPort}").EndPoint)//设置默认终结点，用于发送
            .SetValue(UdpSessionConfig.UseBindProperty, true)//是否执行绑定，一般作为接收端时需要绑定
            .SetValue(UdpSessionConfig.ListenIPHostsProperty, new IPHost[] { new IPHost($"127.0.0.1:{bindPort}") })//绑定的IPHost，udp只能绑定一个地址。
            .SetValue(UdpSessionConfig.BufferLengthProperty, 2048)//设置缓存
            .SetValue(UdpSessionConfig.ServerNameProperty, "RRQMUdpServer");//设置服务名称

            udpSession.Setup(config);//加载配置
            udpSession.Start();//启动
            ShowMsg($"UdpService已启动，监听端口：{bindPort}，目标端口：{targetPort}");
        }

        private void CreateTcpService(int port)
        {
            SimpleTcpService tcpService = new SimpleTcpService();

            //订阅初始化事件
            tcpService.CreateSocketCliect += (SimpleSocketClient arg1, CreateOption arg2) =>
            {
                arg1.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//设置数据处理适配器
            };

            //订阅连接事件
            tcpService.ClientConnected += (object sender, MesEventArgs e) =>
            {
                ShowMsg("客户端已连接到TcpService");
            };

            //订阅收到消息事件
            tcpService.Received += (SimpleSocketClient arg1, RRQMCore.ByteManager.ByteBlock arg2, object arg3) =>
            {
                arg1.Send(arg2);
            };

            //订阅断开连接事件
            tcpService.ClientDisconnected += (object sender, MesEventArgs e) =>
            {
                ShowMsg("客户端已断开");
            };

            //注入配置
            var config = new ServiceConfig();
            config.SetValue(ServiceConfig.ListenIPHostsProperty, new IPHost[] { new IPHost($"127.0.0.1:{port}") })
                .SetValue(ServiceConfig.LoggerProperty, new Log())//设置内部日志记录器
                .SetValue(ServiceConfig.ThreadCountProperty, 1)//设置多线程数量
                .SetValue(TcpServiceConfig.ClearIntervalProperty, 300)//300秒无数据交互将被清理
                .SetValue(ServiceConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TcpServiceConfig.SeparateThreadReceiveProperty, false);
            //载入配置                                                       
            tcpService.Setup(config);

            //或通过实例注入配置，实例注入时须实例化对应配置，否则部分属性不可见
            //var config1 = new TcpServiceConfig();
            //config1.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789") };
            //config1.Logger = new Log();
            //config1.ThreadCount = 5;
            //config1.ClearInterval = 300;
            //config1.BufferLength = 1024;

            //载入配置                                                       
            //tcpService.Setup(config1);


            //启动
            tcpService.Start();
            ShowMsg($"TcpService已启动,端口：{port}");
        }

    }

    public class XUnitTestServer : ServerProvider
    {
        [RRQMRPC]
        public int Sum(int a,int b)
        {
            return a + b;
        }
    }
}
