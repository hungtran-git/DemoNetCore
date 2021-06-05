using aspnet_core_3_jwt_authentication_tutorial_with_example_api.Helpers;
using aspnet_core_3_jwt_authentication_tutorial_with_example_api.Models;
using aspnet_core_3_jwt_authentication_tutorial_with_example_api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace aspnet_core_3_jwt_authentication_tutorial_with_example_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            Response.Cookies.Append("Token", response.Token);
            return Ok(JsonConvert.SerializeObject(Response.Cookies));
        }

        [Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("Token");
            return Ok(JsonConvert.SerializeObject(Response.Cookies));
        }
        [HttpGet("UnauthorizedPage")]
        public IActionResult UnauthorizedPage()
        {
            return Ok("Unauthorized page");
        }
    }
}
