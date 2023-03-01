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

        [HttpPut]
        [Route("update/{id:int}")]
        public async Task<IActionResult> UpdateArticle(int id, ArticleDTO article)
        {
            ArticleModel model = await _apiService.GetArticleByIdAsync(id);

            if (model == null)
            {
                return NotFound($"Unable to find article. |{id}");
            }

            bool isUpdated = await _apiService.UpdateArticleByIdAsync(id, article);

            if (isUpdated)
            {

                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = new DatabaseFileService().SaveFile(articles);

                if (isSavedToFile)
                {
                    return Ok("Article has been updated. Changes HAS BEEN SAVED to physical file");
                }
                else
                {
                    return Ok("Article has been updated. Changes HASN'T SAVED to physical file");
                }
            }
            else
            {
                return BadRequest($"Unable to update article. |{id}");
            }

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            ArticleModel article = await _apiService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound($"Article not found. |{id}");
            }

            bool isDeleted = await _apiService.DeleteArticleAsync(article);

            if (isDeleted)
            {

                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = new DatabaseFileService().SaveFile(articles);

                if (isSavedToFile)
                {
                    return Ok($"Article has been deleted. Changes HAS BEEN SAVED to physical file. |{id}");
                }
                else
                {
                    return Ok($"Article has been deleted. Changes HASN'T SAVED to physical file. |{id}");
                }
            }
            return BadRequest($"Unable to delete article. |{id}");
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
