using GB_Webpage.Models;

namespace GB_Webpage.Services
{
    public interface IApiService
    {
        public Task<ArticleModel> GetAllArticles();
    }
}
