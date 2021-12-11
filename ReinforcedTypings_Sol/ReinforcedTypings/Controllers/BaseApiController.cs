using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReinforcedTypings.Dtos;

namespace ReinforcedTypings.Controllers
{
    public class BaseApiController : ControllerBase
    {
        [AngularMethod(typeof(BaseDto))]
        public async Task<IActionResult> Parameterless()
        {
            var result = await Task.FromResult(new BaseDto());
            return Ok(result);
        }

        [AngularMethod(typeof(BaseDto))]
        public async Task<IActionResult> Parametrized(Guid id)
        {
            var result = await Task.FromResult(new BaseDto());
            return Ok(result);
        }
    }
}
