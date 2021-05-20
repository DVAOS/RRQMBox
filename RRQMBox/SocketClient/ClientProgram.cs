//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using System;
using System.Text;
using System.Threading;
using RRQMCore.ByteManager;
using RRQMSocket;

namespace Demo.TestTcpClient
{
    internal class ClientProgram
    {
        private static void Main(string[] args)
        {
            Console.ReadKey();
            TestTcpClient();
            TestTokenTcpClient();
            Console.ReadKey();
        }

        private static int count;

        private static void TestTokenTcpClient()
        {
            TokenTcpClient client = new TokenTcpClient();

            //属性
            client.VerifyToken = "ABC";//设置验证口令
            client.BufferLength = 1024*64;//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
            client.Logger = new Log();//设置内部日志记录器，默认日志是控制台输出。
            client.DataHandlingAdapter = new NormalDataHandlingAdapter();//数据处理适配器，可用于处理粘包、解析对象。

            //事件
            client.ConnectedService += Client_ConnectedService;
            client.DisconnectedService += Client_DisconnectedService;
            client.OnReceived += Client_OnReceived;

            //方法
            client.Connect(new IPHost("127.0.0.1:7791"));//连接
            Console.WriteLine("连接成功");
            client.Send(Encoding.UTF8.GetBytes("若汝棋茗"));//发送数据
            Console.WriteLine("发送成功");
        }

        private static void TestTcpClient()
        {
            TcpClient client = new TcpClient();

            //属性
            client.BufferLength = 1024*64;//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
            client.Logger = new Log();//设置内部日志记录器，默认日志是控制台输出。
            client.DataHandlingAdapter = new NormalDataHandlingAdapter();//数据处理适配器，可用于处理粘包、解析对象。

            //事件
            client.ConnectedService += Client_ConnectedService;
            client.DisconnectedService += Client_DisconnectedService;
            client.OnReceived += Client_OnReceived;

            //方法
            client.Connect(new IPHost("127.0.0.1:7789"));//连接
            Console.WriteLine("连接成功");
            client.Send(Encoding.UTF8.GetBytes("若汝棋茗"));//发送数据
            Console.WriteLine("发送成功");

        }

        private static void Client_OnReceived(TcpClient arg1, ByteBlock byteBlock, object arg3)
        {
            count++;
            if (count % 100 == 0)
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Position);
                Console.WriteLine($"已接收到信息：{mes},第{count}条");
            }
        }

        private static void Client_DisconnectedService(object sender, MesEventArgs e)
        {
            Console.WriteLine($"已断开连接");
        }

        private static void Client_ConnectedService(object sender, MesEventArgs e)
        {
            Console.WriteLine($"已成功连接");
        }
    }
}