using Grpc.Core;
using GrpcServer.Web.Protos;
using System.Linq;
using System.Threading.Tasks;

namespace RPCPerformance.Services
{
    public class TestService : TestGrpcService.TestGrpcServiceBase
    {
        public override Task<GrpcGetAddResponse> GetAdd(GrpcGetAddRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GrpcGetAddResponse() { Result = request.A + request.B });
        }
    }
}
