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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.NAT
{
    public static class NATDemo
    {
        public static void Start()
        {
            NATService service = new NATService();

            var config = new NATServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(7788) };
            config.TargetIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost("127.0.0.1:7790") };
            config.NATMode = NATMode.TwoWay;

            service.Setup(config);
            service.Start();

            Console.WriteLine("转发服务器已启动。已将7788端口转发到127.0.0.1:7789与127.0.0.1:7790地址");
        }
    }
}
