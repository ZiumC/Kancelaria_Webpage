using GB_Webpage.DTOs;
using GB_Webpage.Models;
using GB_Webpage.Services.Database.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GB_Webpage.Services.User
{
    public class UserService : IUserService
    {
        private readonly IApiUsersService _apiUsersService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        private readonly string _secretSignature;
        private readonly string _audience;
        private readonly string _issuer;
        private readonly string _salt;

        public UserService(ILogger<UserService> logger, IConfiguration configuration, IApiUsersService apiUsersService)
        {
            _apiUsersService = apiUsersService;
            _configuration = configuration;
            _logger = logger;

            _secretSignature = _configuration["ApplicationSettings:JwtSettings:SecretSignatureKey"];
            _audience = _configuration["ApplicationSettings:JwtSettings:Issuer"];
            _issuer = _configuration["ApplicationSettings:JwtSettings:Issuer"];
            _salt = _configuration["ApplicationSettings:UsersSettings:UserCreditionals:Salt"];
        }

        public string GenerateRefreshToken()
        {
            var refreshToken = "";
            using (var genNumbers = RandomNumberGenerator.Create())
            {
                byte[] array = new byte[512];
                genNumbers.GetBytes(array);
                refreshToken = Convert.ToBase64String(array);
            }

            return refreshToken;
        }


        public string GenerateAccessToken(string user)
        {
            int daysValid = 1;
            try
            {
                daysValid = int.Parse(_configuration["ApplicationSettings:JwtSettings:DaysValidToken"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetMethodName()));
            }

            var claims = new Claim[]
               {
                    new Claim(ClaimTypes.Name, user),
                    new Claim(ClaimTypes.Role, "User")
               };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretSignature));

            SigningCredentials creditionals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    _issuer,
                    _audience,
                    claims,
                    expires: DateTime.UtcNow.AddDays(daysValid),
                    signingCredentials: creditionals
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public bool ValidateTokens(JwtDTO jwt)
        {
            SecurityToken validatedToken;

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretSignature))
            };


            try
            {
                var claim = new JwtSecurityTokenHandler().ValidateToken(jwt.AccessToken, parameters, out validatedToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }

        public bool isUserValidFor(LoginRequestDTO loginRequest)
        {
            UserModel user = new UserModel
            {
                Login = _configuration["ApplicationSettings:UsersSettings:UserCreditionals:Login"],
                Password = _configuration["ApplicationSettings:UsersSettings:UserCreditionals:Password"]
            };

            if (!loginRequest.Login.Equals(user.Login))
            {
                
                _logger.LogWarning(LogFormatterService.FormatAction(
                    "User login",
                    $"Username is invalid! Passed login: {loginRequest.Login}",
                    LogFormatterService.GetMethodName())
                );
                return false;
            }

            if (!VerifyPassword(user, loginRequest.Password))
            {
                _logger.LogWarning(LogFormatterService.FormatAction(
                   "User login",
                   $"Password is invalid! Passed password: {loginRequest.Password}",
                   LogFormatterService.GetMethodName())
               );
                return false;
            }

            return true;
        }

        private bool VerifyPassword(UserModel user, string password)
        {
            var hasher = new PasswordHasher<UserModel>();

            var hashedCurrentPassword = hasher.HashPassword(user, user.Password + _salt);

            PasswordVerificationResult result = hasher.VerifyHashedPassword(user, hashedCurrentPassword, password + _salt);

            return result.ToString().Equals("Success");
        }
    }
}
