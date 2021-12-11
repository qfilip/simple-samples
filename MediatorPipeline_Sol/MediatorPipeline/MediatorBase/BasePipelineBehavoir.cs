using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatorPipeline.MediatorBase
{
    public class BasePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : new()
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await next();
            return await Task.FromResult(new TResponse());
        }
    }
}
