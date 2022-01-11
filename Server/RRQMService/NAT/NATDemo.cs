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
            config.TargetIPHost = new IPHost("127.0.0.1:7789");

            service.Setup(config);
            service.Start();

            Console.WriteLine("转发服务器已启动。已将7788端口转发到127.0.0.1:7789地址");
        }
    }
}
