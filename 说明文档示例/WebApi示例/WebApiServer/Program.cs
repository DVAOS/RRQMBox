using RRQMSocket.RPC;
using RRQMSocket.RPC.WebApi;
using System;

namespace WebApiServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    [Route("[controller]/[action]")]
    public class ServerController : ServerProvider
    {
        [HttpGet]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [HttpPost]
        public int TestPost(MyClass myClass)
        {
            return myClass.A + myClass.B;
        }
    }

    public class MyClass
    {
        public int A { get; set; }
        public int B { get; set; }
    }
}
