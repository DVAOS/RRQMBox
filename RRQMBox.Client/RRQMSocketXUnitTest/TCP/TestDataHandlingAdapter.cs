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
using RRQMSocket;
using System.Text;
using System.Threading;
using Xunit;

namespace RRQMSocketXUnitTest.TCP
{
    public class TestDataHandlingAdapter
    {
        [Fact]
        public void FixedHeaderShouldBeOk()
        {
            int inputCount = 100000;
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedHeaderDataHandlingAdapter(), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             new Log(), //设置日志
             1000);//用BufferLength模拟粘包，分包

            byte[] data = Encoding.UTF8.GetBytes("RRQM");

            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    tester.SimSend(data);//此处模拟发送
                }
            }

            Thread.Sleep(2000);
            Assert.Equal(inputCount, outputCount);
        }

        [Fact]
        public void FixedSizeShouldBeOk()
        {
            int inputCount = 100000;
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedSizeDataHandlingAdapter(1024), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             new Log(), //设置日志
             1000);//用BufferLength模拟粘包，分包

            byte[] data = Encoding.UTF8.GetBytes("RRQM");

            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    tester.SimSend(data);//此处模拟发送
                }
            }
            Thread.Sleep(2000);
            Assert.Equal(inputCount, outputCount);
        }

        [Fact]
        public void TerminatorShouldBeOk()
        {
            int inputCount = 100000;
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new TerminatorDataHandlingAdapter(1024, "\r\n"), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             new Log(), //设置日志
             1000);//用BufferLength模拟粘包，分包

            byte[] data = Encoding.UTF8.GetBytes("RRQM");

            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    tester.SimSend(data);//此处模拟发送
                }
            }
            Thread.Sleep(2000);
            Assert.Equal(inputCount, outputCount);
        }
    }
}