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
            bool isAdded = await _apiService.AddArticle(articleDTO);

            if (isAdded)
            {
                return Ok("Article has been added");
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
