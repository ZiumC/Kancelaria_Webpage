namespace GB_Webpage.Services.Database.Users
{
    public interface IApiUsersService
    {
        public Task<bool> GetUserFataFromBlacklistAsync(string userName);
        public Task<bool> AddUserToBlocklistAsync(string userName, int attempCount, DateTime dateFrom);
        public Task<bool> UpdateUserInBlacklistAsync(string userName, int attempCount, DateTime dateFrom, DateTime dateTo);
    }
}
