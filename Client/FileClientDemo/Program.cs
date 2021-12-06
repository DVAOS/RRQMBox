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
using RRQMCore;
using RRQMCore.ByteManager;
using RRQMCore.Run;
using RRQMSocket;
using RRQMSocket.FileTransfer;
using System;
using System.Threading.Tasks;

namespace FileClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.测试拉取文件");
            Console.WriteLine("2.测试推送文件");
            switch (Console.ReadLine())
            {
                case "1": 
                    {
                        TestPullFile();
                        break;
                    }
                case "2":
                    {
                        TestPushFile();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        /// <summary>
        /// 测试推送文件
        /// </summary>
        static void TestPushFile()
        {
            FileClient fileClient = CreateFileClientPro();

            FileRequest fileRequest = new FileRequest(@"D:\360Downloads\360极速浏览器.exe", $@"C:\Users\carywang\Desktop\新建文件夹\Test.exe");
            fileRequest.Overwrite = true;

            fileRequest.FileCheckerType = FileCheckerType.MD5;
            fileRequest.Flags = TransferFlags.BreakpointResume;

            FileOperator fileOperator = new FileOperator();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                if (fileOperator.Result.ResultCode != ResultCode.Default)
                {
                    loop.Dispose();
                }

                Console.WriteLine($"进度：{fileOperator.Progress}，速度：{fileOperator.Speed()}");
            });

            loopAction.RunAsync();

            fileOperator.SetMaxSpeed(1024);

            RRQMCore.Run.EasyAction.DelayRun(1000 * 10, () =>
            {
                fileOperator.SetMaxSpeed(1024*1024);
            });

            Metadata metadata = new Metadata();
            metadata.Add("1", "1");
            metadata.Add("2", "2");
            IResult result = fileClient.PushFile(fileRequest, fileOperator, metadata);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 测试下拉文件
        /// </summary>
        static void TestPullFile()
        {
            FileClient fileClient = CreateFileClientPro();

            FileRequest fileRequest = new FileRequest(@"D:\360Downloads\360极速浏览器.exe", $@"C:\Users\carywang\Desktop\新建文件夹\Test.exe");
            fileRequest.Overwrite = true;

            fileRequest.FileCheckerType = FileCheckerType.MD5;
            fileRequest.Flags = TransferFlags.BreakpointResume;

            FileOperator fileOperator = new FileOperator();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                if (fileOperator.Result.ResultCode != ResultCode.Default)
                {
                    loop.Dispose();
                }

                Console.WriteLine($"进度：{fileOperator.Progress}，速度：{fileOperator.Speed()}");
            });

            loopAction.RunAsync();

            fileOperator.SetMaxSpeed(1024);

            RRQMCore.Run.EasyAction.DelayRun(1000 * 10, () =>
            {
                fileOperator.SetMaxSpeed(int.MaxValue);
            });

            Metadata metadata = new Metadata();
            metadata.Add("1", "1");
            metadata.Add("2", "2");
            IResult result = fileClient.PullFile(fileRequest, fileOperator, metadata);
            Console.WriteLine(result);
        }

        static FileClient CreateFileClientPro()
        {
            FileClient fileClient = new FileClient();

            fileClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            fileClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };


            //声明配置
            var config = new FileClientConfig();

            //继承TcpClient配置
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.BytePool = BytePool.Default;//设置内存池实例。
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.DataHandlingAdapter = new FixedHeaderDataHandlingAdapter();//设置数据处理适配器,此处只能设置FixedHeaderDataHandlingAdapter类
            config.OnlySend = false;//仅发送，即不开启接收线程（此设置严谨开启）。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送

            //继承TcpRpcParser配置，以实现RPC交互
            config.ProxyToken = "FileServerRPC";//代理令箭，用于获取代理文件,或服务时需验证令箭

            //注入配置
            fileClient.Setup(config);

            //连接服务器
            fileClient.Connect("FileServer");
            //fileClient.DiscoveryService();//需要RPC交互时需要发现服务。
            Console.WriteLine("连接成功");

            return fileClient;
        }
    }
}
