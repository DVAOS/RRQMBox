using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RRQMCore.Helper;
using RRQMSocket.FileTransfer;

namespace RRQMService.TCP
{
    public static class TCPDemo
    {
        public static void Start()
        {
            Console.WriteLine("1.IOCP简单TCP服务器（可兼容ClientTest-6）");
            Console.WriteLine("2.BIO简单TCP服务器（可兼容ClientTest-6）");
            Console.WriteLine("3.Select简单TCP服务器（可兼容ClientTest-6）");
            Console.WriteLine("4.TCP连接性能测试服务器");
            Console.WriteLine("5.TCP接收流量测试服务器");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        TCPDemo.StartSimpleTcpService( ReceiveType.IOCP);
                        break;
                    }
                case "2":
                    {
                        TCPDemo.StartSimpleTcpService(ReceiveType.BIO);
                        break;
                    }
                case "3":
                    {
                        TCPDemo.StartSimpleTcpService(ReceiveType.Select);
                        break;
                    }
                case "4":
                    {
                        TCPDemo.StartConnectPerformanceTcpService();
                        break;
                    } 
                case "5":
                    {
                        TCPDemo.StartFlowPerformanceTcpService();
                        break;
                    }
                default:
                    break;
            }
        }
        static void StartSimpleTcpService(ReceiveType receiveType)
        {
            SimpleTcpService service = new SimpleTcpService();

            service.Connected += (client, e) =>{Console.WriteLine($"客户端{client.Name}连接");};

            service.Disconnected += (client, e) =>{Console.WriteLine($"客户端{client.Name}断开连接，原因：{e.Message}"); };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"已从{client.Name}接收到信息：{mes}");//Name即IP+Port
                client.Send(byteBlock);
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = receiveType;
            //载入配置
            service.Setup(config);

            //启动
            service.Start();


            Console.WriteLine($"{receiveType}服务器启动成功");
        }
        static void StartConnectPerformanceTcpService()
        {
            SimpleTcpService service = new SimpleTcpService();

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = ReceiveType.IOCP;
            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1,1000,(loop)=> 
            {
                Console.WriteLine($"在线客户端数量：{service.SocketClients.Count}");
            });

            loopAction.RunAsync();

            Console.WriteLine($"连接测试服务器启动成功");
        }
        
        static void StartFlowPerformanceTcpService()
        {
            SimpleTcpService service = new SimpleTcpService();

            long flow = 0;

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                flow += byteBlock.Len;
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = ReceiveType.IOCP;
            config.BufferLength = 1024 * 1024;
            //载入配置
            service.Setup(config);

            //启动
            service.Start();

            LoopAction loopAction = LoopAction.CreateLoopAction(-1,1000,(loop)=> 
            {
                Console.WriteLine($"接收：{flow}字节，计：{FileUtility.ToFileLengthString(flow)}");
                flow = 0;
            });

            loopAction.RunAsync();

            Console.WriteLine($"流量测试服务器启动成功");
        }
    }
}
