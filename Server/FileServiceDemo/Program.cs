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
using RRQMSocket.FileTransfer;
using System;

namespace FileServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateFileServicePro();
            Console.ReadKey();
        }

        static void CreateFileServicePro()
        {
            FileService fileService = new FileService();

            fileService.Connected += (socketClient, e) =>
            {
                //有客户端连接
            };

            fileService.Disconnected += (socketClient, e) =>
            {
                //有客户端断开连接
            };

            fileService.BeforeFileTransfer += (socketClient, e) => 
            {
                Console.WriteLine("请求传输文件");
            };

            fileService.FinishedFileTransfer += (socketClient, e) =>
            {
                Console.WriteLine($"传输文件结束，{e.Result}");
            };

            //声明配置
            var config = new FileServiceConfig();

            //继承TcpService配置
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.ServerName = "RRQMService";//服务名称
            config.ThreadCount = 5;//多线程数量，当SeparateThreadReceive为false时，该值只决定BytePool的数量。
            config.Backlog = 30;
            config.ClearInterval = 60 * 1000;//60秒无数据交互会清理客户端
            config.ClearType = ClearType.Receive | ClearType.Send;//清理统计
            config.MaxCount = 10000;//最大连接数

            //继承TokenService配置
            config.VerifyToken = "FileServer";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //继承ProcotolService配置
            config.CanResetID = true;//是否重新设置ID

            //继承TcpRpcParser配置，以实现RPC交互
            config.NameSpace = "RRQMTest";
            config.RPCVersion = new Version(1, 0, 0, 0);//定义此次发布的RPC版本。
            config.ProxyToken = "FileServerRPC";//代理令箭，当客户端获取代理文件,或服务时需验证令箭

            //文件传输相关配置

            try
            {
                fileService.Setup(config);
                fileService.Start();
                Console.WriteLine("启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
