using Grpc.Core;
using GrpcServer.Web.Protos;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcConsoleApp.Services
{
    public class TestService : TestGrpcService.TestGrpcServiceBase
    {
        public override Task<AddReturn> Add(AddPS request, ServerCallContext context)
        {
            return Task.FromResult(new AddReturn() { Rusult = request.A + request.B });
        }
    }
}
