namespace GB_Webpage.Services.Database.Users
{
    public class ApiUsersService : IApiUsersService
    {

        public Task<bool> AddUserToBlocklistAsync(string userName, int attempCount, DateTime dateFrom)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetUserFataFromBlacklistAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserInBlacklistAsync(string userName, int attempCount, DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }
    }
}
