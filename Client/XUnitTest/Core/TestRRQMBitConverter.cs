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
using RRQMCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest.Core
{
    public class TestRRQMBitConverter
    {
        [Fact]
        public void BigEndianShouldOk()
        {
            byte[] data = RRQMBitConverter.BigEndian.GetBytes(10);
            Assert.Equal(10, RRQMBitConverter.BigEndian.ToInt32(data, 0));
        }

        [Fact]
        public void LittleEndianShouldOk()
        {
            byte[] data = RRQMBitConverter.LittleEndian.GetBytes(10);
            Assert.Equal(10, RRQMBitConverter.LittleEndian.ToInt32(data, 0));
        }
    }
}
