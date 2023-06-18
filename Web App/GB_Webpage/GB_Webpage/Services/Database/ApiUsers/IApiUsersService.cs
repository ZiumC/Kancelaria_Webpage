using GB_Webpage.Models;

namespace GB_Webpage.Services.Database.Users
{
    public interface IApiUsersService
    {
        public Task<BlockedUserModel?> GetUserDataFromBlacklistAsync(string userName);
        public Task<bool> AddUserToBlocklistAsync(string userName);
        public Task<bool> UpdateUserInBlockedlistAsync(int blockedUserId, BlockedUserModel blockedUserData);
    }
}
