using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.Http.Plugins;
using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //证书在RRQMBox/Ssl证书相关/证书生成.zip  解压获取。
            //然后放在运行目录。
            //最后客户端需要先安装证书。

            var service = new HttpService();

            service.AddPlugin<MyHttpPlug>();

            var config = new RRQMConfig();
            config.UsePlugin()
                .SetReceiveType(ReceiveType.Auto).SetServiceSslOption(new ServiceSslOption()
                {
                    Certificate = new X509Certificate2("RRQMSocket.pfx", "RRQMSocket"),
                    SslProtocols = SslProtocols.Tls12
                })
                .SetListenIPHosts(new IPHost[] { new IPHost(7789) });

            service.Setup(config).Start();
            Console.WriteLine("Http服务器已启动");
            Console.WriteLine("https://127.0.0.1:7789");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 支持GET、Post、Put，Delete，或者其他
    /// </summary>
    class MyHttpPlug : HttpPluginBase
    {
        protected override void OnGet(ITcpClientBase client, HttpContextEventArgs e)
        {
            HttpResponse response = new HttpResponse();
            response.FromText("Success");
            e.Response = response;
            Console.WriteLine("处理完毕");
            e.Handled = true;

            base.OnGet(client, e);
        }
    }
}
