using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatorPipeline.MediatorBase;
using MediatorPipeline.Models;

namespace MediatorPipeline.Commands
{
    public class WhateverCommand : BaseCommand<SampleDto>
    {
        private readonly SampleDto _sampleDto;
        public WhateverCommand(SampleDto sampleDto)
        {
            _sampleDto = sampleDto;
        }


        public class Validator : AbstractValidator<WhateverCommand>
        {
            public Validator()
            {
                RuleFor(x => x._sampleDto)
                    .NotNull()
                    .Must(dto => dto.IsValidDto)
                    .WithMessage("Request dto sucks");
            }
        }


        public class Handler : BaseHandler<WhateverCommand, SampleDto>
        {
            public async override Task<SampleDto> Handle(WhateverCommand request, CancellationToken cancellationToken)
            {
                if(request._sampleDto.ThrowException)
                {
                    throw new Exception("This is an exceptionally good exception");
                }

                
                return request._sampleDto;
            }
        }
    }
}
