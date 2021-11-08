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
using RRQMSocket.FileTransfer;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RRQMSocketXUnitTest
{
    public class TestByteBlock
    {
        [Fact]
        public void ShouldCanWriteAndRead()
        {
            ByteBlock byteBlock = BytePool.Default.GetByteBlock(1024 * 1024);

            //开始写

            byte writeByte = 10;//Byte
            byteBlock.Write(writeByte);

            char writeChar = 'A';//Char
            byteBlock.Write(writeChar);

            int writeInt = int.MaxValue;//int
            byteBlock.Write(writeInt);

            double writeDouble = 3.14;//Double
            byteBlock.Write(writeDouble);

            Test writeObject = new Test() { P1 = 10, P2 = "RRQM" };//object
            byteBlock.WriteObject(writeObject);

            byteBlock.WriteObject(null);//null object

            byte[] writeBytes = new byte[1024];//byte[]包
            new Random().NextBytes(writeBytes);
            byteBlock.WriteBytesPackage(writeBytes);

            byteBlock.WriteBytesPackage(null);//null byte[]包



            //重置流位置，然后依次读
            byteBlock.Pos = 0;
            byte newWriteByte = byteBlock.ReadByte();//byte
            Assert.Equal(writeByte, newWriteByte);

            char newWriteChar = byteBlock.ReadChar();//char
            Assert.Equal(writeChar, newWriteChar);

            int newWriteInt = byteBlock.ReadInt32();//int
            Assert.Equal(writeInt, newWriteInt);

            double newWriteDouble = byteBlock.ReadDouble();//Double
            Assert.Equal(writeDouble, newWriteDouble);

            Test newWriteObject = byteBlock.ReadObject<Test>();//object
            Assert.Equal(writeObject.P1,newWriteObject.P1);
            Assert.Equal(writeObject.P2,newWriteObject.P2);

            object nullObject = byteBlock.ReadObject<object>();//null object
            Assert.Null(nullObject);

            byte[] newWriteBytes = byteBlock.ReadBytesPackage();
            for (int i = 0; i < newWriteBytes.Length; i++)
            {
                Assert.Equal(writeBytes[i],newWriteBytes[i]);
            }

            byte[] newNullWriteBytes = byteBlock.ReadBytesPackage();
            Assert.Null(newNullWriteBytes);
        }

        [Fact]
        public void ShouldCanRatioCapacity()
        {
            //测试扩容比赋值
            Assert.Equal(1.5f, ByteBlock.Ratio);
            ByteBlock.Ratio = 0.5f;
            Assert.Equal(1f, ByteBlock.Ratio);
            ByteBlock.Ratio = 1.5f;

            //测试申请内存
            ByteBlock byteBlock = BytePool.Default.GetByteBlock(10,true);

            Assert.NotNull(byteBlock);
            Assert.Equal(0,byteBlock.Pos);
            Assert.Equal(0,byteBlock.Len);
            Assert.Equal(10,byteBlock.Capacity);

            //测试写入时动态扩容
            byte[] data = new byte[20];
            new Random().NextBytes(data);
            byteBlock.Write(data);
            Assert.Equal(20, byteBlock.Pos);
            Assert.Equal(20, byteBlock.Len);
            Assert.Equal(25, byteBlock.Capacity);

            byte[] data2 = new byte[100];
            new Random().NextBytes(data2);
            byteBlock.Write(data2);
            Assert.Equal(120, byteBlock.Pos);
            Assert.Equal(120, byteBlock.Len);
            Assert.Equal(167, byteBlock.Capacity);
        }
    }

    [Serializable]
    public class Test
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
    }
}
