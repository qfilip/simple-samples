using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAuth.Services;

namespace WebAuth.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApiController : ControllerBase
    {
        private readonly UserService userService;
        private readonly JwtService jwtService;

        public ApiController(UserService userService, JwtService jwtService)
        {
            this.userService = userService;
            this.jwtService = jwtService;
        }

        [HttpGet]
       public IActionResult MockUsers()
        {
            this.userService.MockUsers();
            return Ok();
        }

        public IActionResult GetToken(string username)
        {
            var user = userService.GetUser(username);
            var token = jwtService.GenerateToken(user);
            
            return Ok(token);
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetSecret()
        {
            return Ok("User secret");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminSecret()
        {
            return Ok("Admin secret");
        }
    }
}
