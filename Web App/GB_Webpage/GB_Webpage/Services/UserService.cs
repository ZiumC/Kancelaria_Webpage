using GB_Webpage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GB_Webpage.Services
{
    public class UserService
    {
        public static bool VerifyUserPassword(LoginRequestModel currentUser, string passedPassword, string salt)
        {
            var hasher = new PasswordHasher<LoginRequestModel>();

            var hashedCurrentPassword = hasher.HashPassword(currentUser, currentUser.Password + salt);

            PasswordVerificationResult result = hasher.VerifyHashedPassword(currentUser, hashedCurrentPassword, passedPassword + salt);

            return result.ToString().Equals("Success");
        }

        public static string GenerateRefreshToken() 
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

        public static string GenerateAccessToken(string secretSignature, string user ,string issuer, string audience) 
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
                    expires: DateTime.UtcNow.AddMinutes(2),
                    signingCredentials: creditionals
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        
    }
}
