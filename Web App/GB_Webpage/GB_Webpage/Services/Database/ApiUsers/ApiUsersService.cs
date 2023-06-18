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

        public async Task<bool> AddUserToBlocklistAsync(string userName, int attemps)
        {
            try
            {
                var newDatabaseUser = _context.BlockedUsers.Add
                    (
                        new BlockedUserModel
                        {
                            Username = userName,
                            Attemps = attemps,
                            DateFirstInvalidAttemp = DateTime.UtcNow,
                            DateBlockedTo = null
                        }
                    );

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetAsyncMethodName()));
                return false;
            }
            return true;
        }


        public async Task<bool> UpdateUserInBlacklistAsync(int blockedUserId, BlockedUserModel blockedUserData)
        {
            try
            {
                var userQuery = await
                    (_context.BlockedUsers
                    .Where(bu => bu.Id == blockedUserId)
                    .FirstAsync());

                userQuery.Username = blockedUserData.Username;
                userQuery.Attemps = blockedUserData.Attemps;
                userQuery.DateFirstInvalidAttemp = blockedUserData.DateFirstInvalidAttemp;
                userQuery.DateBlockedTo = blockedUserData.DateBlockedTo;

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetAsyncMethodName()));
                return false;
            }
            return true;
        }
    }
}
