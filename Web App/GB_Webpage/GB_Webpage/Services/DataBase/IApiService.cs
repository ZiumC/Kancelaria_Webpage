using GB_Webpage.DTOs;
using GB_Webpage.Models;

namespace GB_Webpage.Services
{
    public interface IApiService
    {
        public Task<IEnumerable<ArticleModel>> GetAllArticlesAsync();
        public Task<ArticleModel?> GetArticleByIdAsync(int id);
        public Task<bool> DeleteArticleAsync(ArticleModel article);
        public Task<bool> AddArticleAsync(ArticleDTO articleDTO);
    }
}
