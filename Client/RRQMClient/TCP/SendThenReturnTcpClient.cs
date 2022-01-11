using RRQMCore;
using RRQMCore.ByteManager;
using RRQMCore.Exceptions;
using RRQMCore.Run;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        throw new RRQMTimeoutException();
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
        protected override void HandleReceivedData(ByteBlock byteBlock, object obj)
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
