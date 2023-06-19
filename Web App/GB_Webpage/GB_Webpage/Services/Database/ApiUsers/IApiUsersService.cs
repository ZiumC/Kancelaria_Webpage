using GB_Webpage.Models;

namespace GB_Webpage.Services.Database.Users
{
    public interface IApiUsersService
    {
        public Task<SuspendedUserModel?> GetUserDataFromBlacklistAsync(string userName);
        public Task<bool> AddUserToBlocklistAsync(string userName);
        public Task<bool> UpdateUserInBlockedlistAsync(int blockedUserId, SuspendedUserModel blockedUserData);
    }
}
