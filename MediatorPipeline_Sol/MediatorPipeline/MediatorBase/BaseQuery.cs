using MediatR;

namespace MediatorPipeline.MediatorBase
{
    public abstract class BaseQuery<TResponse> : IRequest<TResponse>
    {
    }
}