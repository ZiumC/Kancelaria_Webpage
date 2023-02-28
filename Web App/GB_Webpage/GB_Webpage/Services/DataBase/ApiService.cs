using GB_Webpage.Data;
using GB_Webpage.Models;

namespace GB_Webpage.Services.DataBase
{
    public class ApiService : IApiService
    {

        private readonly ApiContext _context;

        public ApiService(ApiContext context)
        {
            _context = context;
        }

        public async Task<ArticleModel> GetAllArticles()
        {
            return new ArticleModel { Id = 1, Title = "Dupa", Description = "ADASDADSDa", Date = DateTime.Now };
        }
    }
}
