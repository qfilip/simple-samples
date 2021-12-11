using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("[controller]/[action]")]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Secret()
        {
            return Ok();
        }
    }
}
