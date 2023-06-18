using GB_Webpage.Models;

namespace GB_Webpage.Services.Database.Users
{
    public interface IApiUsersService
    {
        public Task<BlockedUserModel?> GetUserDataFromBlacklistAsync(string userName);
        public Task<bool> AddUserToBlocklistAsync(string userName, int attempsLeft);
        public Task<bool> UpdateUserInBlacklistAsync(int blockedUserId, BlockedUserModel blockedUserData);
    }
}
