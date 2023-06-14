using GB_Webpage.Data;

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

        public async Task<bool> AddUserToBlocklistAsync(string userName, int attempCount, DateTime dateFrom)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetUserFataFromBlacklistAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserInBlacklistAsync(string userName, int attempCount, DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }
    }
}
