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
using BeetleX.XRPC.Clients;
using BeetleX.XRPC.Packets;
using GrpcServer.Web.Protos;
using RRQMProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RPCPerformanceClient
{
    public static class BeetleXRPC
    {
        public static void Start(int count)
        {
            Console.WriteLine("1.测试Sum");
            Console.WriteLine("2.测试GetAdd");
            Console.WriteLine("3.测试GetBytes");
            Console.WriteLine("4.测试BigString");


            XRPCClient client = new XRPCClient("localhost", 9090);
            client.Options.ParameterFormater = new MsgPacket();//default messagepack
            ITestTaskController testController = client.Create<ITestTaskController>();

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        var rs = testController.Sum(10,20);//试调一次，保持在线
                        rs.Wait();

                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs  =testController.Sum(i,i);
                                rs.Wait();
                                if (rs.Result!=i+i)
                                {
                                    Console.WriteLine("调用结果不一致");
                                }

                                if (i%1000==0)
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
                        var rs = testController.GetBytes(10);//试调一次，保持在线
                        rs.Wait();

                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = testController.GetAdd(new GetAddRequest() { A=i,B=i});
                                rs.Wait();
                                if (rs.Result.Result!=i)
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
                        var rs = testController.GetBytes(10);//试调一次，保持在线
                        rs.Wait();

                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = testController.GetBytes(i);//测试10k数据
                                rs.Wait();
                                if (rs.Result.Length != i)
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
                                var rs = testController.GetBigString();

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

    public interface ITestTaskController
    {
        Task<int> Sum(int a, int b);

        Task<GetAddResponse> GetAdd(GetAddRequest request);

        Task<byte[]> GetBytes(int length);

        Task<string> GetBigString();
    }
}
