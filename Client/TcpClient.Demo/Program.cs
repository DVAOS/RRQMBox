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
using RRQMCore.Exceptions;
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RRQMSocket.Helper;
using RRQMCore.Collections.Concurrent;
using System.Linq;

namespace TcpClientDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("选择服务类型");
            Console.WriteLine("1.普通TCP客户端");
            Console.WriteLine("2.TCP性能连接");
            Console.WriteLine("3.BIO单线程拥塞TCP客户端");
            Console.WriteLine("4.发送流量测试");
            Console.WriteLine("5.测试同步发送与接收");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        CreateNormalTcpClient();
                        break;
                    }
                case "2":
                    {
                        TestConnectPerformance();
                        break;
                    }
                case "3":
                    {
                        CreateBIOTcpClient();
                        break;
                    }
                case "4":
                    {
                        CreateFlowPerformance();
                        break;
                    }
                case "5":
                    {
                        CreateSendThenReturnTcpClient();
                        break;
                    }
                default:
                    break;
            }
            Console.ReadKey();
        }

        private static void CreateFlowPerformance()
        {
            Console.WriteLine("测试之前，请确认服务器不处理接收数据，不然无法测试真实水平");
            Console.ReadKey();

            SimpleTcpClient tcpClient = new SimpleTcpClient();

            tcpClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.ReceiveType = ReceiveType.BIO;//拥塞接收
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            byte[] data = new byte[1024 * 1000];
            new Random().NextBytes(data);

            long flow = 0;

            LoopAction loopAction = LoopAction.CreateLoopAction(-1, 1000, (loop) =>
              {
                  Console.WriteLine($"已发送：{(flow / (1024 * 1024.0)).ToString("0.00")}Mb");
                  flow = 0;
              });

            loopAction.RunAsync();

            Task.Run(() =>
            {
                while (true)
                {
                    tcpClient.Send(data);
                    flow += data.Length;
                }
            });
        }

        private static void CreateBIOTcpClient()
        {
            SimpleTcpClient tcpClient = new SimpleTcpClient();

            tcpClient.Connecting += (client, e) =>
            {
                e.DataHandlingAdapter = new NormalDataHandlingAdapter();//从6.1.0开始，适配器最好在此处设置。
                //client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());//直接设置适配器。可以在任何时刻调用。
            };

            tcpClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };

            tcpClient.Received += (client, byteBlock, obj) =>
            {
                //客户端接收信息
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
                Console.WriteLine($"接收：{mes}");
            };

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            config.ReceiveType = ReceiveType.BIO;//拥塞接收
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            tcpClient.Send(Encoding.UTF8.GetBytes("RRQM"));

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tcpClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }

        private static void TestConnectPerformance()
        {
            int count = 1000;
            Console.WriteLine($"即将进行{count}次连接");
            List<SimpleTcpClient> clients = new List<SimpleTcpClient>();
            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    SimpleTcpClient tcpClient = new SimpleTcpClient();

                    clients.Add(tcpClient);

                    //声明配置
                    var config = new TcpClientConfig();
                    config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
                    //载入配置
                    tcpClient.Setup(config);

                    tcpClient.Connect();
                }
            });

            Console.WriteLine($"测试完成，用时:{timeSpan}");
        }

        private static void CreateNormalTcpClient()
        {
            MyTcpClient tcpClient = new MyTcpClient();

            tcpClient.UseReconnection(-1, true);

            tcpClient.Connected += (client, e) =>
            {
                //成功连接到服务器
            };

            tcpClient.Disconnected += (client, e) =>
            {
                //从服务器断开连接，当连接不成功时不会触发。
            };

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                tcpClient.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            }
        }

        private static void CreateSendThenReturnTcpClient()
        {
            SendThenReturnTcpClient tcpClient = new SendThenReturnTcpClient();

            //声明配置
            var config = new TcpClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7789");//远程IPHost
            //载入配置
            tcpClient.Setup(config);

            tcpClient.Connect();

            Console.WriteLine("输入信息，回车发送");
            while (true)
            {
                byte[] data = tcpClient.SendThenReturn(Encoding.UTF8.GetBytes(Console.ReadLine()));
                Console.WriteLine($"同步收到：{Encoding.UTF8.GetString(data)}");
            }
        }
    }

    internal class MyTcpClient : RRQMSocket.TcpClient
    {
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, byteBlock.Len);
            Console.WriteLine($"接收到信息：{mes}");
        }
    }

    /// <summary>
    /// 发送，然后同步等待返回
    /// </summary>
    internal class SendThenReturnTcpClient : TcpClient, IWaitSender
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SendThenReturnTcpClient()
        {
            this.waitData = new WaitData<byte[]>();
        }

        private WaitData<byte[]> waitData;

        private int timeout = 60 * 1000;

        /// <summary>
        /// 超时设置
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                timeout = value;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public byte[] SendThenReturn(byte[] buffer, int offset, int length, CancellationToken token = default)
        {
            lock (this)
            {
                waitData.Reset();
                this.Send(buffer, offset, length);
                this.waitData.SetCancellationToken(token);
                switch (this.waitData.Wait(this.timeout))
                {
                    case WaitDataStatus.SetRunning:
                        return waitData.WaitResult;

                    case WaitDataStatus.Overtime:
                        throw new RRQMTimeoutException();
                    case WaitDataStatus.Canceled:
                        {
                            return default;
                        }
                    case WaitDataStatus.Default:
                    case WaitDataStatus.Disposed:
                    default:
                        throw new RRQMException(RRQMCore.ResType.UnknownError.GetResString());
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public byte[] SendThenReturn(byte[] buffer, CancellationToken token = default)
        {
            return this.SendThenReturn(buffer, 0, buffer.Length, token);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public byte[] SendThenReturn(ByteBlock byteBlock, CancellationToken token = default)
        {
            return this.SendThenReturn(byteBlock.Buffer, 0, byteBlock.Len, token);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<byte[]> SendThenReturnAsync(byte[] buffer, int offset, int length, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                return this.SendThenReturn(buffer, offset, length, token);
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<byte[]> SendThenReturnAsync(byte[] buffer, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                return this.SendThenReturn(buffer, token);
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<byte[]> SendThenReturnAsync(ByteBlock byteBlock, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                return this.SendThenReturn(byteBlock, token);
            });
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="obj"></param>
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            if (this.waitData.Status == WaitDataStatus.Default)
            {
                this.waitData.Set(byteBlock.ToArray());
            }
            else
            {
                //处理数据
            }
        }
    }
}