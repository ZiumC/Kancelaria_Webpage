using GB_Webpage.Models;
using Microsoft.AspNetCore.Identity;

namespace GB_Webpage.Services
{
    public class UserService
    {
        public static bool VerifyUserPassword(LoginRequest currentUser, string passedPassword, string salt)
        {
            var hasher = new PasswordHasher<LoginRequest>();

            var hashedCurrentPassword = hasher.HashPassword(currentUser, currentUser.Password + salt);

            PasswordVerificationResult result = hasher.VerifyHashedPassword(currentUser, hashedCurrentPassword, passedPassword + salt);

            return result.ToString().Equals("Success");
        }
    }
}
