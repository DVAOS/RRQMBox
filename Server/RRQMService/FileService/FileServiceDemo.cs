using RRQMSocket;
using RRQMSocket.FileTransfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            fileService.BeforeFileTransfer += (client, e) =>
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Received", e.FileInfo.FileName);
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                e.FileRequest.SavePath = path;
            };
        }

        private static FileService CreateFileServicePro()
        {
            FileService fileService = new FileService();

            //声明配置
            var config = new FileServiceConfig();

            //继承TcpService配置
            config.ListenIPHosts = new IPHost[] { new IPHost(7789) };//同时监听两个地址
            config.VerifyToken = "FileServer";//连接验证令箭，可实现多租户模式
            fileService.Setup(config);
            fileService.Start();
            Console.WriteLine("启动成功");
            return fileService;
        }
    }
}
