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
using BeetleX.XRPC.Hosting;
using BeetleX.XRPC.Packets;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCPerformance
{
    /// <summary>
    /// Github:<see href="https://github.com/beetlex-io/XRPC"/>
    /// </summary>
    public static class BeetleXRPC
    {
        public static void Start()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.UseXRPC(s =>
                    {
                        s.ServerOptions.LogLevel = BeetleX.EventArgs.LogType.Error;
                        s.ServerOptions.DefaultListen.Port = 9090;
                        s.RPCOptions.ParameterFormater = new JsonPacket();//default messagepack
                    },
                        typeof(Program).Assembly);
                });
            builder.Build().Run();
        }
    }
}
