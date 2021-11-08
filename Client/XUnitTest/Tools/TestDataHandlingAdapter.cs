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
using RRQMSocket.Http;
using System.Text;
using System.Threading;
using Xunit;

namespace RRQMSocketXUnitTest.Tools
{
    public class TestDataHandlingAdapter
    {
        [Theory]
        [InlineData(10000,10)]
        [InlineData(10000,100)]
        [InlineData(10000,1000)]
        public void FixedHeaderShouldBeOk(int inputCount,int bufferLength)
        {
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedHeaderDataHandlingAdapter(), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             new Log(), //设置日志
             bufferLength);//用BufferLength模拟粘包，分包

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

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void FixedSizeShouldBeOk(int inputCount, int bufferLength)
        {
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedSizeDataHandlingAdapter(1024), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             new Log(), //设置日志
             bufferLength);//用BufferLength模拟粘包，分包

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

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void TerminatorShouldBeOk(int inputCount, int bufferLength)
        {
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new TerminatorDataHandlingAdapter(1024, "\r\n"), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             new Log(), //设置日志
             bufferLength);//用BufferLength模拟粘包，分包

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

        [Theory]
        [InlineData(10000, 10, HttpType.Server)]
        [InlineData(10000, 100, HttpType.Server)]
        [InlineData(10000, 1000, HttpType.Server)]
        [InlineData(10000, 10, HttpType.Client)]
        [InlineData(10000, 100, HttpType.Client)]
        [InlineData(10000, 1000, HttpType.Client)]
        public void HttpAdapterShouldBeOk(int inputCount, int bufferLength, HttpType httpType)
        {
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new HttpDataHandlingAdapter(1024, httpType), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             new Log(), //设置日志
             bufferLength);//用BufferLength模拟粘包，分包

            ByteBlock byteBlock = BytePool.Default.GetByteBlock(1024);
            switch (httpType)
            {
                case HttpType.Server:
                    HttpRequest httpRequest = new HttpRequest();
                    httpRequest.Method = "GET";
                    httpRequest.FromText("RRQM");
                    httpRequest.Build(byteBlock);
                    break;
                case HttpType.Client:
                    HttpResponse httpResponse = new HttpResponse();
                    httpResponse.FromText("RRQM");
                    httpResponse.Build(byteBlock);
                    break;
            }
            byte[] data = byteBlock.ToArray() ;
            string s = Encoding.UTF8.GetString(data);
            byteBlock.Dispose();

            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    tester.SimSend(data);//此处模拟发送
                }
            }

            Thread.Sleep(10000);
            Assert.Equal(inputCount, outputCount);
        }
    }
}