using GB_Webpage.Data;
using GB_Webpage.DTOs;
using GB_Webpage.Models;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Mvc;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private readonly IApiService _apiService;

        public ArticlesController(IApiService apiService)
        {
            _apiService = apiService;
        }


        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleDTO articleDTO)
        {
            bool isAdded = await _apiService.AddArticleAsync(articleDTO);

            if (isAdded)
            {
                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = new DatabaseFileService().SaveFile(articles);

                if (isSavedToFile)
                {
                    return Ok("Article has been added and SAVED to physical file");
                }
                else
                {
                    return Ok("Article has been added and NOT SAVED to physical file");
                }
            }

            return BadRequest("Article wasn't added");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            IEnumerable<ArticleModel> articles = await _apiService.GetAllArticlesAsync();

            if (articles != null)
            {
                return Ok(articles);
            }

            return NotFound("Articles not found");
        }
    }
}
