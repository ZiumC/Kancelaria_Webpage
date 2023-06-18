using GB_Webpage.DTOs;

namespace GB_Webpage.Services.User
{
    public interface IUserService
    {
        public string GenerateRefreshToken();
        public string GenerateAccessToken(string user);
        public bool ValidateTokens(JwtDTO jwt);
        public bool isUserValidFor(LoginRequestDTO loginRequest);
    }
}
