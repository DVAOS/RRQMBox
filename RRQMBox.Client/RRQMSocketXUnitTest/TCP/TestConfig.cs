//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
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
using Xunit;

namespace RRQMSocketXUnitTest.TCP
{
    public class TestConfig
    {
        [Fact]
        public void OfRRQMConfigBeOk()
        {
            int bufferLength = 1024;
            Log logger = new Log();
            int bytePoolMaxBlockSize = 1024;
            long bytePoolMaxSize = 1024 * 10;

            RRQMConfig config = new RRQMConfig();
            config.SetValue(RRQMConfig.BufferLengthProperty, bufferLength)
                .SetValue(RRQMConfig.LoggerProperty, logger)
                .SetValue(RRQMConfig.BytePoolMaxBlockSizeProperty, bytePoolMaxBlockSize)
                .SetValue(RRQMConfig.BytePoolMaxSizeProperty, bytePoolMaxSize);

            Assert.Equal(bufferLength, config.BufferLength);
            Assert.Equal(logger, config.Logger);
            Assert.Equal(bytePoolMaxBlockSize, config.BytePoolMaxBlockSize);
            Assert.Equal(bytePoolMaxSize, config.BytePoolMaxSize);
        }
    }
}