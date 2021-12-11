using MediatR;

namespace MediatorPipeline.MediatorBase
{
    public abstract class BaseCommand<TDto> : IRequest<TDto>
    {
    }
}
