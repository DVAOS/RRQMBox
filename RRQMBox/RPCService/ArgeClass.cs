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
using System.Collections.Generic;

namespace Demo.Service
{
    public class Test01
    {
        public int Age { get; set; } = 1;
        public string Name { get; set; }
    }

    public class Test02
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public List<int> list { get; set; }
        public int[] nums { get; set; }
    }

    public class Test03 : Test02
    {
        public int Length { get; set; }
    }

    public enum MyEnum
    {
        T1 = 0,
        T2 = 100,
        T3 = 200
    }
}