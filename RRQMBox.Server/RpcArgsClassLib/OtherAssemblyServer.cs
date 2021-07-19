using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpcArgsClassLib
{
    public class OtherAssemblyServer : ServerProvider
    {
        [RRQMRPC]
        public int Sum(int a, int b)
        {
            return a + b;
        }

        [RRQMRPC]
        public ClassOne GetClassOne()
        {
            return new ClassOne() {P1=10,P2="RRQM" };
        }
    }

   [RRQMRPCMember]
    public class ClassOne
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
    }
}
