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
    internal class Program
    {
        private static void Main(string[] args)
        {
            CreateFileServicePro();
            Console.ReadKey();
        }

        private static void CreateFileServicePro()
        {
            FileService fileService = new FileService();

            //声明配置
            var config = new FileServiceConfig();

            //继承TcpService配置
            config.ListenIPHosts = new IPHost[] { new IPHost(7790) };//同时监听两个地址
            config.VerifyToken = "FileServer";//连接验证令箭，可实现多租户模式
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