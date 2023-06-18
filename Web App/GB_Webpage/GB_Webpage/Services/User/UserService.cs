using GB_Webpage.DTOs;
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
        public bool VerifyPassword(LoginRequestDTO currentUser, string passedPassword, string salt)
        {
            var hasher = new PasswordHasher<LoginRequestDTO>();

            var hashedCurrentPassword = hasher.HashPassword(currentUser, currentUser.Password + salt);

            PasswordVerificationResult result = hasher.VerifyHashedPassword(currentUser, hashedCurrentPassword, passedPassword + salt);

            return result.ToString().Equals("Success");
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

        public string GenerateAccessToken(string secretSignature, string user, string issuer, string audience, int daysValid)
        {
            var claims = new Claim[]
               {
                    new Claim(ClaimTypes.Name, user),
                    new Claim(ClaimTypes.Role, "User")
               };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretSignature));

            SigningCredentials creditionals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer,
                    audience,
                    claims,
                    expires: DateTime.UtcNow.AddDays(daysValid),
                    signingCredentials: creditionals
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public bool ValidateTokens(string secretSignature, JwtDTO jwt, string issuer)
        {
            SecurityToken validatedToken;

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.FromMinutes(1),
                ValidIssuer = issuer,
                ValidAudience = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretSignature))
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



    }
}
