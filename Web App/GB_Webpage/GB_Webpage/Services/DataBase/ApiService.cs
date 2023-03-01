using GB_Webpage.Data;
using GB_Webpage.DTOs;
using GB_Webpage.Models;
using Microsoft.EntityFrameworkCore;

namespace GB_Webpage.Services.DataBase
{
    public class ApiService : IApiService
    {

        private readonly ApiContext _context;

        public ApiService(ApiContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<bool> AddArticleAsync(ArticleDTO articleDTO)
        {
            try
            {

                var addArticle = _context.Set<ArticleModel>();
                addArticle.Add
                    (
                        new ArticleModel
                        {
                            Title = articleDTO.Title,
                            Description = articleDTO.Description,
                            Date = articleDTO.DateCreated
                        }
                    );

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<ArticleModel>> GetAllArticlesAsync()
        {
            return await _context.Articles
                .Select(a => a)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

        }

        public async Task<ArticleModel?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles
                .Select(a => a)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteArticleAsync(ArticleModel article)
        {
            try
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.ToString());
                return false; 
            }
            return true;
        }
    }
}
