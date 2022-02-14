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
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTest.DataAdapter
{
    public class MyCustomDataHandlingAdapter : CustomFixedHeaderDataHandlingAdapter<MyFixedHeaderRequestInfo>
    {
        public MyCustomDataHandlingAdapter()
        {
            this.MaxSize = 1024;
        }
        public override int HeaderLength => 3;

        public override int MaxSize { get; set; }

        protected override MyFixedHeaderRequestInfo GetInstance()
        {
            return new MyFixedHeaderRequestInfo();
        }

        protected override bool OnReceivingError(DataResult dataResult)
        {
            this.Owner.Logger.Debug(RRQMCore.Log.LogType.Error, this, dataResult.Message, null);
            return true;
        }

        protected override void Reset()
        {
            if (this.tempByteBlock != null)
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
        /// 自定义属性，标识数据长度
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

        /// <summary>
        /// 当收到数据，由框架封送有效载荷数据。
        /// </summary>
        /// <param name="body"></param>
        /// <returns>是否成功有效</returns>
        public DataResult OnParsingBody(byte[] body)
        {
            if (body.Length == this.bodyLength)
            {
                this.body = body;
                return DataResult.SuccessResult;
            }
            return new DataResult("数据长度不对", DataResultCode.Error);
        }

        /// <summary>
        /// 当收到数据，由框架封送固定协议头。您需要在此函数中，解析自己的固定包头，并且返回后续数据的长度
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public FilterResult OnParsingHeader(byte[] header)
        {
            //在该示例中，第一个字节表示后续的所有数据长度，但是header设置的是3，所以后续还应当接收length-2个长度。
            this.bodyLength = header[0] - 2;
            this.dataType = header[1];
            this.orderType = header[2];
            return FilterResult.Success;
        }
    }
}
