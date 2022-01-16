using Grpc.Core;
using GrpcServer.Web.Protos;
using RPCPerformance.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPCPerformance
{
    public static class GrpcDemo
    {
        public static void Start()
        {
            Server server = new Server
            {
                Services = { TestGrpcService.BindService(new TestService()) },
                Ports = { new ServerPort("localhost", 5555, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine("Grpc启动");
        }
    }
}
