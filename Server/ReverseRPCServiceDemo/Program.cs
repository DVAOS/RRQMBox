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
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;

namespace ReverseRPCServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpRpcParser tcpRPCParser = new TcpRpcParser();
            tcpRPCParser.Connected += TcpRPCParser_Connected;
            //创建配置
            var config = new TcpRpcParserConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost(7789) };//监听一个IP地址
            config.VerifyToken = "123RPC";//令箭值
            config.ProxyToken = "RPC";//默认服务代理令箭
            //载入配置
            tcpRPCParser.Setup(config);

            //启动服务
            tcpRPCParser.Start();
            Console.WriteLine("服务已启动");
            Console.ReadKey();
            string[] ids = tcpRPCParser.SocketClients.GetIDs();
            if (tcpRPCParser.TryGetSocketClient(ids[0], out RpcSocketClient socketClient))
            {
                TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                  {
                      for (int i = 0; i < 100000; i++)
                      {
                          if (i % 1000 == 0)
                          {
                              Console.WriteLine(i);
                          }
                          int value = socketClient.Invoke<int>("ConPerformance", InvokeOption.WaitInvoke, i);
                          if (value != i + 1)
                          {
                              Console.WriteLine("调用结果不一致");
                          }
                      }
                  });
                Console.WriteLine($"测试完成，用时{timeSpan}");
            }
            Console.ReadKey();
        }

        private static void TcpRPCParser_Connected(RpcSocketClient client, MesEventArgs e)
        {
            Console.WriteLine("有客户端已连接，按任意键测试反向调用");
        }
    }
}
