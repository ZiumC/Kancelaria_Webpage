using GB_Webpage.DTOs;

namespace GB_Webpage.Services.User
{
    public interface IUserService
    {
        public bool VerifyPassword(LoginRequestDTO currentUser, string passedPassword, string salt);
        public string GenerateRefreshToken();
        public string GenerateAccessToken(string secretSignature, string user, string issuer, string audience, int daysValid);
        public bool ValidateTokens(string secretSignature, JwtDTO jwt, string issuer);

    }
}
