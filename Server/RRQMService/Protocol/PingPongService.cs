using RRQMCore.ByteManager;
using RRQMSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRQMService.Protocol
{
    class PingPongService : RRQMSocket.ProtocolService<PingPongSocketClient>
    {
    }

    class PingPongSocketClient : RRQMSocket.ProtocolSocketClient
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

        protected override void OnPing()
        {
            Console.WriteLine("Ping");
            base.OnPing();
        }
    }
}
