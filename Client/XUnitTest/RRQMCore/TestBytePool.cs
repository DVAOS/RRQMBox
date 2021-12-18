using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest
{
    public class TestBytePool
    {
        [Fact]
        public void ShouldCanSetPool()
        {
            ByteBlock byteBlock = new ByteBlock(1024);
            Assert.True(byteBlock.Capacity>=1024);
            byteBlock.Dispose();

            byteBlock = new ByteBlock(1024*1024,true);
            Assert.True(byteBlock.Capacity == 1024*1024);
            byteBlock.Dispose();

        }
    }
}
