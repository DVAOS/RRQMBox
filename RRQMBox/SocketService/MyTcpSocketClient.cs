//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  源代码仓库：https://gitee.com/RRQM_Home
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAdopter;

namespace Demo.TestTcpService
{
    public class MyTcpSocketClient : TcpSocketClient
    {
        /// <summary>
        /// 初次创建对象，效应相当于构造函数，但是调用时机在构造函数之后，可覆盖父类方法
        /// </summary>
        public override void Create()
        {

            this.DataHandlingAdapter = new NormalDataHandlingAdapter();//普通TCP报文处理器
            //this.DataHandlingAdapter = new FixedHeaderDataHandlingAdapter();//固定包头TCP报文处理器
            //this.DataHandlingAdapter = new FixedSizeDataHandlingAdapter(1024);//固定长度TCP报文处理器
            //this.DataHandlingAdapter = new TerminatorDataHandlingAdapter(1024, "\r\n");//终止字符TCP报文处理器
            //this.DataHandlingAdapter = new MyTestDataHandingAdopter();//自定义处理器
        }


        int count;
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
        {
            count++;
            if (count % 1 == 0)
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                Console.WriteLine($"已接收到信息：{mes},第{count}条");
            }
            if (this.Online)
            {
                this.Send(byteBlock);//回传消息
            }
        }

    }
}
