using GB_Webpage.DTOs;
using GB_Webpage.Models;
using GB_Webpage.Services;
using GB_Webpage.Services.DatabaseFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GB_Webpage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseFileService _databaseFileService; 
        private readonly ILogger<ArticlesController> _articlesLogger;
        private readonly string _ArticlesFolder;

        public ArticlesController(ILogger<ArticlesController> articlesLogger, IDatabaseFileService databaseFileService, IApiService apiService, IConfiguration configuration)
        {
            _databaseFileService = databaseFileService;
            _articlesLogger = articlesLogger;
            _configuration = configuration;
            _apiService = apiService;
            _ArticlesFolder = _configuration["DatabaseStorage:ArticlesFolder"];
        }

        [HttpPut]
        [Route("update/{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateArticle(int id, ArticleDTO article)
        {
            _articlesLogger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            ArticleModel? model = await _apiService.GetArticleByIdAsync(id);

            if (model == null)
            {
                return NotFound($"Unable to find article. |{id}");
            }

            bool isUpdated = await _apiService.UpdateArticleByIdAsync(id, article);

            if (isUpdated)
            {
                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = _databaseFileService.SaveFile(articles, _ArticlesFolder);

                if (isSavedToFile)
                {
                    return Ok("Article has been updated. Changes HAS BEEN SAVED to physical file");
                }
                else
                {
                    return StatusCode(209, "Article has been updated. Changes HASN'T SAVED to physical file");
                }
            }
            else
            {
                return BadRequest($"Unable to update article. |{id}");
            }

        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            _articlesLogger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            ArticleModel? article = await _apiService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound($"Article not found. | {id}");
            }

            bool isDeleted = await _apiService.DeleteArticleAsync(article);

            if (isDeleted)
            {

                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = _databaseFileService.SaveFile(articles, _ArticlesFolder);

                if (isSavedToFile)
                {
                    return Ok($"Article has been deleted. Changes HAS BEEN SAVED to physical file.");
                }
                else
                {
                    return StatusCode(209, $"Article has been deleted. Changes HASN'T SAVED to physical file.");
                }
            }
            return BadRequest($"Unable to delete article. | {id}");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddArticle(ArticleDTO articleDTO)
        {
            _articlesLogger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            bool isAdded = await _apiService.AddArticleAsync(articleDTO);

            if (isAdded)
            {
                var articles = await _apiService.GetAllArticlesAsync();
                bool isSavedToFile = _databaseFileService.SaveFile(articles, _ArticlesFolder);

                if (isSavedToFile)
                {
                    return Ok("Article has been added and SAVED to physical file");
                }
                else
                {
                    return StatusCode(209, "Article has been added and NOT SAVED to physical file");
                }
            }

            return BadRequest("Article wasn't added");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            _articlesLogger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            IEnumerable<ArticleModel> articles = await _apiService.GetAllArticlesAsync();

            if (articles != null)
            {
                return Ok(articles);
            }

            return NotFound("Articles not found");
        }
    }
}
