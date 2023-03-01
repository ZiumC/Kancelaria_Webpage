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
        }

        public async Task<bool> AddArticle(ArticleDTO articleDTO)
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

                _context.SaveChanges();
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
            return await _context.Articles.Select(a => a).OrderByDescending(a => a.Date).ToListAsync();

        }
    }
}
