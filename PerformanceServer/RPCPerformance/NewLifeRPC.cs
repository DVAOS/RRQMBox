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
using NewLife.Log;
using NewLife.Net;
using NewLife.Remoting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RPCPerformance
{
    public static class NewLifeRPC
    {
        public static void Start()
        {
            XTrace.UseConsole();
            var netUri = new NetUri(NetType.Tcp, IPAddress.Any, 5001);
            var server = new ApiServer(netUri)
            {
                //Log = XTrace.Log,
                //EncoderLog = XTrace.Log,
                //ShowError = true

                //不输出调用日志
            };
            server.Register<TestController>();
            server.Start();
            Console.WriteLine("NewLifeRPC启动成功");
        }
    }
}
