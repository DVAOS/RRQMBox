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
using RRQMCore.ByteManager;
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServiceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择服务类型");
            Console.WriteLine("1.普通TCP服务器");
            Console.WriteLine("2.简单TCP服务器");
            Console.WriteLine("3.TCP同步流服务器");
            Console.WriteLine("4.BIO独立线程接收TCP服务器");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateNormalTcpService();
                        break;
                    }
                case "2":
                    {
                        CreateSimpleTcpService();
                        break;
                    }
                case "3":
                    {
                        CreateStreamTcpService();
                        break;
                    }
                case "4":
                    {
                        CreateBIOTcpService();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void CreateBIOTcpService()
        {
            SimpleTcpService service = new SimpleTcpService();

            service.Connected += (client, e) =>
            {
                //client.Send(Encoding.UTF8.GetBytes("来了，老弟"));
                //有客户端连接
            };

            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };


            service.Connecting += (client, e) =>
            {
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
                //client.Send(Encoding.UTF8.GetBytes("滚"));
                //e.IsPermitOperation = false;//是否允许连接
            };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                Console.WriteLine($"独立线程已从{client.Name}接收到信息：{mes}");//Name即IP+Port

                client.Send(Encoding.UTF8.GetBytes($"响应信息：{mes}"));
            };


            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = ReceiveType.BIO;//独立线程

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine("独立拥塞服务器启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void CreateStreamTcpService()
        {
            SimpleTcpService service = new SimpleTcpService();

            service.Connected += (client, e) =>
            {
                //client.Send(Encoding.UTF8.GetBytes("来了，老弟"));
                //有客户端连接

                NetworkStream networkStream = client.GetNetworkStream();

                RRQMCore.Run.LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
                  {
                      if (networkStream.CanWrite)
                      {
                          try
                          {
                              byte[] data = Encoding.UTF8.GetBytes("你好");
                              networkStream.Write(data,0,data.Length);
                          }
                          catch (Exception)
                          {

                          }
                         
                      }
                      else
                      {
                          loop.Dispose();
                      }
                  });

                loopAction.RunAsync();

                Task.Run(() =>
                {
                    while (true)
                    {
                        byte[] buffer = new byte[1024];
                        if (networkStream.CanRead)
                        {
                            int r = networkStream.Read(buffer,0,buffer.Length);
                            if (r == 0)
                            {
                                break;
                            }
                            Console.WriteLine("从流中接收：" + Encoding.UTF8.GetString(buffer, 0, r));
                        }
                        else
                        {
                            break;
                        }
                    }
                });
            };

            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };


            service.Connecting += (client, e) =>
            {
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
                //client.Send(Encoding.UTF8.GetBytes("滚"));
                //e.IsPermitOperation = false;//是否允许连接
            };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                Console.WriteLine($"IOCP已从{client.Name}接收到信息：{mes}");//Name即IP+Port

                client.Send(Encoding.UTF8.GetBytes($"响应信息：{mes}"));
            };


            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址
            config.ReceiveType = ReceiveType.NetworkStream;

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine("流服务器启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        private static void CreateSimpleTcpService()
        {
            SimpleTcpService service = new SimpleTcpService();


            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
            {
                Console.WriteLine($"客户端数量：{service.SocketClients.Count}");
            });

            loopAction.RunAsync();

            service.Connected += (client, e) =>
            {
                client.Send(Encoding.UTF8.GetBytes("来了，老弟"));
                //有客户端连接
            };

            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };


            service.Connecting += (client, e) =>
            {
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
                //client.Send(Encoding.UTF8.GetBytes("滚"));
                //e.IsPermitOperation = false;//是否允许连接
            };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                Console.WriteLine($"已从{client.Name}接收到信息：{mes}");//Name即IP+Port

                client.Send(Encoding.UTF8.GetBytes($"已收到信息：{mes}"));
            };


            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine("简单服务器启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }


        private static void CreateNormalTcpService()
        {
            MyTcpService service = new MyTcpService();

            service.Connected += (client, e) =>
            {
                //有客户端连接
            };
            service.Connecting += (client, e) =>
            {
                //为初始化配置
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
            };
            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };

            //声明配置
            var config = new TcpServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine("普通服务器启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

    }

    public class MyTcpService : TcpService<MySocketClient>
    {
        protected override void OnConnecting(MySocketClient socketClient, ClientOperationEventArgs e)
        {
            //socketClient.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//直接对数据处理器赋值，立即生效
            //或

            //携带式赋值，本事件触发完成生效
            e.DataHandlingAdapter = new NormalDataHandlingAdapter();
            e.IsPermitOperation = true;//决定允不允许连接
            base.OnConnecting(socketClient, e);
        }
    }

    public class MySocketClient : SocketClient
    {
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            //string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
            //Console.WriteLine($"已从{this.Name}接收到信息：{mes}");//Name即IP+Port

            //this.Send(Encoding.UTF8.GetBytes($"已收到信息：{mes}"));
        }
    }

}
