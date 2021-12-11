using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("[controller]/[action]")]
    public class OAuthController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult GrantAccess()
        {
            return Ok();
        }

        //[HttpGet]
        //public IActionResult Callback([FromQuery] string code, [FromQuery] string state)
        //{
        //    var x = 0;
        //    return Ok();
        //}
    }
}
