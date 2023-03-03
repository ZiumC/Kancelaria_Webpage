using GB_Webpage.Models;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Mvc;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {

            LoginRequest currentUser = new LoginRequest
            {
                Login = _configuration["User:Login"],
                Password = _configuration["User:Password"]
            };

            if (!request.Login.Equals(currentUser.Login))
            {
                return Unauthorized("Login or password is wrong");
            }

            string salt = _configuration["User:salt"];

            if (UserService.VerifyUserPassword(currentUser, request.Password,salt))
            {
                return Ok();
            }

            return Unauthorized("Login or password is wrong");

        }


    }
}
