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
using RRQMCore;
using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolServiceDemo
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择类型");
            Console.WriteLine("1.简单协议接收");
            Console.WriteLine("2.流数据接收");
            Console.WriteLine("3.测试10000_ProtocolSubscriber");
            Console.WriteLine("4.测试10000_ProtocolSubscriber_Then_Return");
            Console.WriteLine("5.测试Channel");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Test_SimpleProtocolService();
                        break;
                    }
                case "2":
                    {
                        Test_ReceiveStream();
                        break;
                    }
                case "3":
                    {
                        Test_ProtocolSubscriber();
                        break;
                    }
                case "4":
                    {
                        Test_ProtocolSubscriber_Then_Return();
                        break;
                    }
                case "5":
                    {
                        Test_Channel();
                        break;
                    }
                default:
                    break;
            }

            Console.ReadKey();
        }

        private static void Test_Channel()
        {
            SimpleProtocolService protocolService = CreateSimpleProtocolService();

            protocolService.Connecting += (client, eOption) =>
            {
                //为初始化配置
                //因为发送与接收太频繁，所以数据处理适配器应当选择具有解决粘包、分包能力的。
                client.SetDataHandlingAdapter(new FixedHeaderDataHandlingAdapter());
            };

            protocolService.Connected += (client, e) =>
            {
                RRQMCore.Run.EasyAction.DelayRun(1, () =>
                 {
                     Task.Run(() =>
                     {
                         Channel channel = client.CreateChannel(10);//创建指定ID的通道。

                         //Channel channel = client.CreateChannel();//创建ID随机的通道。
                         Console.WriteLine($"成功创建通道，请使用{channel.ID}订阅");

                         while (channel.MoveNext())//线程阻塞接收，直至接收完成或错误
                         {
                             byte[] data = channel.GetCurrent();//获取当前数据对象，此操作相当于把ByteBlock进行ToArray
                             Console.WriteLine($"已收到数据，长度为:{data.Length}");

                             //下列注释内容为高效处理数据，省略了ByteBlock进行ToArray的操作。

                             ////或者直接获取内存块
                             //ByteBlock byteBlock = channel.GetCurrentByteBlock();

                             //byteBlock.Pos = 6;//先预设流位置为6
                             //if (byteBlock.TryReadBytesPackageInfo(out int pos,out int length))//获取数据包信息
                             //{

                             ////真实数据则是从byteBlock.Buffer的pos，长度为length
                             //Console.WriteLine($"已收到数据，长度为:{length}");
                             //}

                             //byteBlock.SetHolding(false);//必须手动释放！！！不然内存池会一直创建，性能大大折扣。
                         }

                         //代码执行到此处时，意味着通道接收已结束，可通过channel.Status获取最后的状态。
                         Console.WriteLine($"已结束接收，状态为{channel.Status}");
                     });
                 });
            };

        }

        private static void Test_ProtocolSubscriber_Then_Return()
        {
            SimpleProtocolService protocolService = CreateSimpleProtocolService();

            protocolService.Connecting += (client, eOption) =>
            {
                //为初始化配置
                //因为发送与接收太频繁，所以数据处理适配器应当选择具有解决粘包、分包能力的。
                client.SetDataHandlingAdapter(new FixedHeaderDataHandlingAdapter());

                //此处直接订阅协议为10000，实际上订阅可以随时进行，取消订阅用RemoveProtocolSubscriber
                client.AddProtocolSubscriber(new ProtocolSubscriber(10000, (Subscriber, e) =>
                {
                    //Protocol系的数据，前两个字节为协议，所以真实数据应该偏移2个单位。
                    string mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 2, e.ByteBlock.Len - 2);
                    Console.WriteLine($"已从{client.Name}接收到订阅处理的信息，信息：{mes}");

                    //通过订阅器直接发送，效果等于“10000+数据”。
                    Subscriber.Send(Encoding.UTF8.GetBytes($"已收到数据：{mes}"));

                    //处理完数据后，如果不想数据再被其他端处理，则设为已处理即可。
                    e.Handled = true;

                }));
            };
        }

        private static void Test_ProtocolSubscriber()
        {
            SimpleProtocolService protocolService = CreateSimpleProtocolService();

            protocolService.Connecting += (client, eOption) =>
            {
                //为初始化配置
                //因为发送与接收太频繁，所以数据处理适配器应当选择具有解决粘包、分包能力的。
                client.SetDataHandlingAdapter(new FixedHeaderDataHandlingAdapter());

                //此处直接订阅协议为10000，实际上订阅可以随时进行，取消订阅用RemoveProtocolSubscriber
                client.AddProtocolSubscriber(new ProtocolSubscriber(10000, (Subscriber, e) =>
                 {
                     //Protocol系的数据，前两个字节为协议，所以真实数据应该偏移2个单位。
                     string mes = Encoding.UTF8.GetString(e.ByteBlock.Buffer, 2, e.ByteBlock.Len - 2);
                     Console.WriteLine($"已从{client.Name}接收到订阅处理的信息，信息：{mes}");

                     //通过订阅器直接发送，效果等于“10000+数据”。
                     //Subscriber.Send(Encoding.UTF8.GetBytes($"已收到数据：{mes}"));

                     //处理完数据后，如果不想数据再被其他端处理，则设为已处理即可。
                     e.Handled = true;
                 }));
            };
        }

        private static void Test_SimpleProtocolService()
        {
            SimpleProtocolService protocolService = CreateSimpleProtocolService();
            protocolService.Connecting += (client, eOption) =>
            {
                //为初始化配置
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
            };

        }

        private static void Test_ReceiveStream()
        {
            SimpleProtocolService protocolService = CreateSimpleProtocolService();

            protocolService.Connecting += (client, eOption) =>
            {
                //为初始化配置
                //在测试流接收时，因为发送与接收太频繁，所以数据处理适配器应当选择具有解决粘包、分包能力的。
                client.SetDataHandlingAdapter(new FixedHeaderDataHandlingAdapter());
            };

            protocolService.BeforeReceiveStream += (socketClient, e) =>
            {
                e.Bucket = new MemoryStream();//此处用MemoryStream作为接收容器，也可以使用FileStream。
                e.IsPermitOperation = true;//允许接收该流
                Metadata metadata = e.Metadata;//获取元数据
                StreamOperator streamOperator = e.StreamOperator;//获取操作器，可用于取消任务，获取进度等。

                Console.WriteLine("设置最大传输速度为1024byte");
                streamOperator.SetMaxSpeed(1024);

                Console.WriteLine("5秒后设置为5Mb");
                RRQMCore.Run.EasyAction.DelayRun(5, () =>
                 {
                     streamOperator.SetMaxSpeed(1024 * 1024 * 5);
                 });

                Task.Run(async () =>
                {
                    while (streamOperator.Result.ResultCode ==  ResultCode.Default)
                    {
                        Console.WriteLine($"速度={streamOperator.Speed()},进度={streamOperator.Progress}");

                        await Task.Delay(1000);
                    }

                    Console.WriteLine($"从循环传输结束,状态={streamOperator.Result}");
                });
                Console.WriteLine("开始接收流数据");
            };


            protocolService.ReceivedStream += (socketClient, e) =>
            {
                //此处不管传输成功与否，都会执行，具体状态通过e.Status判断。

                if (e.Result.ResultCode ==  ResultCode.Success)
                {
                    e.Bucket.Dispose();//必须手动释放流数据。
                }

                Console.WriteLine($"从ReceivedStream传输结束,状态={e.Result}");
            };
        }

        private static SimpleProtocolService CreateSimpleProtocolService()
        {
            SimpleProtocolService service = new SimpleProtocolService();

            service.Connected += (client, e) =>
            {
                //有客户端连接
            };

            service.Disconnected += (client, e) =>
            {
                //有客户端断开连接
            };


            service.Received += (client, protocol, byteBlock) =>
            {
                //从客户端收到信息

                //Protocol系的数据，前两个字节为协议，所以真实数据应该偏移2个单位。
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 2, byteBlock.Len - 2);

                if (protocol == null)
                {
                    Console.WriteLine($"已从{client.Name}接收默认协议信息：{mes}");//意味着发送方是直接使用Send发送
                }
                else
                {
                    //运行到此处的数据，意味着该数据既不是系统协议数据，也没有订阅该协议数据。可以自由处理。
                    Console.WriteLine($"已从{client.Name}接收到未订阅处理的信息，协议为：‘{protocol}’，信息：{mes}");
                }
            };


            //声明配置
            var config = new ProtocolServiceConfig();
            config.ListenIPHosts = new IPHost[] { new IPHost("127.0.0.1:7789"), new IPHost(7790) };//同时监听两个地址

            //继承TokenService配置
            config.VerifyToken = "Token";//连接验证令箭，可实现多租户模式
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //载入配置                                                       
            service.Setup(config);

            //启动

            try
            {
                service.Start();
                Console.WriteLine("Protocol服务器启动成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return service;
        }
    }
}
