using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Controllers;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RpcService rpcService = new RpcService();
            rpcService.AddRpcParser("webApiParser", new WebApiParserPlugin() { ProxyToken="RPC" });
            rpcService.RegisterServer(new WeatherForecastController(null));//注册服务，此处实例不会用于调用。可以认为没意义

            //生成的WebApi的本地代理文件。
            //RpcProxyInfo proxyInfo = rpcService.GetProxyInfo(RpcType.WebApi, "RPC");
            //string code = CodeGenerator.ConvertToCode("RRQM", proxyInfo.Codes);

            rpcService.ShareProxy(new IPHost(8848));//分享远程代理
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
