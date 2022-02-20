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
using NewLife.Log;
using NewLife.Remoting;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCPerformanceClient
{
    public static class NewLifeRPC
    {
        public static void Start(int count)
        {
            Console.WriteLine("1.测试Sum");
            Console.WriteLine("2.测试GetBytes");
            Console.WriteLine("3.测试BigString");
            Console.WriteLine("选择数字，测试1w次调用用时。");
            var client = new ApiClient("tcp://127.0.0.1:5001")
            {
                Log = XTrace.Log,
                EncoderLog = XTrace.Log
            };



            switch (Console.ReadLine())
            {
                case "1":
                    {
                        var rs = client.Invoke<Int32>("Test/Sum", new { a = 10, b = 20 });//先试调一下，保证已经建立了完整的连接

                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.Invoke<Int32>("Test/Sum", new { a = i, b = i });
                                if (rs != i + i)
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
                        var rs = client.Invoke<byte[]>("Test/GetBytes", new { a = 10 });//先试调一下，保证已经建立了完整的连接

                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.Invoke<byte[]>("Test/GetBytes", new { a = i });//测试10k数据
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
                case "3":
                    {
                        var rs = client.Invoke<string>("Test/GetBigString");//先试调一下，保证已经建立了完整的连接
                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.Invoke<string>("Test/GetBigString");
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
