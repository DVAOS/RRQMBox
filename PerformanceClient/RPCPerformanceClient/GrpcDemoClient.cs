using Grpc.Core;
using GrpcServer.Web.Protos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCPerformanceClient
{
    public static class GrpcDemoClient
    {
        public static void Start(int count)
        {
            Console.WriteLine("1.测试Sum");
            Console.WriteLine("2.测试GetBytes");
            Console.WriteLine("3.测试BigString");

            Channel channel = new Channel("127.0.0.1:5555", ChannelCredentials.Insecure);
            var client = new TestGrpcService.TestGrpcServiceClient(channel);

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        var rs = client.GetAdd(new GrpcGetAddRequest() {A=10,B=20 });//试调一次，保持在线

                        TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var rs = client.GetAdd(new GrpcGetAddRequest() { A = i, B = i });
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
                default:
                    break;
            }
            channel.ShutdownAsync().Wait();
        }
    }
}
