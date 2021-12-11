using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;

namespace MediatorPipeline.MediatorBase
{
    public class BaseExceptionHandler<TRequest, TResponse> : IRequestExceptionHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task Handle(TRequest request, Exception exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
