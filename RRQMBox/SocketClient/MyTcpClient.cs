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
using System;
using System.Text;
using RRQMCore.ByteManager;
using RRQMSocket;

namespace Demo.TestTcpClient
{
    public class MyTcpClient : TcpClient
    {
        public MyTcpClient()
        {
            this.DataHandlingAdapter = new NormalDataHandlingAdapter();//普通TCP报文处理器
            //this.DataHandlingAdapter = new FixedSizeDataHandlingAdapter(1024);//固定长度TCP报文处理器
            //this.DataHandlingAdapter = new TerminatorDataHandlingAdapter(1024, "\r\n");//终止字符TCP报文处理器
            //this.DataHandlingAdapter = new FixedHeaderDataHandlingAdapter();//固定包头TCP报文处理器
            //this.DataHandlingAdapter = new MyTestDataHandingAdopter();//自定义处理器
        }

        private int count;

        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            count++;
            if (count % 100 == 0)
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Position);
                Console.WriteLine($"已接收到信息：{mes},第{count}条");
            }
        }
    }
}