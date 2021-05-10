//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.TestTcpClient
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            Console.ReadKey();

            for (int i = 0; i < 1; i++)
            {
                ThreadPool.QueueUserWorkItem((object o) =>
                {
                    MyTcpClientTest();
                });
            }

            Console.ReadKey();
        }

        private static TimeSpan time;
        private static int count;

        private static void MyTcpClientTest()
        {
            MyTcpClient client = new MyTcpClient();

            client.Connect(new IPHost("127.0.0.1:7789"));
            Console.WriteLine("连接成功");
            byte[] datas = Encoding.UTF8.GetBytes("若汝棋茗");

            TimeSpan timeSpan = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 1; i++)
                {
                    client.Send(datas);
                }
            });
            lock (typeof(ClientProgram))
            {
                count++;
                time += timeSpan;
                Console.WriteLine(TimeSpan.FromTicks(time.Ticks / count));
            }
        }
    }
}
