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
using RRQMCore.Run;
using RRQMSocket;
using RRQMSocket.FileTransfer;
using System;

namespace FileClientDemo
{
    internal class Program
    {
        private static void Main(string[] args)
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
        private static void TestPushFile()
        {
            FileClient fileClient = CreateFileClientPro();

            FileRequest fileRequest = new FileRequest(@"D:\360Downloads\360极速浏览器.exe", $@"C:\Users\carywang\Desktop\新建文件夹\Test.exe");
            fileRequest.Overwrite = true;

            fileRequest.FileCheckerType = FileCheckerType.MD5;
            fileRequest.Flags = TransferFlags.BreakpointResume;

            FileOperator fileOperator = new FileOperator();

            /*片段代码的作用是实时获取传输进度*/
            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                if (fileOperator.Result.ResultCode != ResultCode.Default)
                {
                    loop.Dispose();
                }

                Console.WriteLine($"进度：{fileOperator.Progress}，速度：{fileOperator.Speed()}");
            });

            loopAction.RunAsync();
            /*片段代码的作用是实时获取传输进度*/

            //Task.Run(async () => //上述代码片功能可以用该代码片段代替
            //{
            //    while (true)
            //    {
            //        if (fileOperator.Result.ResultCode != ResultCode.Default)
            //        {
            //            break;
            //        }
            //        Console.WriteLine($"进度：{fileOperator.Progress}，速度：{fileOperator.Speed()}");
            //        await Task.Delay(1000);
            //    }
            //});

            //fileOperator.SetMaxSpeed(1024);

            //RRQMCore.Run.EasyAction.DelayRun(1000 * 10, () =>
            //{
            //    fileOperator.SetMaxSpeed(1024 * 1024);
            //});

            //此代码功能为取消传输任务
            //fileOperator.SetCancellationTokenSource(new CancellationTokenSource());
            //fileOperator.Cancel();

            //传入元数据
            Metadata metadata = new Metadata();
            metadata.Add("1", "1");
            metadata.Add("2", "2");

            IResult result = fileClient.PushFile(fileRequest, fileOperator, metadata);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 测试下拉文件
        /// </summary>
        private static void TestPullFile()
        {
            FileClient fileClient = CreateFileClientPro();

            FileRequest fileRequest = new FileRequest(@"D:\360Downloads\360极速浏览器.exe", $@"C:\Users\carywang\Desktop\新建文件夹\Test.exe");
            fileRequest.Overwrite = true;//是否覆盖
            fileRequest.FileCheckerType = FileCheckerType.MD5;//进行MD5校验
            fileRequest.Flags = TransferFlags.BreakpointResume;//尝试断点续传

            FileOperator fileOperator = new FileOperator();//实例化本次传输的控制器，用于获取传输进度、速度、状态等。

            //此处的作用相当于Timer，定时每秒输出当前的传输进度和速度。
            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                if (fileOperator.Result.ResultCode != ResultCode.Default)
                {
                    loop.Dispose();
                }

                Console.WriteLine($"进度：{fileOperator.Progress}，速度：{fileOperator.Speed()}");
            });

            loopAction.RunAsync();

            fileOperator.SetMaxSpeed(1024);//开源版不支持该操作

            RRQMCore.Run.EasyAction.DelayRun(1000 * 10, () =>
            {
                fileOperator.SetMaxSpeed(int.MaxValue);
            });

            Metadata metadata = new Metadata();//传递到服务器的元数据
            metadata.Add("1", "1");
            metadata.Add("2", "2");

            //此方法会阻塞，直到传输结束，也可以使用PullFileAsync
            IResult result = fileClient.PullFile(fileRequest, fileOperator, metadata);
            Console.WriteLine(result);
        }

        private static FileClient CreateFileClientPro()
        {
            FileClient fileClient = new FileClient();

            //声明配置
            var config = new FileClientConfig();

            //继承TcpClient配置
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost

            //注入配置
            fileClient.Setup(config);

            try
            {
                //连接服务器
                fileClient.Connect("FileServer");
                Console.WriteLine("连接成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return fileClient;
        }
    }
}