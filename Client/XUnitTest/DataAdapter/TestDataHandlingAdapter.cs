//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  API首页：https://www.yuque.com/eo2w71/rrqm
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.ByteManager;
using RRQMSocket;
using RRQMSocket.Http;
using RRQMSocket.WebSocket;
using System;
using System.Text;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTest.DataAdapter
{
    public class TestDataHandlingAdapter
    {
        ITestOutputHelper output;
        public TestDataHandlingAdapter(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void FixedHeaderShouldBeOk(int inputCount, int bufferLength)
        {
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedHeaderPackageAdapter(), bufferLength);//用BufferLength模拟粘包，分包

            byte[] data = Encoding.UTF8.GetBytes("RRQM");

            output.WriteLine(tester.Run(data, inputCount, inputCount, 1000 * 60).ToString());
        }

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void FixedSizeShouldBeOk(int inputCount, int bufferLength)
        {
            DataAdapterTester tester = DataAdapterTester.CreateTester(new FixedSizePackageAdapter(1024), bufferLength);//用BufferLength模拟粘包，分包

            byte[] data = Encoding.UTF8.GetBytes("RRQM");

            output.WriteLine(tester.Run(data, inputCount, inputCount, 1000 * 60).ToString());
        }

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void TerminatorShouldBeOk(int inputCount, int bufferLength)
        {
            DataAdapterTester tester = DataAdapterTester.CreateTester(new TerminatorPackageAdapter(1024, "\r\n"), bufferLength);//用BufferLength模拟粘包，分包

            byte[] data = Encoding.UTF8.GetBytes("RRQM");

            output.WriteLine(tester.Run(data, inputCount, inputCount, 1000 * 60).ToString());
        }

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void HttpServerAdapterShouldBeOk(int inputCount, int bufferLength)
        {
            DataAdapterTester tester = DataAdapterTester.CreateTester(new HttpServerDataHandlingAdapter(1024), bufferLength);//用BufferLength模拟粘包，分包

            ByteBlock byteBlock = BytePool.GetByteBlock(1024);
            HttpRequest httpRequest = new HttpRequest();
            httpRequest.Method = "GET";
            httpRequest.FromText("RRQM");
            httpRequest.Build(byteBlock);
            byte[] data = byteBlock.ToArray();
            string s = Encoding.UTF8.GetString(data);
            byteBlock.Dispose();

            output.WriteLine(tester.Run(data, inputCount, inputCount, 1000 * 60).ToString());
        }

        [Theory]
        [InlineData(10000, 10)]
        [InlineData(10000, 100)]
        [InlineData(10000, 1000)]
        public void HttpClientAdapterShouldBeOk(int inputCount, int bufferLength)
        {
            DataAdapterTester tester = DataAdapterTester.CreateTester(new HttpClientDataHandlingAdapter(1024), bufferLength);//用BufferLength模拟粘包，分包

            ByteBlock byteBlock = BytePool.GetByteBlock(1024);
            HttpResponse httpResponse = new HttpResponse();
            httpResponse.FromText("RRQM");
            httpResponse.Build(byteBlock);
            byte[] data = byteBlock.ToArray();
            string s = Encoding.UTF8.GetString(data);
            byteBlock.Dispose();

            output.WriteLine(tester.Run(data, inputCount, inputCount, 1000 * 60).ToString());
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

            WSDataFrame dataFrame = new WSDataFrame();
            dataFrame.AppendBinary(data, 0, data.Length);
            byte[] newdata = dataFrame.BuildResponseToBytes();

            bool success = true;
            DataAdapterTester tester = DataAdapterTester.CreateTester(new WebSocketDataHandlingAdapter(), bufferLength,//用BufferLength模拟粘包，分包
            (ByteBlock byteBlock, IRequestInfo obj) =>
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
            });

            output.WriteLine(tester.Run(newdata, testCount, testCount, dataLen * 100).ToString());
            Assert.True(success);
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
            DataAdapterTester tester = DataAdapterTester.CreateTester(new WebSocketDataHandlingAdapter(), 1024,//用BufferLength模拟粘包，分包
            (ByteBlock byteBlock, IRequestInfo obj) =>
              {
                  rev_DataFrame = (WSDataFrame)obj;
                  rev_DataFrame.PayloadData.SetHolding(true);
              });

            tester.Run(newdata, 1, 1, 1000);//此处模拟发送

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
            DataAdapterTester tester = DataAdapterTester.CreateTester(new WebSocketDataHandlingAdapter(), 1024,//用BufferLength模拟粘包，分包
            (ByteBlock byteBlock, IRequestInfo obj) =>
              {
                  rev_DataFrame = (WSDataFrame)obj;
                  rev_DataFrame.PayloadData.SetHolding(true);
              }
            );

            tester.Run(newdata, 1, 1, 1000);//此处模拟发送

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

        [Theory]
        [InlineData(10000, 3)]
        [InlineData(10000, 5)]
        [InlineData(10000, 200)]
        [InlineData(10000, 500)]
        [InlineData(10000, 1000)]
        public void MyCustomDataHandlingAdapterShouldBeOk(int inputCount, int bufferLength)
        {
            DataAdapterTester tester = DataAdapterTester.CreateTester(new MyCustomDataHandlingAdapter(), bufferLength);//用BufferLength模拟粘包，分包

            ByteBlock block = new ByteBlock();
            block.Write((byte)102);//写入数据长度
            block.Write((byte)1);//写入数据类型
            block.Write((byte)1);//写入数据指令

            byte[] buffer = new byte[100];
            new Random().NextBytes(buffer);
            block.Write(buffer);//写入数据

            byte[] data = block.ToArray();

            //输出测试时间，用于衡量适配性能
            output.WriteLine(tester.Run(data, inputCount, inputCount, 1000 * 2).ToString());
        }
    }
}