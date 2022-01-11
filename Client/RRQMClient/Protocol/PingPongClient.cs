using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMClient.Protocol
{
    class PingPongClient : RRQMSocket.ProtocolClient
    {
        protected override void HandleProtocolData(short procotol, ByteBlock byteBlock)
        {
            throw new NotImplementedException();
        }

        protected override void HandleStream(StreamStatusEventArgs args)
        {
            throw new NotImplementedException();
        }

        protected override void PreviewHandleStream(StreamOperationEventArgs args)
        {
            throw new NotImplementedException();
        }
        protected override void OnPong()
        {
            Console.WriteLine("Pong");
            base.OnPong();
        }
    }
}
