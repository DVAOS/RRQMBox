using RRQMCore;
using RRQMCore.Run;
using RRQMSocket;
using RRQMSocket.FileTransfer;
using RRQMSocket.FileTransfer.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.FileService
{
    public static class FileServiceDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.测试拉取文件");
            Console.WriteLine("2.测试推送文件");
            Console.WriteLine("3.测试批量推送文件");
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
                case "3":
                    {
                        TestMultiple();
                        break;
                    }
                default:
                    break;
            }
        }

        static void TestMultiple()
        {
            FileClient fileClient = CreateFileClientPro();

            Console.WriteLine("请输入文件夹路径");
            string[] paths = System.IO.Directory.GetFiles(Console.ReadLine(), "*.*", System.IO.SearchOption.AllDirectories);
            Console.WriteLine($"共搜索到{paths.Length}个文件，按任意键开始传输。");
            Console.ReadKey();

            List<FileRequest> requests = new List<FileRequest>();
            List<FileOperator> fileOperators = new List<FileOperator>();
            List<Metadata> metadatas = new List<Metadata>();

            foreach (var item in paths)
            {
                FileRequest fileRequest = new FileRequest()
                {
                    Path = item,
                    Overwrite = true,
                    SavePath = item//此处随便写，服务器会重定向保存路径。
                };

                metadatas.Add(null);
                fileOperators.Add(new FileOperator());
                requests.Add(fileRequest);
            }

            int num = 0;
            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                bool fin = true;
                var ops = fileOperators.ToArray();
                foreach (var item in ops)
                {
                    if (item.Result.ResultCode == ResultCode.Default)
                    {
                        fin = false;
                    }
                    else
                    {
                        if (item.Result.ResultCode == ResultCode.Error)
                        {
                            int index = fileOperators.IndexOf(item);
                        }

                        Console.WriteLine($"{++num}号文件传输完成，{item.Result}");
                        fileOperators.Remove(item);
                    }
                }

                if (fin)
                {
                    loop.Dispose();
                    Console.WriteLine("传输结束。");
                }
            });
            loopAction.RunAsync();
            fileClient.PushFilesAsync(5, requests.ToArray(), fileOperators.ToArray(), metadatas.ToArray());
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
