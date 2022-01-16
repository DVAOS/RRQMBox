using Grpc.Core;
using GrpcServer.Web.Protos;
using System;

namespace GrpcClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            Channel channel = new Channel("127.0.0.1:5555", ChannelCredentials.Insecure);
            var client = new TestGrpcService.TestGrpcServiceClient(channel);

            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
              {
                  for (int i = 0; i < 100000; i++)
                  {
                      var replay = client.Add(new AddPS() { A = 10, B = 20 });
                      if (i % 1000 == 0)
                      {
                          Console.WriteLine(i);
                      }
                  }
              });

            Console.WriteLine(timeSpan);
            Console.ReadKey();

            channel.ShutdownAsync().Wait();
            Console.ReadLine();
        }
    }
}
