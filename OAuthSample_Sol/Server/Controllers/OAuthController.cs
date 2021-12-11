using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("[controller]/[action]")]
    public class OAuthController : ControllerBase
    {
        private JwtService _jwtService;
        private FormBuilderService _formService;
        public JwtService JwtService => _jwtService ?? (JwtService)HttpContext.RequestServices.GetService(typeof(JwtService));
        public FormBuilderService FormService => _formService ?? (FormBuilderService)HttpContext.RequestServices.GetService(typeof(FormBuilderService));

        [HttpGet]
        [Authorize]
        public IActionResult Secret()
        {
            var result = new { HappinessIs = "Music" };
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Authorize(
               string response_type,   // auth flow type
               string client_id,       // duh
               string redirect_uri,    // duh
               string scope,           // required info -> email, password, whatever
               string state)           // random string generated to confirm that we are going back to the same client
        {
            // add scope, state if you wish
            var form = await FormService.BuildForm(redirect_uri, state, "mycode");
            
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = form
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authorize([FromBody] OAuthorizationModel authModel)
        {
            // validate authModel.Name (exists in DB, etc)
            var qbuilder = new QueryBuilder();

            qbuilder.Add("code", authModel.Code);
            qbuilder.Add("state", authModel.State);

            var redirection = $"{authModel.RedirectUri}{qbuilder}";

            return Redirect(redirection);
        }

        [HttpGet]
        public string Token(string grant_type, string code, string redirect_uri, string client_id)
        {
            var jwt = JwtService.GenerateJwt();
            var accessToken = new
            {
                access_token = jwt,
                token_type = "Bearer",
                expires_in = "30 mins"
            };

            var json = JsonSerializer.Serialize(accessToken);
            
            return json;
        }

        //[HttpPost]
        //public IActionResult Token(string grant_type, string code, string redirect_uri, string client_id)
        //{
        //    // validate params above
        //    // issue token
        //    var token = JwtService.GenerateJwt();

        //    return Ok(token);
        //}
    }
}
