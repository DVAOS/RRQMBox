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
using RRQMSocket;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Threading;

namespace InvokeRpcFromNormal
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TcpRpcClient client = new TcpRpcClient();

            var config = new TcpRpcClientConfig();
            config.RemoteIPHost = new IPHost("127.0.0.1:7794");
            config.ProxyToken = "RPC";

            //config.SerializationSelector = new MySerializationSelector();

            client.Setup(config);

            try
            {
                //1.先连接
                client.Connect();
                Console.WriteLine("连接成功");

                //2.然后发现服务
                MethodItem[] methodItems = client.DiscoveryService();
                Console.WriteLine("服务发现成功");

                foreach (var item in methodItems)
                {
                    Console.WriteLine($"服务{item.ServerName}中的‘{item.Method}’可以调用");
                }

                Console.WriteLine("按任意键调用TestOne");
                Console.ReadKey();

                //3.调用

                InvokeOption invokeOption = new InvokeOption();
                invokeOption.SerializationType = (RRQMCore.Serialization.SerializationType)4;

                //invokeOption.SerializationType = RRQMCore.Serialization.SerializationType.Json;
                //invokeOption.SerializationType = RRQMCore.Serialization.SerializationType.SystemBinary;
                //invokeOption.SerializationType = RRQMCore.Serialization.SerializationType.Xml;

                invokeOption.FeedbackType = FeedbackType.WaitInvoke;
                //invokeOption.FeedbackType = FeedbackType.OnlySend;
                //invokeOption.FeedbackType = FeedbackType.WaitSend;

                invokeOption.Timeout = 1000 * 10;//10秒后无反应，则抛出RRQMTimeoutException异常

                CancellationTokenSource tokenSource = new CancellationTokenSource();
                invokeOption.CancellationToken = tokenSource.Token;

                //tokenSource.Cancel();//调用时取消任务

                invokeOption.InvokeType = RRQMSocket.RPC.InvokeType.CustomInstance;
                //invokeOption.InvokeType = RRQMSocket.RPC.InvokeType.GlobalInstance;
                //invokeOption.InvokeType = RRQMSocket.RPC.InvokeType.NewInstance;

                string returnString = client.Invoke<string>("TestOne", invokeOption, "10");

                Console.WriteLine($"调用成功，结果={returnString}");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}