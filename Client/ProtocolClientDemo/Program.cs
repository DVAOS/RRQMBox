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
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolClientDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("选择类型");
            Console.WriteLine("1.协议发送");
            Console.WriteLine("2.流数据发送");
            Console.WriteLine("3.测试10000_ProtocolSubscriber_Send");
            Console.WriteLine("4.测试Test_Protocol_10000_Send_Then_Return");
            Console.WriteLine("5.性能测试Test_Protocol_10000_Send_Then_Return");
            Console.WriteLine("6.测试Test_Channel");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Test_ProtocolSend();
                        break;
                    }
                case "2":
                    {
                        Test_StreamSend();
                        break;
                    }
                case "3":
                    {
                        Test_Protocol_10000_Send();
                        break;
                    }
                case "4":
                    {
                        Test_Protocol_10000_Send_Then_Return();
                        break;
                    } 
                case "5":
                    {
                        Test_Protocol_10000_Send_Then_Return_Performance();
                        break;
                    } 
                case "6":
                    {
                        Test_Channel();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        static void Test_Channel()
        {
            SimpleProtocolClient protocolClient = CreateSimpleProtocolClient(new FixedHeaderDataHandlingAdapter());

            Console.WriteLine("输入Channel的ID订阅，然后读写。");

            int id = int.Parse(Console.ReadLine());

            //必须知道接收方已创建通道的ID
            if (protocolClient.TrySubscribeChannel(id, out Channel channel))
            {
                byte[] data = new byte[1024*1024];
                for (int i = 0; i < 10; i++)
                {
                    channel.Write(data);//持续写入（发送）
                }
                channel.Complete();//最后调用完成
                //channel.Cancel();//或调用取消
                //channel.Dispose();//或销毁
                Console.WriteLine("发送完成");
            }
            else
            {
                Console.WriteLine("未找到该ID对应的Channel");
            }
        }

        static void Test_Protocol_10000_Send_Then_Return_Performance()
        {
            SimpleProtocolClient protocolClient = CreateSimpleProtocolClient(new FixedHeaderDataHandlingAdapter());

            WaitSenderSubscriber subscriber = new WaitSenderSubscriber(10000);
            protocolClient.AddProtocolSubscriber(subscriber);

            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 10000; i++)
                {
                    byte[] data = subscriber.SendThenReturn(Encoding.UTF8.GetBytes(i.ToString()));
                    if (data != null)
                    {
                        if (i % 100 == 0)
                        {
                            Console.WriteLine(Encoding.UTF8.GetString(data, 0, data.Length));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"第{i}次失败");
                    }
                }
            });

            Console.WriteLine(timeSpan);
        }

        private static void Test_Protocol_10000_Send_Then_Return()
        {
            SimpleProtocolClient protocolClient = CreateSimpleProtocolClient(new FixedHeaderDataHandlingAdapter());
            Console.WriteLine("输入信息，然后Enter发送");

            WaitSenderSubscriber waitSenderSubscriber = new WaitSenderSubscriber(10000);

            protocolClient.AddProtocolSubscriber(waitSenderSubscriber);
            while (true)
            {
                byte[] data = waitSenderSubscriber.SendThenReturn(Encoding.UTF8.GetBytes(Console.ReadLine()));
                Console.WriteLine($"{Encoding.UTF8.GetString(data)}");
            }
        }

        private static void Test_Protocol_10000_Send()
        {
            SimpleProtocolClient protocolClient = CreateSimpleProtocolClient(new FixedHeaderDataHandlingAdapter());
            Console.WriteLine("输入信息，然后Enter发送");
            while (true)
            {
                protocolClient.Send(10000, Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }

        private static void Test_StreamSend()
        {
            //在测试流接收时，因为发送与接收太频繁，所以数据处理适配器应当选择具有解决粘包、分包能力的。
            SimpleProtocolClient protocolClient = CreateSimpleProtocolClient(new FixedHeaderDataHandlingAdapter());

            byte[] data = new byte[1024 * 1024 * 50];
            new Random().NextBytes(data);
            MemoryStream stream = new MemoryStream(data);
            stream.Position = 0;

            Console.WriteLine($"即将发送流数据，长度为:{stream.Length}");

            StreamOperator streamOperator = new StreamOperator();
            streamOperator.PackageSize = 1024 * 64;//分包长度
            streamOperator.MaxSpeed = 1024 * 1024 * 5;//最大传输值
            
            //streamOperator.Cancel();//随时取消传输
            
            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (a) =>
            {
                if (streamOperator.Status != ChannelStatus.Default)
                {
                    a.Dispose();
                }
                Console.WriteLine($"速度：{streamOperator.Speed()},进度：{streamOperator.Progress}");
            });
            loopAction.RunAsync();

            Metadata metadata = new Metadata();//将键值对的元数据传到接收端
            metadata.Add("1", "1");
            metadata.Add("2", "2");

            //该方法会阻塞，直到结束
            AsyncResult asyncResult = protocolClient.SendStream(stream, streamOperator, metadata);
            Console.WriteLine($"状态：{asyncResult.IsSuccess}，信息：{asyncResult.Message}");

        }

        private static void Test_ProtocolSend()
        {
            SimpleProtocolClient protocolClient = CreateSimpleProtocolClient(new NormalDataHandlingAdapter());
            Console.WriteLine("输入信息，然后Enter发送，则按空协议发送");
            Console.WriteLine("输入short类型协议，空格，然后Enter发送，则按输入协议发送");
            while (true)
            {
                string strEnter = Console.ReadLine();
                string[] p_m = strEnter.Split(' ');
                if (p_m.Length == 2)
                {

                    protocolClient.Send(short.Parse(p_m[0]), Encoding.UTF8.GetBytes(p_m[1]));
                }
                else
                {
                    protocolClient.Send(Encoding.UTF8.GetBytes(strEnter));
                }

            }
        }

        private static SimpleProtocolClient CreateSimpleProtocolClient(DataHandlingAdapter adapter)
        {
            SimpleProtocolClient protocolClient = new SimpleProtocolClient();

            protocolClient.Received += (client, protocol, byteBlock) =>
            {
                //从服务器收到信息

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
            var config = new ProtocolClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.BufferLength = 1024 * 64;//缓存池容量
            config.BytePoolMaxSize = 512 * 1024 * 1024;//单个线程内存池容量
            config.BytePoolMaxBlockSize = 20 * 1024 * 1024;//单个线程内存块限制
            config.Logger = new Log();//日志记录器，可以自行实现ILog接口。
            config.SeparateThreadReceive = false;//独立线程接收，当为true时可能会发生内存池暴涨的情况
            config.DataHandlingAdapter = adapter;//设置数据处理适配器
            config.OnlySend = false;//仅发送，即不开启接收线程，同时不会感知断开操作。
            config.SeparateThreadSend = false;//在异步发送时，使用独立线程发送

            //继承TokenClient配置
            config.VerifyToken = "Token";//连接验证令箭
            config.VerifyTimeout = 3 * 1000;//验证3秒超时

            //从Protocpl配置
            config.HeartbeatFrequency = 5000;//每5秒心跳


            //载入配置
            protocolClient.Setup(config);

            protocolClient.Connect();
            return protocolClient;
        }
    }
}
