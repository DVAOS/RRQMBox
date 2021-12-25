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
using System;
using System.Collections.Generic;
using System.Threading;

namespace TestCustomDataHandleAdapterService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("1.测试适配器");
            Console.WriteLine("2.启动测试适配器服务");

            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Test_CustomDataHandleAdapter();
                        break;
                    }
                case "2":
                    {
                        CreateService();
                        break;
                    }
                default:
                    break;
            }
        }

        private static void CreateService()
        {
            SimpleTcpService service = new SimpleTcpService();

            service.Connecting += (client, e) =>
            {
                client.SetDataHandlingAdapter(new NormalDataHandlingAdapter());
            };

            service.Received += (client, byteBlock, obj) =>
            {
                //从客户端收到信息
            };

            service.Setup(7789).Start();
            Console.ReadKey();
        }

        public class MySocketClient : SimpleSocketClient
        {
            public void Up_Go_Send(byte[] data)
            {
                ByteBlock byteBlock = BytePool.GetByteBlock(this.BufferLength);//内存池实现，可以直接new byte[].
                byteBlock.Write((byte)1);
                byteBlock.Write((byte)1);
                byteBlock.Write(data);
                try
                {
                    this.Send(byteBlock);
                }
                finally
                {
                    byteBlock.Dispose();
                }
            }

            public void Down_Go_Send(byte[] data)
            {
                ByteBlock byteBlock = BytePool.GetByteBlock(this.BufferLength);//内存池实现，可以直接new byte[].
                byteBlock.Write((byte)0);
                byteBlock.Write((byte)1);
                byteBlock.Write(data);
                try
                {
                    this.Send(byteBlock);
                }
                finally
                {
                    byteBlock.Dispose();
                }
            }

            public void Up_Hold_Send(byte[] data)
            {
                ByteBlock byteBlock = BytePool.GetByteBlock(this.BufferLength);//内存池实现，可以直接new byte[].
                byteBlock.Write((byte)1);
                byteBlock.Write((byte)0);
                byteBlock.Write(data);
                try
                {
                    this.Send(byteBlock);
                }
                finally
                {
                    byteBlock.Dispose();
                }
            }

            public void Down_Hold_Send(byte[] data)
            {
                ByteBlock byteBlock = BytePool.GetByteBlock(this.BufferLength);//内存池实现，可以直接new byte[].
                byteBlock.Write((byte)0);
                byteBlock.Write((byte)0);
                byteBlock.Write(data);
                try
                {
                    this.Send(byteBlock);
                }
                finally
                {
                    byteBlock.Dispose();
                }
            }

            public void Up_Go_SplicingSend(byte[] data)
            {
                List<TransferByte> transferBytes = new List<TransferByte>();
                transferBytes.Add(new TransferByte(new byte[] { 1 }));
                transferBytes.Add(new TransferByte(new byte[] { 1 }));
                transferBytes.Add(new TransferByte(data));
                this.Send(transferBytes);
            }

            public void Down_Go_SplicingSend(byte[] data)
            {
                List<TransferByte> transferBytes = new List<TransferByte>();
                transferBytes.Add(new TransferByte(new byte[] { 0 }));
                transferBytes.Add(new TransferByte(new byte[] { 1 }));
                transferBytes.Add(new TransferByte(data));
                this.Send(transferBytes);
            }

            public void Up_Hold_SplicingSend(byte[] data)
            {
                List<TransferByte> transferBytes = new List<TransferByte>();
                transferBytes.Add(new TransferByte(new byte[] { 1 }));
                transferBytes.Add(new TransferByte(new byte[] { 0 }));
                transferBytes.Add(new TransferByte(data));
                this.Send(transferBytes);
            }

            public void Down_Hold_SplicingSend(byte[] data)
            {
                List<TransferByte> transferBytes = new List<TransferByte>();
                transferBytes.Add(new TransferByte(new byte[] { 0 }));
                transferBytes.Add(new TransferByte(new byte[] { 0 }));
                transferBytes.Add(new TransferByte(data));
                this.Send(transferBytes);
            }
        }

        private static void Test_CustomDataHandleAdapter()
        {
            int received = 0;
            DataAdapterTester dataAdapterTester = DataAdapterTester.CreateTester(new CustomDataHandleAdapter(), (byteBlock, obj) =>
             {
                 received++;//此处接收次数
             },
             10);//包长度，在测试时应当多次设置该值，以模拟更恶劣的环境

            ByteBlock block = new ByteBlock();
            block.Write((byte)1);//写入数据类型
            block.Write((byte)1);//写入数据指令
            block.Write(new byte[100]);//写入数据

            int count = 100000;
            byte[] data = block.ToArray();
            for (int i = 0; i < count; i++)
            {
                dataAdapterTester.SimSend(data, 0, data.Length);
            }

            Thread.Sleep(2000);
            Console.WriteLine($"应当接收{count}次，实际成功接收{received}次");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 解析的对象最好是类，而不是结构体，不然会和object发生拆装箱性能消耗。
    /// </summary>
    internal class DataModel
    {
        public DataModel(byte type, byte instruct, byte length, byte[] data)
        {
            Type = type;
            Instruct = instruct;
            Length = length;
            Data = data;
        }

        public byte Type { get; internal set; }
        public byte Instruct { get; internal set; }
        public byte Length { get; internal set; }
        public byte[] Data { get; internal set; }
    }

    /// <summary>
    /// 数据结构解析
    /// </summary>
    internal class DataModelCustomDataHandleAdapter : DataHandlingAdapter
    {
        /// <summary>
        /// 是否支持拼接发送，为false的话可以不实现<see cref="PreviewSend(IList{TransferByte}, bool)"/>
        /// </summary>
        public override bool CanSplicingSend => true;

        /// <summary>
        /// 临时包，此包仅当前实例储存
        /// </summary>
        private ByteBlock tempByteBlock;

        /// <summary>
        /// 包剩余长度
        /// </summary>
        private byte surPlusLength;

        protected override void PreviewReceived(ByteBlock byteBlock)
        {
            byte[] buffer = byteBlock.Buffer;
            int r = byteBlock.Len;
            if (this.tempByteBlock == null)//如果没有临时包，则直接分包。
            {
                SplitPackage(buffer, 0, r);
            }
            else
            {
                if (surPlusLength == r)//接收长度正好等于剩余长度，组合完数据以后直接处理数据。
                {
                    this.tempByteBlock.Write(buffer, 0, surPlusLength);
                    PreviewHandle(this.tempByteBlock);
                    this.tempByteBlock = null;
                    surPlusLength = 0;
                }
                else if (surPlusLength < r)//接收长度大于剩余长度，先组合包，然后处理包，然后将剩下的分包。
                {
                    this.tempByteBlock.Write(buffer, 0, surPlusLength);
                    PreviewHandle(this.tempByteBlock);
                    this.tempByteBlock = null;
                    SplitPackage(buffer, surPlusLength, r);
                }
                else//接收长度小于剩余长度，无法处理包，所以必须先组合包，然后等下次接收。
                {
                    this.tempByteBlock.Write(buffer, 0, r);
                    surPlusLength -= (byte)r;
                }
            }
        }

        /// <summary>
        /// 分解包
        /// </summary>
        /// <param name="dataBuffer"></param>
        /// <param name="index"></param>
        /// <param name="r"></param>
        private void SplitPackage(byte[] dataBuffer, int index, int r)
        {
            while (index < r)
            {
                byte length = dataBuffer[index];

                byte recedSurPlusLength = (byte)(r - index - 1);
                if (recedSurPlusLength >= length)
                {
                    ByteBlock byteBlock = BytePool.GetByteBlock(length);
                    byteBlock.Write(dataBuffer, index + 1, length);
                    PreviewHandle(byteBlock);
                    surPlusLength = 0;
                }
                else//半包
                {
                    this.tempByteBlock = BytePool.GetByteBlock(length);
                    surPlusLength = (byte)(length - recedSurPlusLength);
                    this.tempByteBlock.Write(dataBuffer, index + 1, recedSurPlusLength);
                }
                index += length + 1;
            }
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="byteBlock"></param>
        private void PreviewHandle(ByteBlock byteBlock)
        {
            try
            {
                this.GoReceived(null, new DataModel(byteBlock.ReadByte(), byteBlock.ReadByte(), (byte)byteBlock.Len, byteBlock.ToArray(2)));
            }
            finally
            {
                byteBlock.Dispose();//在框架里面将内存块释放
            }
        }

        #region 发送

        protected override void PreviewSend(byte[] buffer, int offset, int length, bool isAsync)
        {
            int dataLen = length - offset;//先获取需要发送的实际数据长度

            if (dataLen > byte.MaxValue)//超长判断
            {
                throw new RRQMOverlengthException("发送数据太长。");
            }

            ByteBlock byteBlock = BytePool.GetByteBlock(64 * 1024);//从内存池申请内存块，因为此处数据绝不超过255，所以避免内存池碎片化，每次申请64K
            //ByteBlock byteBlock = BytePool.GetByteBlock(dataLen+1);//实际写法。

            try
            {
                byteBlock.Write((byte)dataLen);//先写长度
                byteBlock.Write(buffer, offset, length);//再写数据

                if (isAsync)//判断异步
                {
                    byte[] data = byteBlock.ToArray();//使用异步时不能将byteBlock.Buffer进行发送，应当ToArray成新的Byte[]。
                    this.GoSend(data, 0, data.Length, isAsync);//调用GoSend，实际发送
                }
                else
                {
                    this.GoSend(byteBlock.Buffer, 0, byteBlock.Len, isAsync);
                }
            }
            finally
            {
                byteBlock.Dispose();//释放内存块
            }
        }

        protected override void PreviewSend(IList<TransferByte> transferBytes, bool isAsync)
        {
            int dataLen = 0;
            foreach (var item in transferBytes)
            {
                dataLen += item.Length;
            }

            if (dataLen > byte.MaxValue)//超长判断
            {
                throw new RRQMOverlengthException("发送数据太长。");
            }

            ByteBlock byteBlock = BytePool.GetByteBlock(64 * 1024);//从内存池申请内存块，因为此处数据绝不超过255，所以避免内存池碎片化，每次申请64K
            //ByteBlock byteBlock = BytePool.GetByteBlock(dataLen+1);//实际写法。

            try
            {
                byteBlock.Write((byte)dataLen);//先写长度

                foreach (var item in transferBytes)
                {
                    byteBlock.Write(item.Buffer, item.Offset, item.Length);//依次写入
                }

                if (isAsync)//判断异步
                {
                    byte[] data = byteBlock.ToArray();//使用异步时不能将byteBlock.Buffer进行发送，应当ToArray成新的Byte[]。
                    this.GoSend(data, 0, data.Length, isAsync);//调用GoSend，实际发送
                }
                else
                {
                    this.GoSend(byteBlock.Buffer, 0, byteBlock.Len, isAsync);
                }
            }
            finally
            {
                byteBlock.Dispose();
            }
        }

        #endregion 发送
    }

    /// <summary>
    /// 支持分片
    /// </summary>
    internal class CanSplicingCustomDataHandleAdapter : DataHandlingAdapter
    {
        /// <summary>
        /// 是否支持拼接发送，为false的话可以不实现<see cref="PreviewSend(IList{TransferByte}, bool)"/>
        /// </summary>
        public override bool CanSplicingSend => true;

        /// <summary>
        /// 临时包，此包仅当前实例储存
        /// </summary>
        private ByteBlock tempByteBlock;

        /// <summary>
        /// 包剩余长度
        /// </summary>
        private byte surPlusLength;

        protected override void PreviewReceived(ByteBlock byteBlock)
        {
            byte[] buffer = byteBlock.Buffer;
            int r = byteBlock.Len;
            if (this.tempByteBlock == null)//如果没有临时包，则直接分包。
            {
                SplitPackage(buffer, 0, r);
            }
            else
            {
                if (surPlusLength == r)//接收长度正好等于剩余长度，组合完数据以后直接处理数据。
                {
                    this.tempByteBlock.Write(buffer, 0, surPlusLength);
                    PreviewHandle(this.tempByteBlock);
                    this.tempByteBlock = null;
                    surPlusLength = 0;
                }
                else if (surPlusLength < r)//接收长度大于剩余长度，先组合包，然后处理包，然后将剩下的分包。
                {
                    this.tempByteBlock.Write(buffer, 0, surPlusLength);
                    PreviewHandle(this.tempByteBlock);
                    this.tempByteBlock = null;
                    SplitPackage(buffer, surPlusLength, r);
                }
                else//接收长度小于剩余长度，无法处理包，所以必须先组合包，然后等下次接收。
                {
                    this.tempByteBlock.Write(buffer, 0, r);
                    surPlusLength -= (byte)r;
                }
            }
        }

        /// <summary>
        /// 分解包
        /// </summary>
        /// <param name="dataBuffer"></param>
        /// <param name="index"></param>
        /// <param name="r"></param>
        private void SplitPackage(byte[] dataBuffer, int index, int r)
        {
            while (index < r)
            {
                byte length = dataBuffer[index];

                byte recedSurPlusLength = (byte)(r - index - 1);
                if (recedSurPlusLength >= length)
                {
                    ByteBlock byteBlock = BytePool.GetByteBlock(length);
                    byteBlock.Write(dataBuffer, index + 1, length);
                    PreviewHandle(byteBlock);
                    surPlusLength = 0;
                }
                else//半包
                {
                    this.tempByteBlock = BytePool.GetByteBlock(length);
                    surPlusLength = (byte)(length - recedSurPlusLength);
                    this.tempByteBlock.Write(dataBuffer, index + 1, recedSurPlusLength);
                }
                index += length + 1;
            }
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="byteBlock"></param>
        private void PreviewHandle(ByteBlock byteBlock)
        {
            try
            {
                this.GoReceived(byteBlock, null);
            }
            finally
            {
                byteBlock.Dispose();//在框架里面将内存块释放
            }
        }

        #region 发送

        protected override void PreviewSend(byte[] buffer, int offset, int length, bool isAsync)
        {
            int dataLen = length - offset;//先获取需要发送的实际数据长度

            if (dataLen > byte.MaxValue)//超长判断
            {
                throw new RRQMOverlengthException("发送数据太长。");
            }

            ByteBlock byteBlock = BytePool.GetByteBlock(64 * 1024);//从内存池申请内存块，因为此处数据绝不超过255，所以避免内存池碎片化，每次申请64K
            //ByteBlock byteBlock = BytePool.GetByteBlock(dataLen+1);//实际写法。

            try
            {
                byteBlock.Write((byte)dataLen);//先写长度
                byteBlock.Write(buffer, offset, length);//再写数据

                if (isAsync)//判断异步
                {
                    byte[] data = byteBlock.ToArray();//使用异步时不能将byteBlock.Buffer进行发送，应当ToArray成新的Byte[]。
                    this.GoSend(data, 0, data.Length, isAsync);//调用GoSend，实际发送
                }
                else
                {
                    this.GoSend(byteBlock.Buffer, 0, byteBlock.Len, isAsync);
                }
            }
            finally
            {
                byteBlock.Dispose();//释放内存块
            }
        }

        protected override void PreviewSend(IList<TransferByte> transferBytes, bool isAsync)
        {
            int dataLen = 0;
            foreach (var item in transferBytes)
            {
                dataLen += item.Length;
            }

            if (dataLen > byte.MaxValue)//超长判断
            {
                throw new RRQMOverlengthException("发送数据太长。");
            }

            ByteBlock byteBlock = BytePool.GetByteBlock(64 * 1024);//从内存池申请内存块，因为此处数据绝不超过255，所以避免内存池碎片化，每次申请64K
            //ByteBlock byteBlock = BytePool.GetByteBlock(dataLen+1);//实际写法。

            try
            {
                byteBlock.Write((byte)dataLen);//先写长度

                foreach (var item in transferBytes)
                {
                    byteBlock.Write(item.Buffer, item.Offset, item.Length);//依次写入
                }

                if (isAsync)//判断异步
                {
                    byte[] data = byteBlock.ToArray();//使用异步时不能将byteBlock.Buffer进行发送，应当ToArray成新的Byte[]。
                    this.GoSend(data, 0, data.Length, isAsync);//调用GoSend，实际发送
                }
                else
                {
                    this.GoSend(byteBlock.Buffer, 0, byteBlock.Len, isAsync);
                }
            }
            finally
            {
                byteBlock.Dispose();
            }
        }

        #endregion 发送
    }

    /// <summary>
    /// 不支持分片
    /// </summary>
    internal class CustomDataHandleAdapter : DataHandlingAdapter
    {
        /// <summary>
        /// 是否支持拼接发送，为false的话可以不实现<see cref="PreviewSend(IList{TransferByte}, bool)"/>
        /// </summary>
        public override bool CanSplicingSend => false;

        /// <summary>
        /// 临时包，此包仅当前实例储存
        /// </summary>
        private ByteBlock tempByteBlock;

        /// <summary>
        /// 包剩余长度
        /// </summary>
        private byte surPlusLength;

        protected override void PreviewReceived(ByteBlock byteBlock)
        {
            byte[] buffer = byteBlock.Buffer;
            int r = byteBlock.Len;
            if (this.tempByteBlock == null)//如果没有临时包，则直接分包。
            {
                SplitPackage(buffer, 0, r);
            }
            else
            {
                if (surPlusLength == r)//接收长度正好等于剩余长度，组合完数据以后直接处理数据。
                {
                    this.tempByteBlock.Write(buffer, 0, surPlusLength);
                    PreviewHandle(this.tempByteBlock);
                    this.tempByteBlock = null;
                    surPlusLength = 0;
                }
                else if (surPlusLength < r)//接收长度大于剩余长度，先组合包，然后处理包，然后将剩下的分包。
                {
                    this.tempByteBlock.Write(buffer, 0, surPlusLength);
                    PreviewHandle(this.tempByteBlock);
                    this.tempByteBlock = null;
                    SplitPackage(buffer, surPlusLength, r);
                }
                else//接收长度小于剩余长度，无法处理包，所以必须先组合包，然后等下次接收。
                {
                    this.tempByteBlock.Write(buffer, 0, r);
                    surPlusLength -= (byte)r;
                }
            }
        }

        /// <summary>
        /// 分解包
        /// </summary>
        /// <param name="dataBuffer"></param>
        /// <param name="index"></param>
        /// <param name="r"></param>
        private void SplitPackage(byte[] dataBuffer, int index, int r)
        {
            while (index < r)
            {
                byte length = dataBuffer[index];

                byte recedSurPlusLength = (byte)(r - index - 1);
                if (recedSurPlusLength >= length)
                {
                    ByteBlock byteBlock = BytePool.GetByteBlock(length);
                    byteBlock.Write(dataBuffer, index + 1, length);
                    PreviewHandle(byteBlock);
                    surPlusLength = 0;
                }
                else//半包
                {
                    this.tempByteBlock = BytePool.GetByteBlock(length);
                    surPlusLength = (byte)(length - recedSurPlusLength);
                    this.tempByteBlock.Write(dataBuffer, index + 1, recedSurPlusLength);
                }
                index += length + 1;
            }
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="byteBlock"></param>
        private void PreviewHandle(ByteBlock byteBlock)
        {
            try
            {
                this.GoReceived(byteBlock, null);
            }
            finally
            {
                byteBlock.Dispose();//在框架里面将内存块释放
            }
        }

        #region 发送

        protected override void PreviewSend(byte[] buffer, int offset, int length, bool isAsync)
        {
            int dataLen = length - offset;//先获取需要发送的实际数据长度

            if (dataLen > byte.MaxValue)//超长判断
            {
                throw new RRQMOverlengthException("发送数据太长。");
            }

            ByteBlock byteBlock = BytePool.GetByteBlock(64 * 1024);//从内存池申请内存块，因为此处数据绝不超过255，所以避免内存池碎片化，每次申请64K
            //ByteBlock byteBlock = BytePool.GetByteBlock(dataLen+1);//实际写法。

            try
            {
                byteBlock.Write((byte)dataLen);//先写长度
                byteBlock.Write(buffer, offset, length);//再写数据

                if (isAsync)//判断异步
                {
                    byte[] data = byteBlock.ToArray();//使用异步时不能将byteBlock.Buffer进行发送，应当ToArray成新的Byte[]。
                    this.GoSend(data, 0, data.Length, isAsync);//调用GoSend，实际发送
                }
                else
                {
                    this.GoSend(byteBlock.Buffer, 0, byteBlock.Len, isAsync);
                }
            }
            finally
            {
                byteBlock.Dispose();//释放内存块
            }
        }

        protected override void PreviewSend(IList<TransferByte> transferBytes, bool isAsync)
        {
            //暂时不实现。
        }

        #endregion 发送
    }
}