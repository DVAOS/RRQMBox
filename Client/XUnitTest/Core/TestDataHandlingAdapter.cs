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
using RRQMSocket.WebSocket;
using RRQMSocket.WebSocket.Helper;
using System;
using System.Text;
using System.Threading;
using Xunit;

namespace XUnitTest.Core
{
    public class TestDataHandlingAdapter
    {
        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void FixedHeaderShouldBeOk(int inputCount, int bufferLength)
        {
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedHeaderDataHandlingAdapter(), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
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
        [InlineData(10000, 1000)]
        public void FixedSizeShouldBeOk(int inputCount, int bufferLength)
        {
            int outputCount = 0;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedSizeDataHandlingAdapter(1024), (ByteBlock byteBlock, object obj) =>
            {
                //此处模拟接收
                outputCount++;
            },
             bufferLength);//用BufferLength模拟粘包，分包

            byte[] data = Encoding.UTF8.GetBytes("RRQM");

            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    tester.SimSend(data);//此处模拟发送
                }
            }
            Thread.Sleep(20000);
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
             bufferLength);//用BufferLength模拟粘包，分包

            ByteBlock byteBlock = BytePool.GetByteBlock(1024);
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
            byte[] data = byteBlock.ToArray();
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

        [Theory]
        [InlineData(1024, 1)]
        [InlineData(1024, 10)]
        [InlineData(1024, 1024)]
        [InlineData(1024, 1024 * 64)]
        [InlineData(10240, 10)]
        [InlineData(10240, 1024)]
        [InlineData(10240, 1024 * 64)]
        [InlineData(65536, 10)]
        [InlineData(65536, 1024)]
        [InlineData(65536, 1024 * 64)]
        [InlineData(655360, 100)]
        [InlineData(655360, 1024)]
        [InlineData(655360, 1024 * 64)]

        public void WebSocketAdapterNoMaskShouldBeOk(int dataLen, int bufferLength)
        {
            byte[] data = new byte[dataLen];
            new Random().NextBytes(data);

            int testCount = 5000;
            EventWaitHandle waitHandle = new AutoResetEvent(false);

            WSDataFrame dataFrame = new WSDataFrame();
            dataFrame.AppendBinary(data, 0, data.Length);
            byte[] newdata = dataFrame.BuildResponseToBytes();

            int outputCount = 0;
            bool success = true;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new WebSocketDataHandlingAdapter(),
                (ByteBlock byteBlock, object obj) =>
            {
                WSDataFrame revDataFram = (WSDataFrame)obj;


                if (dataFrame.FIN != revDataFram.FIN)
                {
                    success = false;
                }
                if (dataFrame.RSV1 != revDataFram.RSV1)
                {
                    success = false;
                }
                if (dataFrame.RSV2 != revDataFram.RSV2)
                {
                    success = false;
                }
                if (dataFrame.RSV3 != revDataFram.RSV3)
                {
                    success = false;
                }
                if (dataFrame.PayloadLength == revDataFram.PayloadLength)
                {
                    for (int i = 0; i < dataFrame.PayloadLength; i++)
                    {
                        if (dataFrame.PayloadData.Buffer[i] != revDataFram.PayloadData.Buffer[i])
                        {
                            success = false;
                        }
                    }
                }
                else
                {
                    success = false;
                }
                //此处模拟接收
                outputCount++;
                if (outputCount == testCount)
                {
                    waitHandle.Set();
                }
            },
             bufferLength);//用BufferLength模拟粘包，分包

            for (int j = 0; j < 1; j++)
            {
                for (int i = 0; i < testCount; i++)
                {
                    tester.SimSend(newdata);//此处模拟发送
                }
            }

            if (waitHandle.WaitOne(1000 * 60 * 5))
            {
                Assert.True(success);
                Assert.Equal(testCount, outputCount);
            }
            else
            {
                throw new Exception("测试超时。");
            }

        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        [InlineData(1000000)]
        [InlineData(10000000)]
        public void WSDataNoMaskShouldBeOk(int length)
        {
            byte[] data = new byte[length];
            new Random().NextBytes(data);

            WSDataFrame dataFrame = new WSDataFrame();
            dataFrame.AppendBinary(data, 0, data.Length);
            byte[] newdata = dataFrame.BuildResponseToBytes();

            WSDataFrame rev_DataFrame = null;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new WebSocketDataHandlingAdapter(),
            (ByteBlock byteBlock, object obj) =>
              {
                  rev_DataFrame = (WSDataFrame)obj;
                  rev_DataFrame.PayloadData.SetHolding(true);
              },
            1024);//用BufferLength模拟粘包，分包

            tester.SimSend(newdata);//此处模拟发送

            Thread.Sleep(1000 * 2);
            Assert.NotNull(rev_DataFrame);
            Assert.Equal(dataFrame.FIN, rev_DataFrame.FIN);
            Assert.Equal(dataFrame.RSV1, rev_DataFrame.RSV1);
            Assert.Equal(dataFrame.RSV2, rev_DataFrame.RSV2);
            Assert.Equal(dataFrame.RSV3, rev_DataFrame.RSV3);
            Assert.Equal(dataFrame.Opcode, rev_DataFrame.Opcode);
            Assert.Equal(dataFrame.Mask, rev_DataFrame.Mask);
            Assert.Equal(dataFrame.PayloadLength, rev_DataFrame.PayloadLength);
            Assert.Null(rev_DataFrame.MaskingKey);

            for (int i = 0; i < dataFrame.PayloadLength; i++)
            {
                Assert.Equal(dataFrame.PayloadData.Buffer[i], rev_DataFrame.PayloadData.Buffer[i]);
            }
            rev_DataFrame.PayloadData.SetHolding(false);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        [InlineData(1000000)]
        [InlineData(10000000)]
        public void WSDataMaskShouldBeOk(int length)
        {
            byte[] data = new byte[length];
            new Random().NextBytes(data);

            WSDataFrame dataFrame = new WSDataFrame();
            dataFrame.AppendBinary(data, 0, data.Length);
            byte[] newdata = dataFrame.BuildRequestToBytes();

            WSDataFrame rev_DataFrame = null;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new WebSocketDataHandlingAdapter(),
            (ByteBlock byteBlock, object obj) =>
              {
                  rev_DataFrame = (WSDataFrame)obj;
                  rev_DataFrame.PayloadData.SetHolding(true);
              },
            1024);//用BufferLength模拟粘包，分包

            tester.SimSend(newdata);//此处模拟发送

            Thread.Sleep(1000 * 2);
            Assert.NotNull(rev_DataFrame);
            Assert.Equal(dataFrame.FIN, rev_DataFrame.FIN);
            Assert.Equal(dataFrame.RSV1, rev_DataFrame.RSV1);
            Assert.Equal(dataFrame.RSV2, rev_DataFrame.RSV2);
            Assert.Equal(dataFrame.RSV3, rev_DataFrame.RSV3);
            Assert.Equal(dataFrame.Opcode, rev_DataFrame.Opcode);
            Assert.Equal(dataFrame.Mask, rev_DataFrame.Mask);
            Assert.Equal(dataFrame.PayloadLength, rev_DataFrame.PayloadLength);
            Assert.NotNull(rev_DataFrame.MaskingKey);

            for (int i = 0; i < dataFrame.PayloadLength; i++)
            {
                Assert.Equal(dataFrame.PayloadData.Buffer[i], rev_DataFrame.PayloadData.Buffer[i]);
            }
            rev_DataFrame.PayloadData.SetHolding(false);
        }
    }
}