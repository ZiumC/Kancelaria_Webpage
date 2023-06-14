﻿using GB_Webpage.Models;

namespace GB_Webpage.Services.Database.Users
{
    public interface IApiUsersService
    {
        public Task<BlockedUserModel?> GetUserFataFromBlacklistAsync(string userName);
        public Task<bool> AddUserToBlocklistAsync(BlockedUserModel blockedUser);
        public Task<bool> UpdateUserInBlacklistAsync(BlockedUserModel blockedUser);
    }
}
