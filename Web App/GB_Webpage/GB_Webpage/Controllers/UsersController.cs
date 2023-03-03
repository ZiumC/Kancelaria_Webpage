using GB_Webpage.Models;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string _folder = "RefreshToken";

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(LoginRequestModel request)
        {

            LoginRequestModel currentUser = new LoginRequestModel
            {
                Login = _configuration["User:Login"],
                Password = _configuration["User:Password"]
            };

            if (!request.Login.Equals(currentUser.Login))
            {
                return Unauthorized("Login or password is wrong");
            }

            string salt = _configuration["User:salt"];

            if (UserService.VerifyUserPassword(currentUser, request.Password, salt))
            {
                string secretSignature = _configuration["SecretSignatureKey"];
                string issuer = _configuration["profiles:GB_Webpage:applicationUrl"].Split(";")[0];

                string refreshToken = UserService.GenerateRefreshToken();
                string accessToken = UserService.GenerateAccessToken(secretSignature, request.Login, issuer, issuer);

                new DatabaseFileService(_folder).SaveFile<JwtModel>(new JwtModel
                {
                    AccessToken = "",
                    RefreshToken = refreshToken
                });

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }

            return Unauthorized("Login or password is wrong");

        }


    }
}
