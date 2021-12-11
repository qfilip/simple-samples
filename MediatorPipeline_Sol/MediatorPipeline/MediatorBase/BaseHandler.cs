using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MediatorPipeline.MediatorBase
{
    public abstract class BaseHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
