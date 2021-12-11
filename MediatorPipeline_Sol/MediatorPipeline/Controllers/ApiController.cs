using System.Threading.Tasks;
using MediatorPipeline.Commands;
using MediatorPipeline.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediatorPipeline.Controllers
{
    [Route("[controller]/[action]")]
    public class ApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Info()
        {
            var msg =
                "Try hitting following endpoints, and debug: \r\n" +
                "[api/valid] [api/invalid] [api/exceptional]";

            return Ok(msg);
        }


        [HttpGet]
        public async Task<IActionResult> Valid()
        {
            var dto = SampleDto.GetSelf(isValid: true, throwEx: false);
            var result = await _mediator.Send(new WhateverCommand(dto));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Invalid()
        {
            var dto = SampleDto.GetSelf(isValid: false, throwEx: false);
            var result = await _mediator.Send(new WhateverCommand(dto));

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Exceptional()
        {
            var dto = SampleDto.GetSelf(isValid: true, throwEx: true);
            var result = await _mediator.Send(new WhateverCommand(dto));

            return Ok(result);
        }
    }
}
