using RRQMProxy;
using RRQMSocket.RPC.WebApi;
using RRQMSocketXUnitTest.RPC;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitTest.RPC.WebApi
{
    public class TestWebApi
    {
        [Fact]
        public void ShouldSuccessfulCallService()
        {
            WebApiClient client = new WebApiClient();
            client.Setup("127.0.0.1:7801");
            client.Connect();

            IXUnitTestController controller = new XUnitTestController(client);
            var result1 = controller.HttpGetGetListClass01(10);
            Assert.Equal(10, result1.Count);
        }
    }
}
