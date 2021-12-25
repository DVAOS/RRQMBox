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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RRQM.WebApplication.Controllers;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using System;

namespace RRQM.WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RPCService rpcService = new RPCService();

            JsonRpcParser jsonRpcParser = new JsonRpcParser();
            jsonRpcParser.Setup(7705);
            jsonRpcParser.Start();

            rpcService.AddRPCParser("jsonRpcParser ", jsonRpcParser);
            Console.WriteLine("jsonRpc解析器已添加");

            rpcService.RegisterServer(new WeatherForecastController(null));

            Console.WriteLine("RPC服务已启动");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}