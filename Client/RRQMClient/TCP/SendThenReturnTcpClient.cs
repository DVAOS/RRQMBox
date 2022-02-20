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
using RRQMCore.ByteManager;
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMClient.TCP
{
    /// <summary>
    /// 发送，然后同步等待返回
    /// </summary>
    internal class SendThenReturnTcpClient : TcpClient, IWaitSender
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SendThenReturnTcpClient()
        {
            this.waitData = new WaitData<byte[]>();
        }

        private WaitData<byte[]> waitData;

        private int timeout = 60 * 1000;

        /// <summary>
        /// 超时设置
        /// </summary>
        public int Timeout
        {
            get { return timeout; }
            set
            {
                if (value < 1)
                {
                    value = 1;
                }
                timeout = value;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public byte[] SendThenReturn(byte[] buffer, int offset, int length, CancellationToken token = default)
        {
            lock (this)
            {
                waitData.Reset();
                this.Send(buffer, offset, length);
                this.waitData.SetCancellationToken(token);
                switch (this.waitData.Wait(this.timeout))
                {
                    case WaitDataStatus.SetRunning:
                        return waitData.WaitResult;

                    case WaitDataStatus.Overtime:
                        throw new TimeoutException();
                    case WaitDataStatus.Canceled:
                        {
                            return default;
                        }
                    case WaitDataStatus.Default:
                    case WaitDataStatus.Disposed:
                    default:
                        throw new RRQMException(RRQMCore.ResType.UnknownError.GetResString());
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public byte[] SendThenReturn(byte[] buffer, CancellationToken token = default)
        {
            return this.SendThenReturn(buffer, 0, buffer.Length, token);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public byte[] SendThenReturn(ByteBlock byteBlock, CancellationToken token = default)
        {
            return this.SendThenReturn(byteBlock.Buffer, 0, byteBlock.Len, token);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<byte[]> SendThenReturnAsync(byte[] buffer, int offset, int length, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                return this.SendThenReturn(buffer, offset, length, token);
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<byte[]> SendThenReturnAsync(byte[] buffer, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                return this.SendThenReturn(buffer, token);
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<byte[]> SendThenReturnAsync(ByteBlock byteBlock, CancellationToken token = default)
        {
            return Task.Run(() =>
            {
                return this.SendThenReturn(byteBlock, token);
            });
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="obj"></param>
        protected override void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (this.waitData.Status == WaitDataStatus.Default)
            {
                this.waitData.Set(byteBlock.ToArray());
            }
            else
            {
                //处理数据
            }
        }
    }
}
