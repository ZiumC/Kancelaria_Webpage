using GB_Webpage.DTOs;
using GB_Webpage.Models;

namespace GB_Webpage.Services.Database.Articles
{
    public interface IApiArticlesService
    {
        public Task<IEnumerable<ArticleModel>> GetAllArticlesAsync();
        public Task<ArticleModel?> GetArticleByIdAsync(int id);
        public Task<bool> DeleteArticleAsync(ArticleModel article);
        public Task<bool> AddArticleAsync(ArticleDTO articleDTO);
        public Task<bool> UpdateArticleByIdAsync(int id, ArticleDTO article);
    }
}
