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
using RRQMProxy;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCPerformanceClient
{
    public static class RRQMRPCTCP
    {
        public static void Start(int count)
        {
            Console.WriteLine("1.测试Sum");
            Console.WriteLine("2.测试GetAdd");
            Console.WriteLine("3.测试GetBytes");
            Console.WriteLine("4.测试BigString");

            TcpRpcClient client = new TcpRpcClient();
            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");
            client.Setup(config);
            client.Connect("123RPC");
            client.DiscoveryService("RPC");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.Invoke<int>("Sum", InvokeOption.WaitInvoke, i,i);
                                if (rs!= i + i)
                                {
                                    Console.WriteLine("调用结果不一致");
                                }
                                if (i % 1000 == 0)
                                {
                                    Console.WriteLine(i);
                                }
                            }
                        });
                        Console.WriteLine(timeSpan);
                        break;
                    }
                case "2":
                    {
                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.Invoke<GetAddResponse>("GetAdd", InvokeOption.WaitInvoke,new GetAddRequest() { A=i,B=i });
                                if (rs.Result != i + i)
                                {
                                    Console.WriteLine("调用结果不一致");
                                }
                                if (i % 1000 == 0)
                                {
                                    Console.WriteLine(i);
                                }
                            }
                        });
                        Console.WriteLine(timeSpan);
                        break;
                    }
                case "3":
                    {
                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.Invoke<byte[]>("GetBytes", InvokeOption.WaitInvoke,i);//测试10k数据
                                if (rs.Length != i)
                                {
                                    Console.WriteLine("调用结果不一致");
                                }
                                if (i % 1000 == 0)
                                {
                                    Console.WriteLine(i);
                                }
                            }
                        });
                        Console.WriteLine(timeSpan);
                        break;
                    }
                case "4":
                    {
                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.Invoke<string>("GetBigString", InvokeOption.WaitInvoke);
                                if (i % 1000 == 0)
                                {
                                    Console.WriteLine(i);
                                }
                            }
                        });
                        Console.WriteLine(timeSpan);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
