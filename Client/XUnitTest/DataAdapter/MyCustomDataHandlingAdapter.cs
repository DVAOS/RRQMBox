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
using RRQMSocket;
using RRQMSocket.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest.DataAdapter
{
    public class MyCustomDataHandlingAdapter : CustomFixedHeaderDataHandlingAdapter<MyFixedHeaderRequestInfo>
    {
        public MyCustomDataHandlingAdapter()
        {
            this.MaxPackageSize = 1024;
        }

        /// <summary>
        /// 接口实现，指示固定包头长度
        /// </summary>
        public override int HeaderLength => 3;

        /// <summary>
        /// 获取新实例
        /// </summary>
        /// <returns></returns>
        protected override MyFixedHeaderRequestInfo GetInstance()
        {
            return new MyFixedHeaderRequestInfo();
        }

        protected override void Reset()
        {
            if (this.tempByteBlock!=null)
            {
                this.tempByteBlock.Dispose();
                this.tempByteBlock = null;
            }
           
        }
    }

    public class MyFixedHeaderRequestInfo : IFixedHeaderRequestInfo
    {
        private int bodyLength;
        /// <summary>
        /// 接口实现，标识数据长度
        /// </summary>
        public int BodyLength
        {
            get { return bodyLength; }
        }

        private byte dataType;
        /// <summary>
        /// 自定义属性，标识数据类型
        /// </summary>
        public byte DataType
        {
            get { return dataType; }
        }

        private byte orderType;
        /// <summary>
        /// 自定义属性，标识指令类型
        /// </summary>
        public byte OrderType
        {
            get { return orderType; }
        }

        private byte[] body;
        /// <summary>
        /// 自定义属性，标识实际数据
        /// </summary>
        public byte[] Body
        {
            get { return body; }
        }

       
        public bool OnParsingBody(byte[] body)
        {
            if (body.Length == this.bodyLength)
            {
                this.body = body;
                return true;
            }
            return false;
        }

      
        public bool OnParsingHeader(byte[] header)
        {
            //在该示例中，第一个字节表示后续的所有数据长度，但是header设置的是3，所以后续还应当接收length-2个长度。
            this.bodyLength = header[0]-2;
            this.dataType = header[1];
            this.orderType = header[2];
            return  true;
        }
    }
}
