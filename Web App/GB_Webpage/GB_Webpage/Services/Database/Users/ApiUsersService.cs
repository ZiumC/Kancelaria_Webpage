using GB_Webpage.Data;
using GB_Webpage.Models;
using Microsoft.EntityFrameworkCore;

namespace GB_Webpage.Services.Database.Users
{
    public class ApiUsersService : IApiUsersService
    {
        private readonly ApiContext _context;
        private readonly ILogger<ApiUsersService> _logger;

        public ApiUsersService(ApiContext context, ILogger<ApiUsersService> logger)
        {
            _context = context;
            _context.Database.EnsureCreated();
            _logger = logger;
        }
        public async Task<BlockedUserModel?> GetUserDataFromBlacklistAsync(string userName)
        {
            BlockedUserModel? userData = await _context.BlockedUsers
                .Where(u => u.Username.ToLower().Equals(userName.ToLower()))
                .Select(u => new BlockedUserModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    DateFirstInvalidAttemp = u.DateFirstInvalidAttemp,
                    DateBlockedTo = u.DateBlockedTo,

                }).FirstOrDefaultAsync();

            return userData;
        }

        public async Task<bool> AddUserToBlocklistAsync(string userName, int attempsLeft)
        {
            

            throw new NotImplementedException();
        }


        public async Task<bool> UpdateUserInBlacklistAsync(BlockedUserModel blockedUser)
        {
            throw new NotImplementedException();
        }
    }
}
