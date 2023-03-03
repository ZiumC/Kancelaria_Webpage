using GB_Webpage.DTOs;
using GB_Webpage.Models;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string _folder = "RefreshToken";
        private readonly string _issuer;
        private readonly string _secretSignature;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _issuer = _configuration["profiles:GB_Webpage:applicationUrl"].Split(";")[0];
            _secretSignature = _configuration["SecretSignatureKey"];
        }

        [HttpPost]
        [Route("login")]
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

                string refreshToken = UserService.GenerateRefreshToken();
                string accessToken = UserService.GenerateAccessToken(_secretSignature, request.Login, _issuer, _issuer);

                new DatabaseFileService(_folder).SaveFile<UserRefreshTokenModel>(new UserRefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserName = request.Login
                });

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }

            return Unauthorized("Login or password is wrong");

        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult RefreshToken(JwtDTO jwt)
        {
            UserRefreshTokenModel savedUserToken = new DatabaseFileService(_folder).ReadFile<UserRefreshTokenModel>();

            if (savedUserToken == null)
            {
                return NotFound("Refresh token not found");
            }

            if (!savedUserToken.RefreshToken.Equals(jwt.RefreshToken))
            {
                return StatusCode(452, "Tokens aren't valid to server");
            }

            bool areTokensValid = UserService.ValidateUserTokens(_secretSignature, jwt, _issuer);

            if (areTokensValid)
            {
                string refreshToken = UserService.GenerateRefreshToken();
                string accessToken = UserService.GenerateAccessToken(_secretSignature, savedUserToken.UserName, _issuer, _issuer);

                new DatabaseFileService(_folder).SaveFile<UserRefreshTokenModel>(new UserRefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserName = savedUserToken.UserName
                });

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }

            return StatusCode(452, "Tokens aren't valid to server");

        }

    }
}
