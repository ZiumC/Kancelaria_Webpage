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


        public async Task<IEnumerable<SuspendedUserModel>> GetAllSuspendedUsers() 
        {
            return await _context.BlockedUsers.ToListAsync();
        }

        public async Task<SuspendedUserModel?> GetUserDataFromBlacklistAsync(string userName)
        {
            SuspendedUserModel? userData = await _context.BlockedUsers
                .Where(u => u.Username.ToLower().Equals(userName.ToLower()))
                .Select(u => new SuspendedUserModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Attempts = u.Attempts,
                    DateFirstInvalidAttemp = u.DateFirstInvalidAttemp,
                    SuspendedDateTo = u.SuspendedDateTo,

                }).FirstOrDefaultAsync();

            return userData;
        }

        public async Task<bool> AddUserToBlocklistAsync(string userName)
        {
            try
            {
                var newDatabaseUser = _context.BlockedUsers.Add
                    (
                        new SuspendedUserModel
                        {
                            Username = userName,
                            DateFirstInvalidAttemp = DateTime.UtcNow,
                            SuspendedDateTo = null
                        }
                    );

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetMethodName()));
                return false;
            }
            return true;
        }


        public async Task<bool> UpdateUserInBlockedlistAsync(int blockedUserId, SuspendedUserModel blockedUserData)
        {
            try
            {
                var userQuery = await
                    (_context.BlockedUsers
                    .Where(bu => bu.Id == blockedUserId)
                    .FirstAsync());

                userQuery.Username = blockedUserData.Username;
                userQuery.Attempts = blockedUserData.Attempts;
                userQuery.DateFirstInvalidAttemp = blockedUserData.DateFirstInvalidAttemp;
                userQuery.SuspendedDateTo = blockedUserData.SuspendedDateTo;

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, LogFormatterService.GetMethodName()));
                return false;
            }
            return true;
        }
    }
}
