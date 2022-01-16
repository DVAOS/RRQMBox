using Grpc.Core;
using GrpcConsoleApp.Services;
using GrpcServer.Web.Protos;
using System;

namespace GrpcConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 5555;
            Server server = new Server
            {
                Services = { TestGrpcService.BindService(new TestService()) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine($"Greeter Server Listening on port {port}");
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();

            server.ShutdownAsync().Wait();

        }
    }
}
