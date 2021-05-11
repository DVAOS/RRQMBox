using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RRQMSocket.RPC;
using RRQMSocket.RPC.RRQMRPC;

namespace Demo.Client
{
    public class CallBackServer : ServerProvider
    {
        [RRQMRPCCallBackMethod(1000)]
        public string SayHello(int age)
        {
            string mes = $"Hello,我今年{age}岁了";
            Console.WriteLine(mes);
            return mes;
        }
    }
}
