using GB_Webpage.DTOs;
using GB_Webpage.Models;
using GB_Webpage.Services;
using GB_Webpage.Services.DatabaseFiles;
using Microsoft.AspNetCore.Mvc;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IDatabaseFileService _databaseFileService;
        private readonly string _refreshTokenFolder;
        private readonly string _issuer;
        private readonly string _secretSignature;
        private readonly int _daysValid;

        public UsersController(IConfiguration configuration, IDatabaseFileService databaseFileService)
        {
            _configuration = configuration;
            _databaseFileService = databaseFileService;

            _issuer = _configuration["profiles:GB_Webpage:applicationUrl"].Split(";")[0];
            _secretSignature = _configuration["SecretSignatureKey"];
            _daysValid = int.Parse(_configuration["profiles:GB_Webpage:DaysValidToken"]);
            _refreshTokenFolder = _configuration["Paths:DatabaseStorage:RefreshTokenFolder"];
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginRequestDTO request)
        {

            LoginRequestDTO currentUser = new LoginRequestDTO
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
                string accessToken = UserService.GenerateAccessToken(_secretSignature, request.Login, _issuer, _issuer, _daysValid);

                _databaseFileService.SaveFile<UserRefreshTokenModel>(new UserRefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserName = request.Login
                }, _refreshTokenFolder);

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }

            return Unauthorized("Login or password is wrong");

        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult RefreshToken(JwtDTO jwt)
        {
            UserRefreshTokenModel? savedUserToken = _databaseFileService.ReadFile<UserRefreshTokenModel>(_refreshTokenFolder);

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
                string accessToken = UserService.GenerateAccessToken(_secretSignature, savedUserToken.UserName, _issuer, _issuer, _daysValid);

                _databaseFileService.SaveFile<UserRefreshTokenModel>(new UserRefreshTokenModel
                {
                    RefreshToken = refreshToken,
                    UserName = savedUserToken.UserName
                }, _refreshTokenFolder);

                return Ok(new { accessToken = accessToken, refreshToken = refreshToken });
            }

            return StatusCode(452, "Tokens aren't valid to this server");

        }

    }
}
