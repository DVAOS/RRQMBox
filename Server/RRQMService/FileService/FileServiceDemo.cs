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
using RRQMSocket;
using RRQMSocket.FileTransfer;
using System;
using System.IO;

namespace RRQMService.FileServiceN
{
    public static class FileServiceDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.批量文件服务器");
            switch (Console.ReadLine())
            {
                case "1":
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
            FileService fileService = CreateFileServicePro();
            fileService.FileTransfering += (client, e) =>
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Received", e.FileInfo.FileName);
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                e.FileRequest.SavePath = path;
            };
            fileService.FileTransfered += FileService_FinishedFileTransfer;
        }

        private static void FileService_FinishedFileTransfer(FileSocketClient client, FileTransferStatusEventArgs e)
        {

        }

        private static FileService CreateFileServicePro()
        {
            FileService fileService = new FileService();

            fileService.Setup(new RRQMConfig()
                .SetListenIPHosts(new IPHost[] { new IPHost(7789) })
                .SetBufferLength(1024 * 1024)
                .SetThreadCount(1)
                .SetVerifyToken("FileServer"))
                .Start();
            Console.WriteLine("启动成功");
            return fileService;
        }
    }
}
